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

    private Coroutine loadingRoutine;
    private void Start()
    {
        loading.gameObject.SetActive(true);
        var tween = textProgress.DOText("L O A D I N G . . .", 1f);
        tween.SetLoops(-1);
        StartCoroutine(SetLayout());
        loadingRoutine = StartCoroutine(LocalDB.Initialize(() =>
        {
            DOTween.KillAll();
            loading.gameObject.SetActive(false);
            StopCoroutine(loadingRoutine);
            GC.Collect();
        }));
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
            .ToArray();
        for (int i = 0; i < scenes.Length; i++)
        {
            var button = Instantiate(original, parent).GetComponent<Button>();
            AddListener(button, scenes[i]);
        }
        original.gameObject.SetActive(false);

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
        dropContents.options = options;
        dropContents.onValueChanged.AddListener((value) =>
        {
            var alphabet = (eAlphabet)(value * 2);
            GameManager.Instance.currentAlphabet = alphabet;
        });
    }
    private IEnumerator SetLayout()
    {
        var data = GameManager.Instance.alphabets;

        yield return LocalDB.Initialize(() => Debug.Log("끝!"));
        Debug.Log("다음단계");
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
}
