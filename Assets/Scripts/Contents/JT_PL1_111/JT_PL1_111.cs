using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class JT_PL1_111 : MultiAnswerContents<Question111, string>
{
    protected override int QuestionCount => 2;

    protected override eContents contents => eContents.JT_PL1_111;
    public Rocket rocket;
    public TextButton[] buttons;
    public Button buttonRocket;

    protected override void Awake()
    {
        base.Awake();
        buttonRocket.onClick.AddListener(PlayCurrentWord);
        for(int i = 0;i < buttons.Length; i++)
        {
            AddOnClickTextButtonListener(buttons[i]);
        }
    }
    protected override List<Question111> MakeQuestion()
    {
        int correctCount = 4;
        int incorrectCount = buttons.Length - correctCount;
        var list = new List<Question111>();
        for(int i = 0;i < QuestionCount; i++)
        {
            var corrects = GameManager.Instance.GetWords()
               .Where(x => x.First().ToString().ToUpper() == GameManager.Instance.currentAlphabet.ToString())
               .OrderBy(x => Guid.NewGuid().ToString())
               .Take(correctCount)
               .ToArray();
            var incorrects = GameManager.Instance.GetWords()
               .Where(x => x.First().ToString().ToUpper() != GameManager.Instance.currentAlphabet.ToString())
               .OrderBy(x => Guid.NewGuid().ToString())
               .Take(incorrectCount)
               .ToArray();
            list.Add(new Question111(corrects, incorrects));
        }
        return list;    
    }

    protected override void ShowQuestion(Question111 question)
    {
        var randomQuestions = question.RandomQuestions;
        for(int i=0;i < randomQuestions.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].Init(randomQuestions[i]);
            buttons[i].button.interactable = false;
            var rt = buttons[i].GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
        }
        CallRokect();
    }
    protected override void ShowResult()
    {
        rocket.audioPlayer.Stop();
        base.ShowResult();
    }
    private void AddOnClickTextButtonListener(TextButton button)
    {
        var window = rocket.mask.GetComponent<RectTransform>();
        var rt = button.GetComponent<RectTransform>();
        button.onClick += (value) =>
        {
            if (currentQuestion.currentCorrect == value)
            {
                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].button.interactable = false;

                PlayCurrentWord();

                var seq = DOTween.Sequence();

                var moveTween = rt.DOMove(window.position, 1f);
                moveTween.SetEase(Ease.Linear);
                var scaleTweenStart = rt.DOScale(Vector3.one * 1.2f, .5f);
                scaleTweenStart.SetEase(Ease.Linear);
                var scaleTweenEnd = rt.DOScale(Vector3.one * 0.3f, .5f);
                scaleTweenEnd.SetEase(Ease.Linear);

                seq.Append(scaleTweenStart);
                seq.Append(scaleTweenEnd);
                seq.Insert(0, moveTween);

                seq.onComplete += () =>
                {
                    button.gameObject.SetActive(false);
                    rocket.Away(value, () =>
                    {
                        AddAnswer(value);
                        Debug.LogFormat("현재 문제 : {0}/{1}\n현재 문제 풀이 상황 : {2}/{3}", 
                            currentQuestionIndex+1, QuestionCount,
                            currentQuestion.currentIndex+1, currentQuestion.correctCount
                            );
                        if (!CheckOver())
                            CallRokect();
                    });
                };
                seq.Play();
            }
        };
    }
    private void PlayCurrentWord()
    {
        var clip = GameManager.Instance.GetClipWord(currentQuestion.currentCorrect);
        audioPlayer.Play(clip);
    }
    private void CallRokect()
    {
        rocket.Call(() =>
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].button.interactable = true;
            PlayCurrentWord();
        });
    }
}
public class Question111 : MultiQuestion<string>
{
    public int currentIndex { get; private set; } = 0;
    public string currentCorrect => correct[currentIndex];
    
    public Question111(string[] correct, string[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(string answer) => currentCorrect == answer;
    public override void SetAnswer(string answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}
