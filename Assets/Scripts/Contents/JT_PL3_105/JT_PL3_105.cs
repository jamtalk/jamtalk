using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_105 : SingleAnswerContents<Question3_105, WordSource>
{
    protected override eContents contents => eContents.JT_PL3_105;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 5;

    public Mole[] moles;
    public MoleElement305[] elements;
    public RectTransform[] layouts;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < elements.Length; i++)
            elements[i].Init(moles[i]);
    }

    protected override List<Question3_105> MakeQuestion()
    {
        var questions = new List<Question3_105>();
        var currentWord = GameManager.Instance.GetDigraphs() // dirgaphs
            .Where(x => x.type == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();

        return questions;
    }

    protected override void ShowQuestion(Question3_105 question)
    {
        var tempLayouts = layouts
            .OrderBy(x => UnityEngine.Random.Range(0f, layouts.Length))
            .Take(3)
            .ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            var data = question.totalQuestion[i];
            elements[i].transform.position = tempLayouts[i].position;
            AddListener(elements[i], data);
        }
    }

    private void AddListener(MoleElement305 element, WordSource data)
    {
        var button = element.GetComponent<Button>();
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            if (element.text.text.Contains(GameManager.Instance.currentDigrpahs.ToString()))
            {                                                 
                AddAnswer(data);
                if (CheckOver())
                    ShowResult();
            }
            else
            {
                
            }
        });

    }



}


[Serializable]
public class Mole
{
    public eDigraphs digraphs;
    public Sprite color;
}

public class Question3_105 : SingleQuestion<WordSource>
{
    private Sprite spriteCorrect;
    private Sprite[] spriteQuestions;
    public Sprite[] SpriteQuestions
    {
        get
        {
            return spriteQuestions.Union(new Sprite[] { spriteCorrect })
                .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
                .ToArray();
        }
    }
    public Question3_105(WordSource correct, WordSource[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}