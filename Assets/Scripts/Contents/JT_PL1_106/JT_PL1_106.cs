using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public class JT_PL1_106 : SingleAnswerContents<Question106, WordSource>
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
        var correctWord = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f,100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x=>x!=GameManager.Instance.currentAlphabet)
                .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(4)
                .ToArray();
            questions.Add(new Question106(correctWord[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question106 question)
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            var data = question.totalQuestion[i];
            buttonQuestions[i].isOn = false;
            buttonQuestions[i].sprite = data.sprite;
            AddDoubleClickListener(buttonQuestions[i],data);
        }
        var phanics = GameManager.Instance.GetResources().AudioData.phanics;
        audioPlayer.Play(phanics);
        buttonPhanics.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown,eAlphabetType.Upper,GameManager.Instance.currentAlphabet);
        buttonPhanics.button.onClick.RemoveAllListeners();
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(phanics));
    }
    private void ResetQuestion()
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            buttonQuestions[i].isOn = false;
        }
        var phanics = GameManager.Instance.GetResources().AudioData.phanics;
        audioPlayer.Play(phanics);
        buttonPhanics.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown, eAlphabetType.Upper, GameManager.Instance.currentAlphabet);
        buttonPhanics.button.onClick.RemoveAllListeners();
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(phanics));
    }
    private void AddDoubleClickListener(DoubleClickButton button, WordSource data)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            for (int i = 0; i < buttonQuestions.Length; i++)
            {
                buttonQuestions[i].isOn = buttonQuestions[i] == button;
                audioPlayer.Play(data.clip);
            }
        });

        button.onClick.AddListener(() =>
        {
            if(currentQuestion.correct == data)
            {
                button.button.targetGraphic.gameObject.GetComponent<Image>().sprite = spritePop;
                //button.transform.GetChild(0).GetComponent<Image>().sprite = spritePop;
                button.image.gameObject.SetActive(false);
                audioPlayer.Play(1f, clipPop);
                var tween = button.GetComponent<RectTransform>().DOScale(1.5f, .25f);
                tween.SetLoops(2, LoopType.Yoyo);
                tween.SetEase(Ease.Linear);
                tween.onComplete += () =>
                {
                    AddAnswer(data);
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
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}
[Serializable]
public class Question106 : SingleQuestion<WordSource>
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
    public Question106(WordSource correct, WordSource[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x=>x.sprite).ToArray();
    }
}
