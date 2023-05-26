using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL2_109 : BaseThrowingAlphabet<VowelContentsSetting,VowelWordsData>
{
    protected override eContents contents => eContents.JT_PL2_109;

    protected override List<Question_ThrowerAlphabet<VowelWordsData>> MakeQuestion()
    {
        var longVowel = GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Long)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        var shortVowel = GameManager.Instance.GetResources().Vowels
            .Where(x => x.VowelType == eVowelType.Short)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();
        return new Question_ThrowerAlphabet<VowelWordsData>[]
        {
            new Question_ThrowerAlphabet<VowelWordsData>(longVowel),
            new Question_ThrowerAlphabet<VowelWordsData>(shortVowel)
        }.OrderBy(x => Random.Range(0f, 100f)).ToList();
    }
}
