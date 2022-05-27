using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public class JT_PL1_106 : SingleAnswerContents<Question106, string>
{
    protected override eContents contents => eContents.JT_PL1_106;

    protected override int QuestionCount => 4;

    public DoubleClickButton[] buttonQuestions;
    public ImageButton buttonPhanics;
    public Sprite spritePop;
    public AudioClip clipPop;

    protected override List<Question106> MakeQuestion()
    {
        var questions = new List<Question106>();
        var correctWord = GameManager.Instance.GetWords()
            .Where(x=>x.First().ToString().ToUpper()==GameManager.Instance.currentAlphabet.ToString())
            .OrderBy(x => Guid.NewGuid().ToString()).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.GetWords()
                .Where(x => x.First().ToString().ToUpper() != GameManager.Instance.currentAlphabet.ToString())
                .OrderBy(x => Guid.NewGuid().ToString()).ToArray()
                .Take(4)
                .ToArray();
            questions.Add(new Question106(correctWord[i], tmp));
        }
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
        audioPlayer.Play(GameManager.Instance.GetClipPhanics());
        buttonPhanics.SetSprite(GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown, eAlphbetType.Upper));
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.GetClipPhanics()));
    }
    private void ResetQuestion()
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            buttonQuestions[i].isOn = false;
        }
        audioPlayer.Play(GameManager.Instance.GetClipPhanics());
        buttonPhanics.SetSprite(GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown, eAlphbetType.Upper));
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
            if(currentQuestion.correct == button.image.sprite.name)
            {
                button.GetComponent<Image>().sprite = spritePop;
                button.image.gameObject.SetActive(false);
                audioPlayer.Play(1f, clipPop);
                var tween = button.GetComponent<RectTransform>().DOScale(1.5f, .25f);
                tween.SetLoops(2, LoopType.Yoyo);
                tween.SetEase(Ease.Linear);
                tween.onComplete += () =>
                {
                    AddAnswer(button.image.sprite.name);
                    if(!CheckOver())
                        button.image.gameObject.SetActive(true);
                };
                tween.Play();
            }
            else
            {
                ResetQuestion();
            }
        });
    }
    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetClipAct2(GameManager.Instance.currentAlphabet), base.ShowResult);
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
