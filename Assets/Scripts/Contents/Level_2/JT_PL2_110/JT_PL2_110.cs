using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL2_110 : BingoContents<AlphabetWordsData, WordBingoButton, Text, WordBingoBoard>
{
    protected override eContents contents => eContents.JT_PL2_110;

    protected override AlphabetWordsData[] correctsTarget
    {
        get
        {
            if(_correctsTarget == null)
            {
                _correctsTarget =  GameManager.Instance.alphabets
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .Where(x => x.Key == GameManager.Instance.currentAlphabet)
                .Where(x => x.key.Length < 6)
                .Distinct()
                .OrderBy(x => Random.Range(0f, 100f))
                .ToArray();
            }

            return _correctsTarget;
        }
    }

    public override AlphabetWordsData[] GetQuestionType()
    {
        return GameManager.Instance.alphabets
            .Select(x => GameManager.Instance.GetResources(x))
            .Where(x => !correctsTarget.Select(y => y.Key).Contains(x.Alphabet))
            .SelectMany(x=>x.Words)
            .Where(x => x.key.Length < 6)
            .Distinct()
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray(); 
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.clip);

    protected override bool IsCurrentAnswer(AlphabetWordsData value) => value.key == currentQuestion.key;
}