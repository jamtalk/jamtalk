using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_108 : BaseWitch<DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL3_108;

    protected override List<Question_Witch<DigraphsSource>> MakeQuestion()
    {
        var questions = new List<Question_Witch<DigraphsSource>>();
        words = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .OrderBy(x => Random.Range(0f, 100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Where(x => x != words[currentQuestionIndex])
                .Take(elements.Length - 1)
                .ToArray();

            questions.Add(new Question_Witch<DigraphsSource>(words[i], tmp));
        }
        return questions;
    }

    protected override void Speak()
    {
        base.Speak();
        currentQuestion.correct.PlayClip();
    }
}
