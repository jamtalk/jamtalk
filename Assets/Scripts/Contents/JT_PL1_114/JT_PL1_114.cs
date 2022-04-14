using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class JT_PL1_114 : SingleAnswerContents<Question114, string>
{
    public DropSapceShip_114 ship;
    public DragObject_114[] drags;
    public GameObject finger;
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_114;
    protected override void Awake()
    {
        base.Awake();
        ship.onInner += (value) =>
        {
            AddAnswer(value);
            if (!CheckOver())
                ship.OutObject(currentQuestion.alphabet, ()=>SetIntractable(true));
        };
        for(int i = 0; i< drags.Length; i++)
        {
            drags[i].onDrop += () =>
            {
                SetIntractable(false);
            };
            drags[i].onDrag += () =>
            {
                finger.gameObject.SetActive(false);
            };
            drags[i].onAnswer += (correct) =>
            {
                finger.gameObject.SetActive(!correct);
            };
        }
    }
    protected override List<Question114> MakeQuestion()
    {
        var correct = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(QuestionCount)
            .ToArray();
        var list = new List<Question114>();
        for(int i = 0;i < QuestionCount; i++)
        {
            var correctWord = GameManager.Instance.GetWords(correct[i])
            .OrderBy(y => UnityEngine.Random.Range(0f, 100f))
            .First();

            var incorrect = GameManager.Instance.alphabets
                .Where(x => !correct.Contains(x))
                .SelectMany(x => GameManager.Instance.GetWords(x))
                .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
                .Take(drags.Length - 1)
                .ToArray();

            list.Add(new Question114(correctWord, incorrect));
        }
        return list;
    }

    protected override void ShowQuestion(Question114 question)
    {
        ship.SetInner();
        ship.OutObject(question.alphabet, () =>
        {
            finger.gameObject.SetActive(true);
            SetIntractable(true);
        });
        var questions = question.questions.Union(new string[] { question.correct })
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .ToArray();
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i].Init(GameManager.Instance.GetSpriteWord(questions[i]));
        }
    }

    private void SetIntractable(bool intracable)
    {
        for (int i = 0; i < drags.Length; i++)
            drags[i].intracable = intracable;
    }
}
public class Question114 : SingleQuestion<string>
{
    public eAlphabet alphabet => (eAlphabet)Enum.Parse(typeof(eAlphabet), correct.First().ToString().ToUpper());
    public Question114(string correct, string[] questions) : base(correct, questions)
    {
    }
}
