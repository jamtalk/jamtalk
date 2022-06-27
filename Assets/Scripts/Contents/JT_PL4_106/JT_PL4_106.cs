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

        for (int i = 0; i < buttons.Length; i++)
            SetButtonAddListener(buttons[i], eDig[i]);
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == eDigraphs.OI) // temp data
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        SetCurrentColor();
    }

    private void SetButtonAddListener(Button button, eDigraphs digraphs)
    {
        // phanics 출력 , image 변경
        button.onClick.AddListener(() =>
        {
            if (current.type == digraphs)
            {
                index += 1;
                current.PlayAct();
            }

            if (CheckOver())
                ShowResult();
        });
        
    }

    private void SetCurrentColor()
    {
        var isCheck = current.value.Contains(current.type.ToString().ToLower());
        string value = string.Empty;

        if (!isCheck)
        {
            string temp = string.Empty;
            if (current.type == eDigraphs.OI)
                temp = "oy";
            else if (current.type == eDigraphs.EA)
                temp = "ee";

            value = current.value.Replace(temp,
                "<color=\"red\">" + temp + "</color>");
        }
        else
        {
            value = current.value.Replace(current.type.ToString().ToLower()
                , "<color=\"red\">" + current.type.ToString().ToLower() + "</color>");
        }

        currentText.text = value;
    }
}
