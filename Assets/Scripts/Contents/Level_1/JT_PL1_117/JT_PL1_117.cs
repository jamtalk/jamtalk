using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class JT_PL1_117 : BingoContents<AlphabetData, BingoButton, Image, BingoBoard>
{
    protected override eContents contents => eContents.JT_PL1_117;
    
    protected override AlphabetData[] correctsTarget =>
        new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
        .Select(x=>GameManager.Instance.GetResources(x))
        .ToArray();

    public override AlphabetData[] GetQuestionType()
    {
        return GameManager.Instance.alphabets
            .Where(x => !correctsTarget.Select(y=>y.Alphabet).Contains(x))
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .Select(x=>GameManager.Instance.GetResources(x))
            .ToArray();
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.AudioData.clip);

    protected override bool IsCurrentAnswer(AlphabetData value) => value.Alphabet == currentQuestion.Alphabet;
}
