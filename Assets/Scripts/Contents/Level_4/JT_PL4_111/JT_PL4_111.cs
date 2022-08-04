using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL4_111 : BaseMatchSentances<DigraphsSentanceData>
{
    protected override eContents contents => eContents.JT_PL4_111;
    protected override void GetSentance()
    {
        words = GameManager.Instance.GetDigraphsSentance()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();
    }
}