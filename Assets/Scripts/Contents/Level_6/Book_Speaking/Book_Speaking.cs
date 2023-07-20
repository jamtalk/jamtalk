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
    public Button buttonSTT;
    private Tween buttonTween;
    private VoiceRecorder recorder => buttonSTT.GetComponent<VoiceRecorder>();
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
        currentPage = 1;
        currentPriority = 0;
        buttonSTT.onClick.AddListener(RecordAction);
        recorder.onSTTResult += (success, value) =>
        {
            PopupManager.Instance.Close();
            if (success)
            {
                currentPriority += 1;
                if (!data[currentPage].ContainsKey(currentPriority))
                    ShowResult();
                else
                    ShowQuestion();
            }
        };
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        ShowQuestion();
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

    private void PlayCorrect()
    {
        AndroidPluginManager.Instance.PlayTTS(currentSentance.value);
    }
    private void RecordAction()
    {
        recorder.RecordOrSendSTT();
        var isRecord = Microphone.IsRecording(recorder.deviceName);
        if (isRecord)
            PlayButtonTween();
        else
            StopButtonTween();
    }

    private void PlayButtonTween()
    {
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }
        buttonTween = buttonSTT.GetComponent<RectTransform>().DOScale(1.5f, 1f);
        buttonTween.SetEase(Ease.Linear);
        buttonTween.SetLoops(-1, LoopType.Yoyo);
        buttonTween.onKill += () => buttonSTT.GetComponent<RectTransform>().localScale = Vector3.one;
        buttonTween.Play();
    }
    private void StopButtonTween()
    {
        PopupManager.Instance.ShowLoading();
        if (buttonTween != null)
        {
            buttonTween.Kill();
            buttonTween = null;
        }

        buttonSTT.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f);
    }
}
