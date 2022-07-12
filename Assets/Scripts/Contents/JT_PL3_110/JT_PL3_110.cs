using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_110 : BingoContents<DigraphsSource, DigraphsBingoButton, Text, DigraphsBingoBoard>
{
    protected override eContents contents => eContents.JT_PL3_110;

    protected override DigraphsSource[] correctsTarget =>
        GameManager.Instance.digrpahs
        .SelectMany(x => GameManager.Instance.GetDigraphs(x))
        .Where(x => x.type == GameManager.Instance.currentDigrpahs)
        .Where(x => x.value.Length < 6)
        .Distinct()
        .OrderBy(x => Random.Range(0f, 100f))
        .ToArray();

    public override DigraphsSource[] GetQuestionType()
    {
        return GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.type != GameManager.Instance.currentDigrpahs)
            .Where(x => x.value.Length < 6)
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.act);

    protected override bool IsCurrentAnswer(DigraphsSource value) => value == currentQuestion;
}
