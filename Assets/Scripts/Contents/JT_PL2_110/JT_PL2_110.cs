using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL2_110 : BingoContents<WordSource, WordBingoButton, Text, WordBingoBoard>
{
    protected override eContents contents => eContents.JT_PL2_110;
    
    protected override WordSource[] correctsTarget =>
        new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
        .SelectMany(x => GameManager.Instance.GetResources(x).Words)
        .Where(x => x.value.Length < 6)
        .Distinct()
        .OrderBy(x => Random.Range(0f, 100f))
        .ToArray();

    public override WordSource[] GetQuestionType()
    {
        return GameManager.Instance.alphabets
            .Select(x => GameManager.Instance.GetResources(x))
            .Where(x => !correctsTarget.Select(y => y.alphabet).Contains(x.Alphabet))
            .SelectMany(x=>x.Words)
            .Where(x => x.value.Length < 6)
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray(); 
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.clip);

    protected override bool IsCurrentAnswer(WordSource value) => value == currentQuestion;
}