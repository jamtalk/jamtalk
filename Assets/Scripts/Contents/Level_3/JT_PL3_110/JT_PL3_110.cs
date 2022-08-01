using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_110 : BingoContents<DigraphsWordsData, DigraphsBingoButton, Text, DigraphsBingoBoard>
{
    protected override eContents contents => eContents.JT_PL3_110;

    protected override DigraphsWordsData[] correctsTarget =>
        GameManager.Instance.digrpahs
        .SelectMany(x => GameManager.Instance.GetDigraphs(x))
        .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
        .Where(x => x.key.Length < 6)
        .Distinct()
        .OrderBy(x => Random.Range(0f, 100f))
        .ToArray();

    public override DigraphsWordsData[] GetQuestionType()
    {
        return GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
            .Where(x => x.key.Length < 6)
            .Distinct()
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.clip);

    protected override bool IsCurrentAnswer(DigraphsWordsData value) => value.key == currentQuestion.key;
}
