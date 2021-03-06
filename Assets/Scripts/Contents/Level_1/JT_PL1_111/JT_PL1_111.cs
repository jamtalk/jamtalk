using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL1_111 : MultiAnswerContents<Question111, AlphabetWordsData>
{
    protected override int QuestionCount => 2;
    public GameObject finger;

    protected override eContents contents => eContents.JT_PL1_111;
    public TextRocket rocket;
    public TextButton111[] buttons;
    public Button buttonRocket;

    protected override void Awake()
    {
        base.Awake();
        buttonRocket.onClick.AddListener(PlayCurrentWord);
        buttonRocket.onClick.AddListener(() =>
        {
            if (finger != null)
            {
                Destroy(finger);
                finger = null;
            }
        });
        for(int i = 0;i < buttons.Length; i++)
        {
            AddOnClickTextButtonListener(buttons[i]);
        }
    }
    protected override List<Question111> MakeQuestion()
    {
        int correctCount = 2;
        int incorrectCount = buttons.Length - correctCount;
        var list = new List<Question111>();
        for(int i = 0;i < QuestionCount; i++)
        {

            var corrects = new eAlphabet[] {GameManager.Instance.currentAlphabet,GameManager.Instance.currentAlphabet+1}
                .Select(x=>GameManager.Instance.GetResources(x).Words.OrderBy(x=>Random.Range(0f,100f)).First())
                .Take(correctCount)
                .ToArray();
            var incorrects = GameManager.Instance.alphabets
                .Where(x=>x!=GameManager.Instance.currentAlphabet)
                .Where(x=>x!= GameManager.Instance.currentAlphabet+1)
                .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(incorrectCount)
                .ToArray();
            list.Add(new Question111(corrects, incorrects));
        }
        return list;    
    }
    protected override eGameResult GetResult()
    {
        return eGameResult.Perfect;
    }
    protected override void ShowQuestion(Question111 question)
    {
        var randomQuestions = question.totalQuestion;
        for (int i=0;i < randomQuestions.Length; i++)
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
    private void AddOnClickTextButtonListener(TextButton111 button)
    {
        var window = rocket.mask.GetComponent<RectTransform>();
        var rt = button.GetComponent<RectTransform>();
        button.onClickData += (value) =>
        {
            PlayWord(value);
            if (currentQuestion.currentCorrect == value)
            {
                if (finger != null)
                    finger.gameObject.SetActive(false);

                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].button.interactable = false;

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
                    rocket.Away(value.key, () =>
                    {
                        AddAnswer(value);
                        Debug.LogFormat("???? ???? : {0}/{1}\n???? ???? ???? ???? : {2}/{3}", 
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
        PlayWord(currentQuestion.currentCorrect);
    }
    private void PlayWord(AlphabetWordsData word)=> audioPlayer.Play(word.clip);
    private void CallRokect()
    {
        if(finger != null)
            finger.gameObject.SetActive(false);
        rocket.Call(() =>
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].button.interactable = true;
            PlayCurrentWord();
            if (finger != null)
                finger.gameObject.SetActive(true);
        });
    }
}
public class Question111 : MultiQuestion<AlphabetWordsData>
{
    public int currentIndex { get; private set; } = 0;
    public AlphabetWordsData currentCorrect => correct[currentIndex];
    
    public Question111(AlphabetWordsData[] correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(AlphabetWordsData answer) => currentCorrect == answer;
    public override void SetAnswer(AlphabetWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}
