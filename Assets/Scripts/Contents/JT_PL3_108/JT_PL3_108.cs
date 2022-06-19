using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_108 : JT_PL2_108
{
    protected override eContents contents => eContents.JT_PL3_108;

    protected override List<Question2_108> MakeQuestion()
    {
        var questions = new List<Question2_108>();
        words = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(elements.Length - 1)
                .ToArray();
            questions.Add(new Question2_108(words[i], tmp));
        }
        return questions;
    }
}
