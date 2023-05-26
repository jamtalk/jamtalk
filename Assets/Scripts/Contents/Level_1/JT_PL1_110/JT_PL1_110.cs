using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL1_110 : BaseThrowingAlphabet<AlphabetContentsSetting,AlphabetWordsData>
{
    protected override eContents contents => eContents.JT_PL1_110;

    protected override List<Question_ThrowerAlphabet<AlphabetWordsData>> MakeQuestion()
    {
        return new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
            .SelectMany(x => GameManager.Instance.GetResources(x).Words.OrderBy(x => Random.Range(0f, 100f)).Take(QuestionCount / 2))
            .Select(x => new Question_ThrowerAlphabet<AlphabetWordsData>(x))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToList();
    }
}