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

    private bool isCnt = true;
    private int index = 0;
    private int indexCnt = 0;
    private Coroutine textCoroutine = null;

    protected override void Awake()
    {
        base.Awake();

        index = 0;
        Show();

        foreach (var item in btnCtr)
            item.nextAction += Show;
    }

    private void Show()
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
            typingText.text = string.Empty;
        }

        if (index < 0)
            return;

        if(indexCnt > 1)
        {
            indexCnt = 0;
            this.index++;
        }
        indexCnt++;

        if (index >= data.Length)
        {
            ShowResult();
            return;
        }

        player.Play(data[index].clip);
        screen.sprite = data[index].sprite;
        caption.text = data[index].value;
        SetTextColor(data[index].value);

        btnCtr[0].SetActive(isCnt);
        btnCtr[1].SetActive(!isCnt);
        isCnt = !isCnt;
    }


    private void SetTextColor(string value)
    {
        var valueList = value.Split('\x020');
        var back = "</color>";
        string result = string.Empty;

        for (int i = 0; i < valueList.Length; i++)
        {
            var front = "<color=" + richColor[Random.Range(0, richColor.Length)]+">";
            valueList[i] = front + valueList[i] + back;

            result += valueList[i] + " ";
        }

        textCoroutine = StartCoroutine(DoText(result, () =>
        {
            typingText.text = string.Empty;
        }));
    }

    private IEnumerator DoText(string value, TweenCallback callback = null)
    {
        yield return new WaitForSecondsRealtime(3f);

        Sequence seq = DOTween.Sequence();

        Tween tween;
        tween = typingText.DOText(value, data[index].clip.length);

        seq.Append(tween);

        seq.onComplete += callback;
        seq.Play();
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
