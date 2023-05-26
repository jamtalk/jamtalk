using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL5_111 : BaseMatchSentances<DigraphsContentsSetting, DigraphsSentanceData>
{
    protected override eContents contents => eContents.JT_PL5_111;
    protected override DigraphsSentanceData[] GetSentance()
    {
        return GameManager.Instance.GetDigraphsSentance()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();
    }
}
