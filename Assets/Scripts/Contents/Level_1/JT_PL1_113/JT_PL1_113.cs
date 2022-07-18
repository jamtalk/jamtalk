using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class JT_PL1_113 : SingleAnswerContents<Question113, eAlphabet>
{
    public EventSystem eventSystem;
    public Thrower113 thrower;
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_113;
    public Charactor113[] charactors;
    public Item113[] items;
    public Sprite[] spritesProduct;
    protected override void Awake()
    {
        base.Awake();
        for(int i = 0;i < charactors.Length; i++)
        {
            AddCharactorListener(charactors[i]);
        }
    }
    private void AddCharactorListener(Charactor113 item)
    {
        item.onAway += () =>
        {
            AddAnswer(currentQuestion.correct);
        };
    }
    protected override List<Question113> MakeQuestion()
    {
        var alphabets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        if(GameManager.Instance.currentAlphabet < eAlphabet.C)
        {
            alphabets = alphabets
            .SelectMany(x => new eAlphabet[] { x, x })
            .ToArray();
        }
        else
        {
            var preAlhpabets = GameManager.Instance.alphabets.Where(x => x < GameManager.Instance.currentAlphabet)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(QuestionCount - alphabets.Length);
            alphabets = alphabets.Union(preAlhpabets).ToArray();
        }

        alphabets = alphabets.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        var list = new List<Question113>();
        for(int i = 0;i < QuestionCount; i++)
        {
            var incorrect = GameManager.Instance.alphabets
                .Where(x => !alphabets.Contains(x))
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(10)
                .ToArray();

            list.Add(new Question113(alphabets[i], incorrect));
        }
        return list;
    }

    protected override void ShowQuestion(Question113 question)
    {
        eventSystem.enabled = true;
        items = items.
            OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        var randomQuestion = question.totalQuestion;
        var ch = charactors[Random.Range(0, charactors.Length)];
        ch.Init(question.correct);

        for (int i = 0;i < items.Length; i++)
        {
            eAlphabet? value = null;
            if (i < randomQuestion.Length)
                value = randomQuestion[i];
            items[i].Init(value, spritesProduct.OrderBy(x => Random.Range(0f, 100f)).First());
            items[i].onClick += (item) =>
            {
                audioPlayer.Play(GameManager.Instance.GetResources(item.value).AudioData.phanics);
                if(item.value == currentQuestion.correct)
                {
                    ch.finger.gameObject.SetActive(false);
                    eventSystem.enabled = false;
                    thrower.Throw(item, ch.product.GetComponent<RectTransform>(), () => ch.SetProduct(item.product.sprite, item.value.ToString()));
                }
            };
        }

        ch.Call();
    }
}
public class Question113 : SingleQuestion<eAlphabet>
{
    public Question113(eAlphabet correct, eAlphabet[] questions) : base(correct, questions)
    {
    }
}
