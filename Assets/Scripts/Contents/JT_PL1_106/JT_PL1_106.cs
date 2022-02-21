using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class JT_PL1_106 : SingleAnswerContents<Question106, string>
{
    protected override eContents contents => eContents.JT_PL1_106;

    protected override int QuestionCount => 4;

    public DoubleClickButton[] buttonQuestions;
    public ImageButton buttonPhanics;


    protected override List<Question106> MakeQuestion()
    {
        var questions = new List<Question106>();
        var correctWord = GameManager.Instance.GetSpriteWord()
            .Select(x=>x.name)
            .OrderBy(x=>Random.Range(0f,100f))
            .Take(QuestionCount)
            .ToArray();

        var questionWords = Enum.GetNames(typeof(eAlphabet))
            .Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x))
            .Where(x => x != GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => GameManager.Instance.GetSpriteWord(x).Select(y => y.name))
            .Select(x => x.Take(4).ToArray())
            .ToArray();

        for(int i = 0;i < QuestionCount; i++)
            questions.Add(new Question106(correctWord[i], questionWords[i]));

        return questions;
    }

    protected override void ShowQuestion(Question106 question)
    {
        var sprites = question.SpriteQuestions;
        Debug.LogFormat("ÀÌ¹ÌÁö ·À½º : {0}\n¹öÆ° ·À½º : {1}", sprites.Length, buttonQuestions.Length);
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            buttonQuestions[i].isOn = false;
            buttonQuestions[i].SetSprite(sprites[i]);
            AddDoubleClickListener(buttonQuestions[i]);
        }
        buttonPhanics.SetSprite(GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.Brown, eAlphbetType.Upper));
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.GetClipPhanics()));
    }
    private void AddDoubleClickListener(DoubleClickButton button)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            for (int i = 0; i < buttonQuestions.Length; i++)
            {
                buttonQuestions[i].isOn = buttonQuestions[i] == button;
                audioPlayer.Play(GameManager.Instance.GetClipWord(button.image.sprite.name));
            }
        });

        button.onClick.AddListener(() =>
        {
            AddAnswer(button.image.sprite.name);
        });
    }
}
[Serializable]
public class Question106 : SingleQuestion<string>
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
    public Question106(string correct, string[] questions) : base(correct, questions)
    {
        spriteCorrect = GameManager.Instance.GetSpriteWord(correct);
        spriteQuestions = questions.Select(x => GameManager.Instance.GetSpriteWord(x)).ToArray();
    }
}
