using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public class JT_PL5_108 : SingleAnswerContents<Question5_108, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL1_106;

    protected override int QuestionCount => 4;

    public DoubleClickButton[] buttonQuestions;
    public ImageButton buttonPhanics;
    public Sprite spritePop;
    public AudioClip clipPop;

    private DigraphsWordsData current;

    protected override List<Question5_108> MakeQuestion()
    {
        var questions = new List<Question5_108>();
        var correctWord = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .OrderBy(x => Random.Range(0f, 100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(4)
                .ToArray();
            questions.Add(new Question5_108(correctWord[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question5_108 question)
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            var data = question.totalQuestion[i];
            buttonQuestions[i].isOn = false;
            buttonQuestions[i].sprite = data.sprite;
            AddDoubleClickListener(buttonQuestions[i], data);
        }
        audioPlayer.Play(question.correct.clip);
        current = question.correct;
        
        var temp = buttonPhanics.GetComponentInChildren<Text>();
        temp.text = question.correct.act;

        // image 변경
        //buttonPhanics.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown, eAlphabetType.Upper, GameManager.Instance.currentAlphabet);
        buttonPhanics.button.onClick.RemoveAllListeners();
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(question.correct.act));
    }
    private void ResetQuestion()
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            buttonQuestions[i].isOn = false;
        }
        audioPlayer.Play(current.clip);

        var temp = buttonPhanics.GetComponentInChildren<Text>();
        temp.text = GameManager.Instance.currentDigrpahs.ToString().ToLower();
        // image 변경
        //buttonPhanics.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown, eAlphabetType.Upper, GameManager.Instance.currentAlphabet);
        buttonPhanics.button.onClick.RemoveAllListeners();
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(current.clip));
    }
    private void AddDoubleClickListener(DoubleClickButton button, DigraphsWordsData data)
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
            if (currentQuestion.correct == data)
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
                    if (!CheckOver())
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
        // digraphs act3 사운드 변경 
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}
[Serializable]
public class Question5_108 : SingleQuestion<DigraphsWordsData>
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
    public Question5_108(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}
