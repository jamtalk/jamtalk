using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_121 : BaseMatchSentances<AlphabetContentsSetting,AlphabetSentanceData>
{
    protected override eContents contents => eContents.JT_PL1_121;
    protected override AlphabetSentanceData[] GetSentance()
    {
        return GameManager.Instance.GetResources().AlphabetSentances
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();
    }
}