using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL3_101;

    protected int index = 0;
    protected virtual int questionCount => 3;
    protected override int GetTotalScore() => index;
    protected override bool CheckOver() => questionCount == index;

    public DragElement301 dragElement;
    public PairColor[] colors;

    public Image[] colorImages;
    public Image resultColorImage;

    public Text[] texts;
    public Text resultText;

    protected override void Awake()
    {
        base.Awake();
        SetColors();
        dragElement.onDrop += OnDrop;
    }

    private void SetColors()
    {
        var reslutValue = "";
        for (int i = 0; i < colorImages.Length; i++)
        {
            colorImages[i].sprite = colors[index].colors[i].color;
            texts[i].text = colors[index].colors[i].alhpabet.ToString();
            reslutValue += colors[index].colors[i].alhpabet.ToString();
        }
        resultText.text = reslutValue;
        resultColorImage.sprite = colors[index].result;
    }

    private void OnDrop(DragElement301 target)
    {
        if (dragElement.isColors)
            index += 1;

        if (CheckOver())
            ShowResult();
        else
        { // ch , sh , th 사운드 추가 후 set color 진행 
            SetColors();
            dragElement.isColors = false;
            resultText.gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class PairColor
{
    public AlphabetColor[] colors;
    public Sprite result;
}

[Serializable]
public class AlphabetColor
{
    public eAlphabet alhpabet;
    public Sprite color;
}