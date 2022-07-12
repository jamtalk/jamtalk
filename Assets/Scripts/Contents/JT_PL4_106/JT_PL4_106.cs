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
    }

    private void MakeQuestion()
    {
        var random = eDig[Random.Range(0, 2)];
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == random)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        var temp = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type != random)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(2)
            .ToArray();

        var tempList = new List<DigraphsSource>();
        for(int i = 0; i < temp.Length; i ++)
            tempList.Add(temp[i]);
        tempList.Add(current);

        var list = tempList.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < doubleClick.Length; i++)
        {
            ButtonAddListener(doubleClick[i], list[i]);
            doubleClick[i].isOn = false;
        }
        SetCurrentColor();
    }
    private void ButtonAddListener(DoubleClickButton button, DigraphsSource data)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(data.act);
        });

        button.onClick.AddListener(() =>
        {
            if (current.type == data.type)
            {
                index += 1;
                audioPlayer.Play(current.clip, () =>
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
