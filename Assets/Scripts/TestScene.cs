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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
    public string root;
    public TextAsset ta;
    public class BookPageData
    {
        public class BookPageNumber
        {
            public int number;
            public int[] pages;

            public BookPageNumber(int number, int[] pages)
            {
                this.number = number;
                this.pages = pages;
            }
        }
        public string type;
        public BookPageNumber[] numbers;

        public BookPageData(string type, BookPageNumber[] numbers)
        {
            this.type = type;
            this.numbers = numbers;
        }
    }
    private void BookPages()
    {
        var dic = new Dictionary<eBookType, Dictionary<int, List<BookMetaData>>>();
        var books = JsonConvert.DeserializeObject<BookMetaData[]>(ta.text);
        foreach (var book in books)
        {
            var type = book.type;
            var number = book.bookNumber;

            if (!dic.ContainsKey(type))
                dic.Add(type, new Dictionary<int, List<BookMetaData>>());
            if (!dic[type].ContainsKey(book.bookNumber))
                dic[type].Add(book.bookNumber, new List<BookMetaData>());
            dic[type][number].Add(book);
        }

        var meta = new List<BookPageData>();
        foreach (var t in dic)
        {
            var type = t.Key;
            var numbers = t.Value.Keys.Select(x => new BookPageData.BookPageNumber(x, t.Value[x].Select(y => y.page).ToArray())).ToArray();
            var bookPageData = new BookPageData(type.ToString(), numbers);
            meta.Add(bookPageData);
            foreach (var n in t.Value)
            {
                var number = n.Key;
                var json = JArray.Parse(JsonConvert.SerializeObject(n.Value));
                foreach (var data in json)
                {
                    var value = (eBookType)data["type"].Value<int>();
                    data["type"] = value.ToString();
                }
                var di = new DirectoryInfo(string.Format("{0}/{1}", root, type));
                if (!di.Exists)
                    di.Create();
                var path = string.Format("{0}/{1}.json", di.FullName, n.Key);
                File.WriteAllText(path, json.ToString(), System.Text.Encoding.UTF8);
            }
        }

        File.WriteAllText(string.Format("{0}/meta.json", root), JArray.Parse(JsonConvert.SerializeObject(meta)).ToString(), System.Text.Encoding.UTF8);
    }
    private void Awake()
    {
        //var urls = GameManager.Instance.schema.data.bookData;
        //var dic = new Dictionary<eBookType, List<BookURLData>>();
        //for(int i= 0;i < urls.Length; i++)
        //{
        //    var data = urls[i];
        //    if (!dic.ContainsKey(data.key))
        //        dic.Add(data.key, new List<BookURLData>());

        //    dic[data.key].Add(data);
        //}

        //foreach(var bookType in dic)
        //{
        //    var type = bookType.Key;
        //    var list = bookType.Value.OrderBy(x=>x.number).ToArray();
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        var obj = JObject.Parse(JsonConvert.SerializeObject(list[i]));
        //        var key = (eBookType)obj["key"].Value<int>();
        //        obj["key"] = key.ToString();

        //        var path = string.Format("{0}/{1}/{2}", root, "URL",type.ToString());
        //        if (!Directory.Exists(path))
        //            Directory.CreateDirectory(path);

        //        path += string.Format("/{0}.json", list[i].number);
        //        File.WriteAllText(path, obj.ToString(),System.Text.Encoding.UTF8);
        //    }
        //}
    }
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
        dropContents.onValueChanged.RemoveAllListeners();
        switch (level)
        {
            case 1:
                dropContents.options = GetAlphabetsOption();
                dropContents.onValueChanged.AddListener((value) =>
                {
                    var alphabet = (eAlphabet)(value * 2);
                    GameManager.Instance.currentAlphabet = alphabet;
                    Debug.LogFormat("{0} 설정",GameManager.Instance.currentAlphabet);
                });
                var alphabet = (eAlphabet)(dropContents.value * 2);
                GameManager.Instance.currentAlphabet = alphabet;
                Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                break;
            case 2:
                dropContents.options = GetVowelOptions();
                dropContents.onValueChanged.AddListener((value) =>
                {
                    GameManager.Instance.currentAlphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), dropContents.options[dropContents.value].text);
                    Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                });
                GameManager.Instance.currentAlphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), dropContents.options[dropContents.value].text);
                Debug.LogFormat("{0} 설정", GameManager.Instance.currentAlphabet);
                break;
            default:
                dropContents.options = GetDigrpahsOption(level);
                dropContents.onValueChanged.AddListener((value) =>
                {
                    GameManager.Instance.currentDigrpahs = (eDigraphs)Enum.Parse(typeof(eDigraphs), dropContents.options[dropContents.value].text);
                    Debug.LogFormat("{0} 설정", GameManager.Instance.currentDigrpahs);
                });
                GameManager.Instance.currentDigrpahs = (eDigraphs)Enum.Parse(typeof(eDigraphs), dropContents.options[dropContents.value].text);
                Debug.LogFormat("{0} 설정", GameManager.Instance.currentDigrpahs);
                break;
        }
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

        var sprite = GameManager.Instance.GetResources().Words.First().sprite;

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
    private List<OptionData> GetDigrpahsOption(int level)
    {
        return GameManager.Instance.digrpahs
            .Where(x=>(int)x>=level*100)
            .Where(x=>(int)x<(level+1)*100)
            .Select(x => new OptionData(x.ToString()))
            .ToList();
    }
}
