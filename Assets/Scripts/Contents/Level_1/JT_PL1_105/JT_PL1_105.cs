using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_105 : STTContents<AlphabetWordsData, Image[]>
{
    protected override int QuestionCount => 3;

    protected override eContents contents => throw new System.NotImplementedException();

    protected override List<Question_STT<AlphabetWordsData>> MakeQuestion()
    {
        return new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
            .Select(x => GameManager.Instance.GetResources(x).Words.OrderBy(x => Random.Range(0f, 100f)).First())
            .Select(x => new Question_STT<AlphabetWordsData>(x))
            .ToList();
    }

    protected override void ShowValue(Question_STT<AlphabetWordsData> question)
    {
        valueViewer[0].sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, question.correct.Alphabet);
        valueViewer[1].sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Lower, question.correct.Alphabet);
    }
}