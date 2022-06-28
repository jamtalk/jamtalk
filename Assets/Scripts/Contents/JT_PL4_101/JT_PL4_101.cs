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

    public Slider[] sliders;
    public Text[] headTexts;
    public Image[] persnalImages;
    public Image[] addImages;
    public Animator[] anis;
    public Image successImages;

    private string[] values = { "ai", "oi", "ea", "ay", "oy", "ee" };
    private eDigraphs[] digraphs = { eDigraphs.AI, eDigraphs.OI, eDigraphs.EA };

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < anis.Length; i++)
            anis[i].SetBool("pacMan", true);

        MakeQuestion();

        sliders[0].onValueChanged.AddListener((value) => AddListener(value, sliders[0]));
    }

    private void MakeQuestion()
    {
        var values = new List<string>();

        foreach (var item in digraphs[index].ToString().ToLower())
            values.Add(item.ToString());

        headTexts[0].text = values[0];
        headTexts[1].text = values[1];
    }

    private void ShowQuestion()
    {

    }

    private void AddListener(float value, Slider slider)
    {
        if (value <= 0.001f)
            slider.gameObject.SetActive(false);
    }

}
