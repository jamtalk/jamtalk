using System.Collections;
using System.Collections.Generic;
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
    public Image successImages;

    private eDigraphs[] digraphs = { eDigraphs.AI, eDigraphs.OI, eDigraphs.EA };
    private List<ePairDigraphs> pairs = new List<ePairDigraphs>();

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
        pairs.Add(DigraphsSource.GetPair(digraphs[index]));

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
            if (value <= 0.001f)
            {
                slideCount += 1;
                slider.gameObject.SetActive(false);
                addObj.gameObject.SetActive(true);

                if (slideCount >= 2)
                {
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
            sliders[i].value = 1;
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
