using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Speaking : BaseContents<BookContentsSetting>
{
    public override eSceneName NextScene => eSceneName.AC_004;
    public EventSystem eventSystem;
    public Button button;
    public STTButton sttButton;
    private Tween sttTween;
    /// <summary>
    /// BookSentanceData[페이지][대사순서]
    /// </summary>
    public Dictionary<int,Dictionary<int, BookConversationData>> data;
    public int currentPage { get; private set; } = 1;
    public int currentPriority { get; private set; } = 0;
    public BookConversationData currentSentance => data[currentPage][currentPriority];

    protected override eContents contents => eContents.Book_Speaking;
    protected override bool CheckOver() => !data.ContainsKey(currentPage) || !data[currentPage].ContainsKey(currentPriority);
    protected override int GetTotalScore() => data.Values.Sum(x => x.Count);

    protected override bool includeExitButton => false;
    protected override bool isGuidence => false;
    protected override void Awake()
    {
        base.Awake();
        data = MakeQuestion();
        button.onClick.AddListener(PlayCorrect);
        sttButton.onSTT += OnSTT;
        sttButton.onRecord += OnRecord;
        currentPage = 1;
        currentPriority = 0;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        ShowQuestion();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnSTT(currentSentance.value);
        }
    }
    public Dictionary<int, Dictionary<int, BookConversationData>> MakeQuestion()
    {
        var books = GameManager.Instance.GetCurrentBooks();
        var dic = new Dictionary<int, Dictionary<int, BookConversationData>>();
        foreach (var book in books)
        {
            var tmp = book.conversations.OrderBy(x => x.priority).ToArray();
            for(int i = 0;i < tmp.Length; i++)
            {
                var page = book.page;
                var priority = i;
                if (!dic.ContainsKey(page))
                    dic.Add(page, new Dictionary<int, BookConversationData>());
                if (!dic[page].ContainsKey(priority))
                    dic[page].Add(priority, tmp[i]);
            }
            for (int i = 0; i < tmp.Length; i++)
            {
                SceneLoadingPopup.SpriteLoader.Add(tmp[i].spriteAsync);
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
        Debug.Log("결과 도착 : " + value);
        OnRecord(false);
        //if (currentSentance.value == value)
        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
         {
             currentPriority += 1;

             if (!data[currentPage].ContainsKey(currentPriority))
             {
                 ShowResult();
                 //currentPage += 1;
                 //if (data.ContainsKey(currentPage))
                 //    currentPriority = data[currentPage].Keys.Min();
             }

             //if (CheckOver())
             //    ShowResult();
             else
                 ShowQuestion();
         });
    }


    private void PlayCorrect()
    {
        AndroidPluginManager.Instance.PlayTTS(currentSentance.value);
    }
    private void OnRecord(bool isRecording)
    {
        if (isRecording)
        {
            sttTween = sttButton.transform.DOScale(1.5f, .5f);
            sttTween.SetEase(Ease.Linear);
            sttTween.SetLoops(-1, LoopType.Yoyo);
            sttTween.onKill += () => sttButton.transform.localScale = Vector3.one;
        }
        else
        {
            sttTween.Kill();
            eventSystem.enabled = true;
        }
    }
}
