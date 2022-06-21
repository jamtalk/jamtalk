using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_101 : SingleAnswerContents<Question3_101, WordsData.WordSources>
{
    protected override eContents contents => eContents.JT_PL3_101;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 3;

    private WordsData.WordSources[] words;


    protected override List<Question3_101> MakeQuestion()
    {
        var questions = new List<Question3_101>();
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
                .Take(words.Length - 1)
                .ToArray();
            questions.Add(new Question3_101(words[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question3_101 question)
    {
        
    }
}

public class Question3_101 : SingleQuestion<WordsData.WordSources>
{
    private Sprite spriteCorrect;
    private Sprite[] spriteQuestions;
    public Sprite[] SpriteQuestions
    {
        get
        {
            return spriteQuestions.Union(new Sprite[] { spriteCorrect })
                .OrderBy(x => Random.Range(0f, 100f))
                .ToArray();
        }
    }
    public Question3_101(WordsData.WordSources correct, WordsData.WordSources[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}