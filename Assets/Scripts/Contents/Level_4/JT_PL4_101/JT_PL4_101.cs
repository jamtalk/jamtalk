using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL4_101;
    protected override bool CheckOver() => questionCount == index;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;
    private int slideCount = 0;

    public Slider[] sliders;
    public Text[] headTexts;
    public GameObject[] addImages;
    public Animator[] anis;
    public Text[] digraphsTexts;
    public Text[] pairsTexts;
    public Sprite[] pacmanImages;
    public Image successImages;

    private eDigraphs[] digraphs = { eDigraphs.AI, eDigraphs.OI, eDigraphs.EA };
    private List<ePairDigraphs> pairs = new List<ePairDigraphs>();
    private DigraphsWordsData data;
    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < anis.Length; i++)
            anis[i].SetBool("pacMan", true);

        MakeQuestion();

        for(int i = 0; i < sliders.Length; i++)
            AddListener(sliders[i], addImages[i]);
    }

    private void MakeQuestion()
    {
        data = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == digraphs[index])
            .First();

        pairs.Add(ResourceSchema.GetPair(digraphs[index]));

        var digraph = digraphs[index].ToString().ToLower();
        var pair = pairs[index].ToString().ToLower();

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

    private void AddListener(Slider slider, GameObject addObj)
    {
        slider.onValueChanged.AddListener((value) =>
        {
            var temp = Convert.ToInt32(slider.value % 5);
            slider.image.sprite = pacmanImages[temp];

            if (value <= 0.001f)
            {
                slideCount += 1;
                slider.gameObject.SetActive(false);
                addObj.gameObject.SetActive(true);

                if (slideCount >= 2)
                {
                    var pair = GameManager.Instance.schema.GetDigrpahsAudio(data.PairDigrpahs);
                    audioPlayer.Play(data.audio.phanics, () => audioPlayer.Play(pair.phanics));
                    StartCoroutine(Reset());
                }
            }
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
            sliders[i].image.sprite = pacmanImages[4];
            addImages[i].gameObject.SetActive(false);
            sliders[i].gameObject.SetActive(true);
        }
        successImages.gameObject.SetActive(false);


        if (CheckOver())
            ShowResult();
        else
            MakeQuestion();
    }

}
