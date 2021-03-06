using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class JT_PL2_108 : BaseWitch<VowelWordsData>
{
    protected override eContents contents => eContents.JT_PL2_108;

    protected override List<Question_Witch<VowelWordsData>> MakeQuestion()
    {
        var questions = new List<Question_Witch<VowelWordsData>>();
        words = GameManager.Instance.GetResources().Vowels
            .OrderBy(x => Random.Range(0f, 100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        Debug.Log(words.Length);
        Debug.Log(QuestionCount);
        for (int i = 0; i < QuestionCount - 1; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(elements.Length - 1)
                .ToArray();
            questions.Add(new Question_Witch<VowelWordsData>(words[i], tmp));
            Debug.Log(tmp[i].key);
        }
        return questions;
    }

    protected override void Speak()
    {
        base.Speak();
        audioPlayer.Play(currentQuestion.correct.clip);
    }
}