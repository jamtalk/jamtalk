using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_107 : JT_PL1_107
{
    protected override eContents contents => eContents.JT_PL5_107;

    protected override void Awake()
    {
        GetWords();
    }

    protected override void GetWords()
    {
        var words = GameManager.Instance.GetResources().Words
            .Take(drops.Length)
            .ToArray();
        SetElement(words);
    }
}
