using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL4_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_101;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private int slideCount = 0;

    public Slider[] sliders;
    public Button[] buttons;
    public Text[] headTexts;
    public GameObject[] addImages;
    public Animator[] anis;
    public Text[] digraphsTexts;
    public Text[] pairsTexts;
    public Image successImages;

    private DigraphsWordsData data;
    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < anis.Length; i++)
            anis[i].SetBool("pacMan", true);

        MakeQuestion();

        for(int i = 0; i < buttons.Length; i++)
            AddListener(buttons[i], addImages[i], i);
    }

    private void MakeQuestion()
    {
        data = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .First();

        var pair = ResourceSchema.GetPair(data.Digraphs).ToString().ToLower();
        var digraph = data.digraphs.ToLower();

        for (int i = 0; i < digraphsTexts.Length; i++)
        {
            digraphsTexts[i].text = digraph;
            pairsTexts[i].text = pair;
        }

        var values = new List<string>();
        var temp = digraph + pair;

        foreach (var item in temp)
            values.Add(item.ToString());

        for (int i = 0; i < values.Count; i++)
            headTexts[i].text = values[i];
    }

    private void AddListener(Button buttons, GameObject addObj, int index)
    {
        buttons.onClick.AddListener(() =>
        {
            buttons.interactable = false;
            Slider slider = sliders[index];
            var seq = DOTween.Sequence();
            var tween = slider.DOValue(1f, 1.5f, true);
            seq.onComplete += () =>
            {
                slideCount += 1;
                slider.gameObject.SetActive(false);
                addObj.gameObject.SetActive(true);

                if (slideCount >= 2)
                {
                    var pair = GameManager.Instance.schema.GetDigrpahsAudio(data.PairDigrpahs);
                    audioPlayer.Play(data.audio.phanics);
                    StartCoroutine(Reset());
                    //audioPlayer.Play(data.audio.phanics, () => audioPlayer.Play(pair.phanics));
                }
            };
            seq.Append(tween);
        });
    }

    private IEnumerator Reset()
    {
        successImages.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        slideCount = 0;
        index += 1;

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = 25;
            addImages[i].gameObject.SetActive(false);
            sliders[i].gameObject.SetActive(true);
        }
        successImages.gameObject.SetActive(false);

        ShowResult();
        //if (CheckOver())
        //    ShowResult();
        //else
        //    MakeQuestion();
    }

}
