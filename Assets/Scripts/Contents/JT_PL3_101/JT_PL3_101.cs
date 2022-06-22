using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL3_101;

    protected int index = 0;
    protected virtual int questionCount => 5;
    protected override int GetTotalScore() => index;
    protected override bool CheckOver() => questionCount == index;

    public GameObject brush;
    public Sprite blueColor;
    public Sprite[] colors;
    public Sprite[] mixColors;


}