using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL2_112 : BaseMatchSentances<AlphabetSentanceData>
{
    protected override eContents contents => eContents.JT_PL2_112;
    protected override void GetSentance()
    {
        words = GameManager.Instance.GetResources().AlphabetSentances
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();
    }
}
