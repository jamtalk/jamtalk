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

    public Text currentText;
    public DoubleClickButton[] doubleClick;
    protected override void Awake()
    {
        base.Awake();

        MakeQuestion();    
    }

    private void MakeQuestion()
    {
        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        var temp = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
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
            var value = GameManager.Instance.schema.GetDigrpahsAudio(data.digraphs).phanics;
            audioPlayer.Play(value, () =>
            {
                button.SetFirstImages();
            });
        });

        button.onClick.AddListener(() =>
        {
            if (current.Digraphs == data.Digraphs)
            {
                Debug.Log(index);
                index += 1;
                button.SetLastImages();
                audioPlayer.Play(data.clip, () =>
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
            var temp = current.PairDigrpahs.ToString().ToLower();
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
