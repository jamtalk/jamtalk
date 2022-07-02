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
    public DoubleClickButton[] doubleClick;

    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();

        for (int i = 0; i < doubleClick.Length; i++)
            ButtonAddListener(doubleClick[i], eDig[i]);
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == eDig[Random.Range(0, 2)])
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        SetCurrentColor();
        for (int i = 0; i < doubleClick.Length; i++)
            doubleClick[i].isOn = false;
    }
    private void ButtonAddListener(DoubleClickButton button, eDigraphs digraphs)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            // phanics 출력
            Debug.Log(digraphs.ToString());
        });

        button.onClick.AddListener(() =>
        {
            if (current.type == digraphs)
            {
                index += 1;
                current.PlayClip(() =>
                {
                    if (CheckOver())
                        ShowResult();
                    else
                        MakeQuestion();
                });
            }
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
            else if (current.type == eDigraphs.AI)
                temp = "ay";

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
