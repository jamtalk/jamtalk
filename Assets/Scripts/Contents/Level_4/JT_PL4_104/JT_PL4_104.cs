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

    private List<eDigraphs> digraphsList = new List<eDigraphs>();
    private List<ePairDigraphs> pairList = new List<ePairDigraphs>();
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
                        selectElement.charactor.gameObject.SetActive(false);
                        element.charactor.gameObject.SetActive(false);

                        if (CheckOver())
                            ShowResult();
                    }
                    else
                    {
                        selectElement.Close();
                        element.Close();
                    }

                    buttonCount = 0;
                }

                eventSystem.enabled = true;
            });
        });
    }

    private IEnumerator Close(WordElement404[] elements)
    {
        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Close();
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

