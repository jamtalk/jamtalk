using GJGameLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OptionData = UnityEngine.UI.Dropdown.OptionData;
using UnityEngine.U2D;

public class TestScene : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Text textProgress;
    public Dropdown dropContents;
    public Dropdown dropLevel;
    public GameObject original;
    public Transform parent;
    public int[] levels;
    public GameObject loading;
    public Slider loadingBar;
    public eSceneName[] ignoreScenes;

    private Coroutine loadingRoutine;
    private void Start()
    {
        Application.targetFrameRate = 60;
        loading.gameObject.SetActive(true);
        var tween = textProgress.DOText("L O A D I N G . . .", 1f);
        tween.SetLoops(-1);
        StartCoroutine(SetLayout());
        var options = new List<OptionData>();
        levels = levels.OrderBy(x => x).ToArray();
        for (int i = 0; i < levels.Length; i++)
            options.Add(new OptionData(string.Format("{0} 단계", levels[i])));
        dropLevel.options = options;
        dropLevel.onValueChanged.AddListener(value => SelectLevel(value + 1));
    }
    public void AddListener(Button button, eSceneName scene)
    {
        button.onClick.AddListener(() =>
        {
            loading.SetActive(true);
            GJSceneLoader.Instance.LoadScene(scene);
        });
        button.transform.GetChild(0).GetComponent<Text>().text = scene.ToString();
    }
    public void SelectLevel(int value)
    {
        Clear();
        CreateInstances(value);
    }
    public void Clear()
    {
        var items = new List<GameObject>();
        for(int i = 1;i < scrollRect.content.childCount; i++)
            items.Add(scrollRect.content.GetChild(i).gameObject);

        for (int i = 0; i < items.Count; i++)
            DestroyImmediate(items[i]);
        GC.Collect();
    }
    public void CreateInstances(int level)
    {
        var scenes = Enum.GetNames(typeof(eSceneName))
            .Select(x => (eSceneName)Enum.Parse(typeof(eSceneName), x))
            .Where(x=>x.ToString().Contains("PL"+level))
            .Where(x=>!ignoreScenes.Contains(x))
            //.Where(x=>x != eSceneName.JT_PL2_102 && x!=eSceneName.JT_PL2_111)
            .ToArray();
        Debug.LogFormat("{0}개 생성",scenes.Length);
        for (int i = 0; i < scenes.Length; i++)
        {
            var button = Instantiate(original, parent).GetComponent<Button>();
            AddListener(button, scenes[i]);
            button.gameObject.SetActive(true);
        }
        original.gameObject.SetActive(false);
        switch (level)
        {
            case 1:
                dropContents.options = GetAlphabetsOption();
                break;
            case 2:
                dropContents.options = GetVowelOptions();
                break;
            default:
                dropContents.options = GetDigrpahsOption();
                break;
        }
        dropContents.onValueChanged.AddListener((value) =>
        {
            var alphabet = (eAlphabet)(value * 2);
            GameManager.Instance.currentAlphabet = alphabet;
        });
    }
    private IEnumerator SetLayout()
    {
        yield return new WaitForSecondsRealtime(1f);
        //var waitFrame = 3;
        //for (int i = 0; i < waitFrame; i++)
        //    yield return new WaitForEndOfFrame();
        loadingBar.gameObject.SetActive(true);
        var initialized = false;
        Debug.Log("이니셜라이징 대기중");
        GameManager.Instance.Initialize(() => initialized = true);
        while (!initialized) { yield return null; }
        Debug.Log("이니셜라이징 완료");
        loadingBar.gameObject.SetActive(false);

        loading.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        var size = scrollRect.GetComponent<RectTransform>().rect.size;
        var layout = scrollRect.content.GetComponent<GridLayoutGroup>();
        var count = layout.constraintCount;
        size.y = 150f;
        size.x -= (layout.spacing.x + layout.padding.left + layout.padding.right) * (count - 1);
        size.x /= count;
        layout.cellSize = size;
        SelectLevel(1);
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
    private List<OptionData> GetDigrpahsOption()
    {
        return GameManager.Instance.digrpahs.Select(x => new OptionData(x.ToString())).ToList();
    }
}
