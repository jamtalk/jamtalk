using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL4_110 : BingoContents<WordsData.WordSources, WordBingoButton, Text, WordBingoBoard>
{
    protected override eContents contents => eContents.JT_PL4_110;

    protected override WordsData.WordSources[] correctsTarget =>
        new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
        .SelectMany(x => GameManager.Instance.GetResources(x).Words)
        .Where(x => x.value.Length < 6)
        .ToArray();

    public override WordsData.WordSources[] GetQuestionType()
    {
        return GameManager.Instance.alphabets
            .Select(x => GameManager.Instance.GetResources(x))
            .Where(x => !correctsTarget.Select(y => y.alphabet).Contains(x.Alphabet))
            .SelectMany(x => x.Words)
            .Where(x => x.value.Length < 6)
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
    }

    protected override bool IsCurrentAnswer(WordsData.WordSources value) => value == currentQuestion;

    protected override AudioClip GetClip() => currentQuestion.clip;
}