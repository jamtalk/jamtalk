using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_109 : JT_PL1_110
{
    protected override eContents contents => eContents.JT_PL3_109;

    protected override void GetWord()
    {
        word = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
    }
}
