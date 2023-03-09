using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using DG.Tweening;
using static UnityEngine.Networking.UnityWebRequest;

public class Book_Listening : BaseContents
{
    protected override eContents contents => eContents.Book_Listening;
    protected override bool CheckOver() => GetTotalScore() == GetCorrectScore();
    protected override int GetTotalScore() => data.Length;
    protected override int GetCorrectScore() => index;
    protected override bool isGuidence => false;

    public AudioSinglePlayer player;
    public Image screen;
    public Text caption;
    public Text typingText;
    public VideoData[] data;
    public Listening_BtnCtr[] btnCtr;

    private int index = 0;
    private int indexCnt = 0;
    private Coroutine textCoroutine = null;

    protected override void Awake()
    {
        base.Awake();

        screen.sprite = data[index].sprite;
        foreach (var item in btnCtr)
            item.action += Show;
    }

    private void Show(int index, ePageButtonType type)
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
            typingText.text = string.Empty;
        }

        if (indexCnt >= 2 && type != ePageButtonType.replay)
        {
            indexCnt = 0;
            this.index++;

            if (index >= data.Length)
            {
                ShowResult();
                return;
            }
        }

        if (type == ePageButtonType.next)
            indexCnt++;
        else if (type == ePageButtonType.play)
        {
            this.index = 0;
            indexCnt = 0;
        }
        else if (type == ePageButtonType.previous)
        {
            indexCnt--;
            if(indexCnt <= 0)
            {
                indexCnt = 0;

                if(this.index > 0)
                    this.index--;
            }
        }

        Debug.Log("index : " + this.index + ", indexCnt : " +indexCnt );
        SetLayout(this.index, type, indexCnt);
    }

    private void SetLayout(int value, ePageButtonType type, int index)
    {
        player.Play(data[value].clip);
        screen.sprite = data[value].sprite;
        caption.text = data[value].value;
        SetTextColor(data[value].value);

        var isSubtitle = true;
        if (index <= 1 && (type == ePageButtonType.next || type == ePageButtonType.play))
        {
            btnCtr[0].SetActive(isSubtitle);
            btnCtr[1].SetActive(!isSubtitle);
        }
        else if (index == 2 && type == ePageButtonType.next)
        {
            btnCtr[0].SetActive(!isSubtitle);
            btnCtr[1].SetActive(isSubtitle);
        }

        if( type == ePageButtonType.previous)
        {
            if (value > 0)
            {
                btnCtr[0].SetActive(!btnCtr[0].gameObject.activeSelf);
                btnCtr[1].SetActive(!btnCtr[1].gameObject.activeSelf);
            }
        }
    }


    private void SetTextColor(string value)
    {
        var valueList = value.Split('\x020');
        var back = "</color>";
        string result = string.Empty;

        var colors = richColor
            .OrderBy(x => Random.Range(0, 100))
            .Take(valueList.Length)
            .ToList();

        for (int i = 0; i < valueList.Length; i++)
        {
            //var front = "<color=" + richColor[Random.Range(0, richColor.Length)]+">";
            var front = "<color=" + colors[i] + ">";
            valueList[i] = front + valueList[i] + back;

            result += valueList[i] + " ";
        }

        textCoroutine = StartCoroutine(DoText(valueList));
    }

    private IEnumerator DoText(string[] values, TweenCallback callback = null)
    {
        var waitTime = 2.5f;
        yield return new WaitForSecondsRealtime(waitTime);
        var delay = (data[index].clip.length - waitTime) / values.Length;

        foreach (var item in values)
        {
            typingText.text += item + " ";
            yield return new WaitForSecondsRealtime(delay);
        }
    }

    private string[] richColor = {
        "aqua",
        "brown",
        "green",
        "grey",
        "lightblue",
        "lime",
        "magenta",
        "maroon",
        "navy",
        "olive",
        "orange",
        "purple",
        "red",
        "yellow",
    };

    [System.Serializable]
    public class VideoData
    {
        public Sprite sprite;
        public AudioClip clip;
        public string value;
    }
}
