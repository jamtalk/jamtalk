using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL4_104 : BaseContents
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL4_104;
    protected override bool CheckOver() => elements.Length/2 == index;
    protected override int GetTotalScore() => elements.Length / 2;
    private int index = 0;
    private int buttonCount = 0;
    private WordElement404 selectElement;
    public WordElement404[] elements;


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;
        isNext = false;

        var digraphs = elements.Where(x => !x.data.isPair)
            .Where(x => !x.isOpen)
            .OrderBy(x => Random.Range(0, 100)).First();
        var pairDrigraphs = elements.Where(x => x.data.isPair)
            .Where(x => !x.isOpen)
            .OrderBy(x => Random.Range(0, 100)).First();

        WordElement404[] targets = { digraphs, pairDrigraphs };
        targets = targets.OrderBy(x => Random.Range(0, 100)).ToArray();

        foreach (var item in targets)
        {
            guideFinger.DoMove(item.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    ClickMotion(item);
                });
            });

            while (!isNext) yield return null;
            isNext = false;
        }

    }

    protected override void EndGuidnce()
    {
        foreach (var item in elements)
            item.Close();
        base.EndGuidnce();
        index = 0;
    }

    protected override void Awake()
    {
        base.Awake();
        
        MakeQuestion();
    }


    private void MakeQuestion()
    {
        var questions = GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(elements.Length / 2)
            .SelectMany(x => new Question4_104[] { new Question4_104(x, true), new Question4_104(x, false) })
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Init(questions[i]);
            AddListener(elements[i]);
            elements[i].Open();
        }
        StartCoroutine(Close(elements));
    }

    private void AddListener(WordElement404 element)
    {
        var button = element.button;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            ClickMotion(element);
        });
    }

    private void ClickMotion(WordElement404 element)
    {
        if (element.isOpen)
            return;

        buttonCount += 1;
        element.Open();

        eventSystem.enabled = false;

        audioPlayer.Play(element.data.data.audio.phanics, () =>
        {
            if (buttonCount == 1)
                selectElement = element;
            else
            {
                if (selectElement.data.value == element.data.value && selectElement.data.isPair != element.data.isPair)
                {
                    index += 1;
                    selectElement.Correct();
                    element.Correct();

                    if (CheckOver())
                        ShowResult();
                    else if (isGuide)
                        StartCoroutine(Close(elements, true));
                }
                else
                {
                    audioPlayer.PlayIncorrect();
                    selectElement.Close();
                    element.Close();
                }

                buttonCount = 0;
            }
            isNext = true;
            eventSystem.enabled = true;
        });
    }

    private IEnumerator Close(WordElement404[] elements, bool isMakeQuestion = false)
    {    
        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Close();
        }

        isNext = true;

        if (isMakeQuestion)
        {
            yield return new WaitForSecondsRealtime(1f);

            foreach (var items in elements)
                items.Correct();

            EndGuidnce();
            MakeQuestion();
        }
    }
}

public class Question4_104
{
    public DigraphsWordsData data { get; private set; }
    public eDigraphs value => data.Digraphs;
    public ePairDigraphs pair => data.PairDigrpahs;
    public bool isPair { get; private set; }
    public string text
    {
        get
        {
            string result;
            if (isPair)
                result = pair.ToString();
            else
                result = value.ToString();

            return result.ToLower();
        }
    }

    public Question4_104(DigraphsWordsData data, bool isPair)
    {
        this.data = data;
        this.isPair = isPair;
    }
}

