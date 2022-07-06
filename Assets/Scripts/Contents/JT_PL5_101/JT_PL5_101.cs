using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL5_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL5_101;
    protected override bool CheckOver() => index == questionCount;
    protected override int GetTotalScore() => questionCount;
    private int questionCount = 3;
    private int index = 0;

    public Image[] alphabetImages;

    private void MakeQuestion()
    {
        var digraphs = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type == GameManager.Instance.currentDigrpahs)
            .First();

        var temp = digraphs.ToString().ToCharArray();

        for(int i = 0; i < temp.Length; i++)
        {
            eAlphabet alphabets = (eAlphabet)Enum.Parse(typeof(eAlphabet), temp[i].ToString());
            var alphabet = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, alphabets);
            alphabetImages[i].sprite = alphabet;
        }
    }

    private void ShowQuestion()
    {

    }

    private void DoMove()
    {

    }
}
