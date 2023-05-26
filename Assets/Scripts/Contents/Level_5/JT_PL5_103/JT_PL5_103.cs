using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL5_103 : STTContents<DigraphsContentsSetting, DigraphsWordsData, Text>
{
    protected override int QuestionCount => 3;

    protected override eContents contents => eContents.JT_PL3_103;

    protected override List<Question_STT<DigraphsWordsData>> MakeQuestion()
    {
        return GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new Question_STT<DigraphsWordsData>(x))
            .ToList();
    }

    protected override void ShowValue(Question_STT<DigraphsWordsData> question)
    {
        valueViewer.text = question.correct.digraphs;
    }
}
