using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using OptionData = UnityEngine.UI.Dropdown.OptionData;

public class ContentsViewer : MonoBehaviour
{
    public Toggle[] levelToggles;
    public Button buttonLevel6;
    public GameObject orizinal;
    public List<ContentsButton> items;
    public GridLayoutGroup layout;
    public RectTransform content;
    public Dropdown dropCurrent;
    public Toggle[] toggles;

    private bool isFisrt = true;

    private void Awake()
    {
        StartCoroutine(SetLayout());
        //Show(Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x)).ToArray());

        for (int i = 0; i < toggles.Length; i++)
            AddToggleListener(toggles[i], i);
        var level = UserDataManager.Instance.CurrentChild.level;
#if DEPLOY
        for (int i = 0;i< levelToggles.Length; i++)
            levelToggles[i].interactable = i < level;
        buttonLevel6.interactable = level == 6;

        if (level == 6)
            Show(Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x)).ToArray());
        else
        {
            var enums = Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x));
            var min = (UserDataManager.Instance.CurrentChild.level - 1) * 100;
            Debug.LogFormat("현재 컨텐츠 : {0}/{1}", UserDataManager.Instance.CurrentChild.contents_title, UserDataManager.Instance.CurrentChild.GetContents());
            //var max = (int)UserDataManager.Instance.CurrentChild.GetContents();
            //enums = enums.Where(x => (int)x >= min && (int)x <= max);


            Show(enums.OrderBy(x => x).ToArray());
        }
#else
        var enums = Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x))
            .Where(x=>(int)x<600);
        Show(enums.OrderBy(x => x).ToArray());
#endif
    }

    private void AddToggleListener(Toggle toggle, int num)
    {

        toggle.onValueChanged.AddListener((value) =>
        {
            if (!value) return;

            if(num >=3 && num < 6)
            {
                GameManager.Instance.currentDigrpahs = (eDigraphs)(num * 100);
            }
            var enums = Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x));
            var ori = enums.Count();

            if (num != 0)
            {
                var min = num * 100;
                Debug.LogFormat("현재 컨텐츠 : {0}/{1}", UserDataManager.Instance.CurrentChild.contents_title, UserDataManager.Instance.CurrentChild.GetContents());
                var max = (int)UserDataManager.Instance.CurrentChild.GetContents();
                enums = enums.Where(x => (int)x >= min && (int)x <= max);
            }

            Show(enums.OrderBy(x => x).ToArray());

            if (0 < num && num < 6)
            {
                dropCurrent.gameObject.SetActive(true);
                CreateInstances(num);
            }
            else
                dropCurrent.gameObject.SetActive(false);
        });
    }

    public void CreateInstances(int level)
    {
        Debug.LogFormat("{0}레벨 선택", level);
        if (level > 0)
        {
            var targetContents = GameManager.Instance.GetEnums<eContents>()
                .Where(x => (int)x >= level * 100)
                .Where(x => (int)x < (level + 1) * 100);

            for (int i = 0; i < items.Count; i++)
                items[i].gameObject.SetActive(targetContents.Contains(items[i].contents));
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
                items[i].gameObject.SetActive(true);
        }

        dropCurrent.onValueChanged.RemoveAllListeners();
        switch (level)
        {
            case 1:
                dropCurrent.options = GetAlphabetsOption();
                dropCurrent.onValueChanged.AddListener((value) =>
                {
                    var alphabet = (eAlphabet)(value * 2);
                    GameManager.Instance.currentAlphabet = alphabet;
                    Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                });
                var alphabet = (eAlphabet)(dropCurrent.value * 2);
                GameManager.Instance.currentAlphabet = alphabet;
                Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                break;
            case 2:
                dropCurrent.options = GetVowelOptions();
                dropCurrent.onValueChanged.AddListener((value) =>
                {
                    GameManager.Instance.currentAlphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), dropCurrent.options[dropCurrent.value].text);
                    Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                });
                GameManager.Instance.currentAlphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), dropCurrent.options[dropCurrent.value].text);
                Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                break;
            default:
                dropCurrent.options = GetDigrpahsOption(level);
                dropCurrent.onValueChanged.AddListener((value) =>
                {
                    GameManager.Instance.currentDigrpahs = (eDigraphs)Enum.Parse(typeof(eDigraphs), dropCurrent.options[dropCurrent.value].text);
                    Debug.LogFormat("{0} 설정", GameManager.Instance.currentDigrpahs);
                });
                GameManager.Instance.currentDigrpahs = (eDigraphs)Enum.Parse(typeof(eDigraphs), dropCurrent.options[dropCurrent.value].text);
                Debug.LogFormat("{0} 설정", GameManager.Instance.currentDigrpahs);
                break;
        }
    }
    private List<OptionData> GetAlphabetsOption()
    {
        var options = new List<OptionData>();
        var enums = Enum.GetNames(typeof(eAlphabet))
            .Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x))
            .ToArray();
        for (int i = 0; i < enums.Length / 2; i++)
        {
            var first = i * 2;
            var next = first + 1;
            options.Add(new OptionData(string.Format("{0} ~ {1}", enums[first], enums[next])));
        }
        return options;
    }
    private List<OptionData> GetVowelOptions()
    {
        return GameManager.Instance.vowels.Select(x => new OptionData(x.ToString())).ToList();
    }
    private List<OptionData> GetDigrpahsOption(int level)
    {
        return GameManager.Instance.digrpahs
            .Where(x => (int)x >= level * 100)
            .Where(x => (int)x < (level + 1) * 100)
            .Select(x => new OptionData(x.ToString()))
            .ToList();
    }

    public void Show(params eContents[] contents)
    {
        CloseAll();
        for(int i = 0;i < contents.Length; i++)
        {
            if ((int)contents[i] < 600)
            {
                if (i >= items.Count && isFisrt)
                {
                    var item = Instantiate(orizinal, content).GetComponent<ContentsButton>();
                    item.name = contents[i].ToString();
                    Debug.LogFormat("{0} 아이템 추가", contents[i]);
                    items.Add(item);
                }
                Show(contents[i], items[i]);
            }
        }
        isFisrt = false;
    }
    private void Show(eContents contents, ContentsButton button)
    {
        Addressables.LoadAssetAsync<Sprite>(contents.ToString()).Completed += (sprite) =>
        {
            button.Init(contents, sprite.Result);
            button.gameObject.SetActive(true);
            button.beforeOnClick += () =>
            {
                Debug.LogFormat("현재 컨텐츠 : {0}({1})\n단어 : {2}({3})", contents, (int)contents, GameManager.Instance.currentDigrpahs, (int)GameManager.Instance.currentDigrpahs);
                if ((int)contents >= 300 && (int)contents < 600)
                {
                    if((int)contents < 400)
                    {
                        //3단계 일때
                        var dig = (int)GameManager.Instance.currentDigrpahs;
                        if (dig < 300 || dig >= 400)
                            GameManager.Instance.currentDigrpahs = (eDigraphs)300;
                    }
                    else if((int)contents < 500)
                    {
                        //3단계 일때
                        var dig = (int)GameManager.Instance.currentDigrpahs;
                        if (dig < 400 || dig >= 500)
                            GameManager.Instance.currentDigrpahs = (eDigraphs)400;
                    }
                    else if((int)contents < 600)
                    {
                        //5단계 일때
                        var dig = (int)GameManager.Instance.currentDigrpahs;
                        if (dig < 500 || dig >= 600)
                            GameManager.Instance.currentDigrpahs = (eDigraphs)500;
                    }
                }
                Debug.LogFormat("수정된 컨텐츠 : {0}({1})\n단어 : {2}({3})", contents, (int)contents, GameManager.Instance.currentDigrpahs, (int)GameManager.Instance.currentDigrpahs);
            };
#if DEPLOY
            if (UserDataManager.Instance.CurrentChild.GetContents() < contents)
                button.Disable();
#endif
        };
    }
    private void CloseAll()
    {
        for (int i = 0; i < items.Count; i++)
            items[i].Disable();
    }
    IEnumerator SetLayout()
    {
        int columCount = 7;
        yield return new WaitForEndOfFrame();
        var size = (content.rect.width - layout.spacing.x*(columCount-1)) / columCount;
        layout.cellSize = new Vector2(size, size);
    }
}
