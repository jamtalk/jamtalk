using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Speaking : BaseContents
{
    public override eSceneName NextScene => eSceneName.AC_004;
    public EventSystem eventSystem;
    public Button button;
    public STTButton sttButton;
    /// <summary>
    /// BookSentanceData[페이지][대사순서]
    /// </summary>
    public Dictionary<int,Dictionary<int,BookSentanceData>> data;
    public int currentPage { get; private set; } = 1;
    public int currentPriority { get; private set; } = 1;
    public BookSentanceData currentSentance => data[currentPage][currentPriority];

    protected override eContents contents => eContents.Book_Speaking;
    protected override bool CheckOver() => !data.ContainsKey(currentPage) || !data[currentPage].ContainsKey(currentPriority);
    protected override int GetTotalScore() => data.Values.Sum(x => x.Count);

    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(PlayCorrect);
        sttButton.onSTT += OnSTT;
        currentPage = 1;
        currentPriority = 1;
        data = MakeQuestion();
        ShowQuestion();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnSTT(currentSentance.value);
            eventSystem.enabled = true;
        }
    }
    public Dictionary<int, Dictionary<int, BookSentanceData>> MakeQuestion()
    {
        var bookTitles = GameManager.Instance.GetCurrentBook();
        var dic = new Dictionary<int, Dictionary<int, BookSentanceData>>();
        foreach (var bookTitle in bookTitles)
        {
            var tmp = bookTitle.book.OrderBy(x => x.priority).ToArray();
            for(int i = 0;i < tmp.Length; i++)
            {
                var page = bookTitle.page;
                var priority = i;
                if (!dic.ContainsKey(page))
                    dic.Add(page, new Dictionary<int, BookSentanceData>());
                if (!dic[page].ContainsKey(priority))
                    dic[page].Add(priority, tmp[i]);
            }
        }

        //var data = GameManager.Instance.GetCurrentBook().SelectMany(x => x.book)
        //    .ToArray();
        //var dic = new Dictionary<int, Dictionary<int, BookSentanceData>>();
        //for (int i = 0;i < data.Length; i++)
        //{
        //    var page = data[i].;
        //    var priority = data[i].priority;
        //    if (!dic.ContainsKey(page))
        //        dic.Add(page, new Dictionary<int, BookSentanceData>());
        //    if (!dic[page].ContainsKey(priority))
        //        dic[page].Add(priority, data[i]);
        //}
        return dic;
    }
    public void ShowQuestion()
    {
        button.image.sprite = currentSentance.sprite;
        button.image.preserveAspect = true;
        AndroidPluginManager.Instance.PlayTTS(currentSentance.value);
    }
    public void OnSTT(string value)
    {
        if (currentSentance.value == value)
            audioPlayer.Play(1f,GameManager.Instance.GetClipCorrectEffect(), () =>
            {
                currentPriority += 1;

                if (!data[currentPage].ContainsKey(currentPriority))
                {
                    currentPage += 1;
                    if (data.ContainsKey(currentPage))
                        currentPriority = data[currentPage].Keys.Min();
                }

                if (CheckOver())
                    ShowResult();
                else
                    ShowQuestion();
            });
    }


    private void PlayCorrect()
    {
        AndroidPluginManager.Instance.PlayTTS(currentSentance.value);
    }
}
