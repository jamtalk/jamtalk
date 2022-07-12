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
    private DigraphsWordsData current;
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
            .Where(x => x.Digraphs == random)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        var temp = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs != random)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(2)
            .ToArray();

        var tempList = new List<DigraphsWordsData>();
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
    private void ButtonAddListener(DoubleClickButton button, DigraphsWordsData data)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(data.act);
        });

        button.onClick.AddListener(() =>
        {
            if (current.Digraphs == data.Digraphs)
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
        var isCheck = current.key.Contains(current.Digraphs.ToString().ToLower());
        string value = string.Empty;

        if (!isCheck)
        {
            string temp = string.Empty;
            if (current.Digraphs == eDigraphs.OI)
                temp = "oy";
            else if (current.Digraphs == eDigraphs.EA)
                temp = "ee";
            else if (current.Digraphs == eDigraphs.AI)
                temp = "ay";

            value = current.key.Replace(temp,
                "<color=\"red\">" + temp + "</color>");
        }
        else
        {
            value = current.key.Replace(current.Digraphs.ToString().ToLower()
                , "<color=\"red\">" + current.Digraphs.ToString().ToLower() + "</color>");
        }

        currentText.text = value;
    }
}
