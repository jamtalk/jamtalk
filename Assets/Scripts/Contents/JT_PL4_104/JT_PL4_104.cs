using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_104 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_104;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;

    public WordElement404[] elements;

    private eDigraphs[] eDig = { eDigraphs.OI, eDigraphs.AI, eDigraphs.EA
                                ,eDigraphs.OW, eDigraphs.AW, eDigraphs.IE};

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();
    }


    private void MakeQuestion()
    {
        var data = eDig.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Init(data[i]);
            AddListener(elements[i]);
        }
    }

    private void AddListener(WordElement404 element)
    {
        var button = element.button;

        button.onClick.AddListener(() =>
        {
            // 해당 음가 출력
            element.Open();    
        });
    }
}

