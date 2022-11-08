using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ContentsViewer : MonoBehaviour
{
    public GameObject orizinal;
    public List<ContentsButton> items;
    public GridLayoutGroup layout;
    public RectTransform content;
    public Toggle[] toggles;
    private void Awake()
    {
        StartCoroutine(SetLayout());
        Show(Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x)).ToArray());
        for (int i = 0; i < toggles.Length; i++)
            AddToggleListener(toggles[i], i);
    }
    private void AddToggleListener(Toggle toggle,int num)
    {
        toggle.onValueChanged.AddListener((value) =>
        {
            var enums = Enum.GetNames(typeof(eContents)).Select(x => (eContents)Enum.Parse(typeof(eContents), x));
            var ori = enums.Count();
            if (value)
            {
                if(num != 0)
                {
                    var min = num * 100;
                    var max = (num + 1) * 100;
                    enums = enums.Where(x => (int)x >= min && (int)x < max);
                }
            }
            Debug.LogFormat("{0} -> {1}", ori, enums.Count());
            Show(enums.OrderBy(x => x).ToArray());
        });
    }

    public void Show(params eContents[] contents)
    {
        CloseAll();
        for(int i = 0;i < contents.Length; i++)
        {
            if (i >= items.Count)
            {
                var item = Instantiate(orizinal, content).GetComponent<ContentsButton>();
                items.Add(item);
            }
            Show(contents[i], items[i]);
        }
    }
    private void Show(eContents contents, ContentsButton button)
    {
        Addressables.LoadAssetAsync<Sprite>(contents.ToString()).Completed += (sprite) =>
        {
            button.Init(contents, sprite.Result);
            button.gameObject.SetActive(true);
        };
    }
    private void CloseAll()
    {
        for (int i = 0; i < items.Count; i++)
            items[i].gameObject.SetActive(false);
    }
    IEnumerator SetLayout()
    {
        int columCount = 7;
        yield return new WaitForEndOfFrame();
        var size = (content.rect.width - layout.spacing.x*(columCount-1)) / columCount;
        layout.cellSize = new Vector2(size, size);
    }
}
