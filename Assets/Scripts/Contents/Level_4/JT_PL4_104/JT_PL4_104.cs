using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_104 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_104;
    protected override bool CheckOver() => eDig.Length == index;
    protected override int GetTotalScore() => eDig.Length;
    private int index = 0;
    private int buttonCount = 0;
    private WordElement404 selectElement;
    public WordElement404[] elements;

    private eDigraphs[] eDig = { eDigraphs.OI, eDigraphs.AI, eDigraphs.EA };

    private List<eDigraphs> digraphsList = new List<eDigraphs>();
    private List<ePairDigraphs> pairList = new List<ePairDigraphs>();
    protected override void Awake()
    {
        base.Awake();
        
        MakeQuestion();
    }


    private void MakeQuestion()
    {
        var dig = eDig.Select(x => new Question4_104(x, true));
        var pair = eDig.Select(x => new Question4_104(x, false));
        var list = dig.Union(pair).OrderBy(x => Random.Range(0f, 100f)).ToList();

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Init(list[i]);
            AddListener(elements[i]);
            elements[i].Open();
        }
        StartCoroutine(Close(elements));
    }

    private void AddListener(WordElement404 element)
    {
        var button = element.button;

        button.onClick.AddListener(() =>
        {
            if (element.isOpen)
                return;

            buttonCount += 1;
            element.Open();

            Debug.LogFormat("pair : {0}, value : {1}"
                , element.data.pair.ToString(), element.data.value.ToString());

            if(buttonCount == 1)
            {
                selectElement = element;
                var value = string.Empty;
                if (selectElement.data.isPair)
                    value = GameManager.Instance.schema.GetDigrpahsAudio(selectElement.data.pair).phanics;
                else
                    value = GameManager.Instance.schema.GetDigrpahsAudio(selectElement.data.value).phanics;

                audioPlayer.Play(value);
            }
            else if( buttonCount > 1)
            {
                var value = string.Empty;
                if (element.data.isPair)
                    value = GameManager.Instance.schema.GetDigrpahsAudio(element.data.pair).phanics;
                else
                    value = GameManager.Instance.schema.GetDigrpahsAudio(element.data.value).phanics;

                audioPlayer.Play(value, () =>
                {
                    if (selectElement.data.value == element.data.value)
                    {
                        index += 1;
                        selectElement.charactor.gameObject.SetActive(false);
                        element.charactor.gameObject.SetActive(false);

                        if (CheckOver())
                            ShowResult();
                    }
                    else
                    {
                        var temp = new List<WordElement404>();
                        temp.Add(selectElement);
                        temp.Add(element);
                        var list = temp.ToArray();

                        StartCoroutine(Close(list));
                    }

                    buttonCount = 0;
                });
            }
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
    public eDigraphs value { get; private set; }
    public ePairDigraphs pair { get; private set; }
    public bool isPair { get; private set; }
    public string text { get; private set; }

    public Question4_104(eDigraphs dig, bool isPair)
    {
        this.value = dig;
        this.isPair = isPair;
        pair = ResourceSchema.GetPair(dig);
        if (isPair)
            text = pair.ToString().ToLower();
        else
            text = dig.ToString().ToLower();
    }
}

