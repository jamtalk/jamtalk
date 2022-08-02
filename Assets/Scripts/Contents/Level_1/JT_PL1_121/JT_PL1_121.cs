using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_121 : BaseMatchSentances<AlphabetSentanceData>
{
    protected override eContents contents => eContents.JT_PL1_121;
    protected override void GetSentance()
    {
        words = GameManager.Instance.GetResources().Sentances
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(questionCount)
            .ToArray();
        index = 0;
    }
}