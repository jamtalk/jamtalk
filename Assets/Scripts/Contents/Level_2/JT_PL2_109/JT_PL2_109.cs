using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL2_109 : JT_PL1_110
{
    protected override eContents contents => eContents.JT_PL2_109;

    protected override List<Question_PL1_110> MakeQuestion()
    {
        return GameManager.Instance.GetResources().Words
            .Select(x => new Question_PL1_110(x))
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(1)
            .ToList();
    }
}
