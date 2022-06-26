using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_106 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_106;
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private DigraphsSource current;
    private eDigraphs[] eDig = { eDigraphs.OI, eDigraphs.AI, eDigraphs.EA };

    public Text currentText;
    public Button[] buttons;

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();

        for (int i = 0; i < buttons.Length - 1; i++)
            buttons[i].onClick.AddListener(() => SetButtonAddListener(eDig[i]));
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == eDigraphs.OI) // temp data
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        currentText.text = current.value;
    }
    private void SetButtonAddListener(eDigraphs digraphs)
    {
        // phanics 출력 , image 변경

        Debug.Log("Click");
        if (current.type == digraphs)
        {
            index += 1;
            current.PlayAct();
        }

        if (CheckOver())
            ShowResult();
    }
}
