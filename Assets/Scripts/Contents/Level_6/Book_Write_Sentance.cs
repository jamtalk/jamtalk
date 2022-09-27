using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book_Write_Sentance : BaseMatchSentances<BookSentanceData>
{
    protected override int QuestionCount => 5;
    public override eSceneName NextScene => eSceneName.AC_004;
    protected override eContents contents => eContents.Book_Writing_Sentance;
    public Button button;
    public GameObject empty;
    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(() => PlayCurrentSentance());
    }
    protected override void ShowQuestion()
    {
        PlayCurrentSentance();
        button.image.sprite = currentSentance.sprite;
        button.image.preserveAspect = true;
        thrower.paths = CreatePath(currentSentance.value.Split(' ').Length);
        Debug.Log(thrower.paths.Length);
        base.ShowQuestion();
    }
    protected override void PlayCurrentSentance(Action onOver=null)
    {
        AndroidPluginManager.Instance.PlayTTS(currentSentance.value,onOver);
    }
    private RectTransform[] CreatePath(int length)
    {
        Debug.Log(length + "俺 积己");
        var childCount = thrower.transform.childCount;
        for(int i = 0;i < length - childCount; i++)
            Instantiate(empty, thrower.transform);
        var list = new List<RectTransform>();
        for(int i = 0;i < thrower.transform.childCount; i++)
        {
            var child = thrower.transform.GetChild(i);
            child.gameObject.SetActive(i < length);
            if (i < length)
                list.Add(child.GetComponent<RectTransform>());
        }
        Debug.Log(list.Count + "俺 积己 肯丰");
        return list.ToArray();
    }
    //IEnumerator SetSize(WordElement121[] words)
    //{
    //    yield return new WaitForEndOfFrame();
    //    for (int i = 0; i < words.Length; i++)
    //        words[i].SetSize();
    //}

    protected override BookSentanceData[] GetSentance()
    {
        return BookData.Instance.sentances
            .Where(x => x.Key == GameManager.Instance.currentBook)
            .Where(x => x.number == GameManager.Instance.currentBookNumber)
            .Take(QuestionCount)
            //.Select(x => new BookWriteSentanceQuestion(x))
            .ToArray();
    }
}
public class BookWriteSentanceQuestion : SingleQuestion<BookSentanceData>
{
    public BookWriteSentanceQuestion(BookSentanceData correct) : base(correct, new BookSentanceData[0])
    {
    }
}
