using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class JT_PL1_114 : SingleAnswerContents<Question114, AlphabetWordsData>
{
    public DropSapceShip_114 ship;
    public DragObject_114[] drags;
    public GameObject finger;
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_114;
    protected override void Awake()
    {
        base.Awake();
        ship.button.onClick.AddListener(() =>
        {
            if (finger != null)
            {
                Destroy(finger);
                finger = null;
            }
        });
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
                if(finger!=null)
                    finger.gameObject.SetActive(false);
            };
            drags[i].onAnswer += (correct) =>
            {
                if (finger != null)
                    finger.gameObject.SetActive(!correct);
            };
        }
    }
    protected override List<Question114> MakeQuestion()
    {
        var correct = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(QuestionCount/2)
            .SelectMany(x=>new eAlphabet[] {x,x})
            .ToArray();
        var list = new List<Question114>();
        for(int i = 0;i < QuestionCount; i++)
        {
            var correctWord = GameManager.Instance.GetResources(correct[i]).Words
            .OrderBy(y => UnityEngine.Random.Range(0f, 100f))
            .First();
            //var correctWord = GameManager.Instance.GetResources(eAlphabet.X).Words.Where(x => x.key == "fox").First();

            var incorrect = GameManager.Instance.alphabets
                .Where(x => !correct.Contains(x))
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
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
            if (finger != null)
                finger.gameObject.SetActive(true);
            SetIntractable(true);
        });
        var questions = question.questions.Union(new AlphabetWordsData[] { question.correct })
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .ToArray();
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i].Init(questions[i]);
        }
    }

    private void SetIntractable(bool intracable)
    {
        for (int i = 0; i < drags.Length; i++)
            drags[i].intracable = intracable;
    }
}
public class Question114 : SingleQuestion<AlphabetWordsData>
{
    public eAlphabet alphabet => correct.Key;
    public Question114(AlphabetWordsData correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
    }
}
