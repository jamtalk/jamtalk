using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL5_104 : MultiAnswerContents<AlphabetContentsSetting, Question5_104, DigraphsWordsData>
{
    protected override int QuestionCount => 3;
    public GameObject finger;

    protected override eContents contents => eContents.JT_PL5_104;
    public EventSystem eventSystem;
    public ImageRocket rocket;
    public DoubleClick504[] buttons;
    public Button buttonRocket;

    int correctCount = 1;
    bool isStop = false;
    bool isRoutine = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        while (!isStop) yield return null;


        for (int i = 0; i < buttons.Length; i++)
        {
            while (!isStop) yield return null;

            isRoutine = false;
            if (buttons[i].data == currentQuestion.currentCorrect)
            {
                guideFinger.gameObject.SetActive(true);
                guideFinger.DoMove(buttons[i].transform.position, () =>
                {
                    guideFinger.DoClick(() =>
                    {
                        buttons[i].isOn = true;
                        audioPlayer.Play(buttons[i].data.audio.phanics, () =>
                        {

                            guideFinger.DoClick(() =>
                            {
                                guideFinger.gameObject.SetActive(false);
                                CorrectButtonMotion(buttons[i]);
                            });
                        });
                    });
                });

                while (!isRoutine) yield return null;
                break;
            }
        }
    }


    protected override void OnAwake()
    {
        base.OnAwake();
        buttonRocket.onClick.AddListener(() => PlayCurrentWord());
        buttonRocket.onClick.AddListener(() =>
        {
            if (finger != null)
            {
                Destroy(finger);
                finger = null;
            }
        });
        for (int i = 0; i < buttons.Length; i++)
        {
            AddOnClickTextButtonListener(buttons[i]);
        }
    }
    protected override List<Question5_104> MakeQuestion()
    {
        int incorrectCount = buttons.Length - correctCount;
        var list = new List<Question5_104>();
        for (int i = 0; i < QuestionCount; i++)
        {
            var corrects = GameManager.Instance.digrpahs
               .SelectMany(x => GameManager.Instance.GetDigraphs(x))
               .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
               .OrderBy(x => Random.Range(0f, 100f))
               .Take(correctCount)
               .ToArray();

            var incorrects = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(incorrectCount)
                .ToArray();
            list.Add(new Question5_104(corrects, incorrects));
        }
        return list;
    }
    protected override eGameResult GetResult()
    {
        return eGameResult.Success;
    }
    protected override void ShowQuestion(Question5_104 question)
    {
        var randomQuestions = question.totalQuestion;
        for (int i = 0; i < randomQuestions.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].Init(randomQuestions[i]);
            buttons[i].name = randomQuestions[i].key;
            buttons[i].text.text = randomQuestions[i].digraphs;
            buttons[i].button.interactable = false;
            var rt = buttons[i].GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
        }
        rocket.text.text = question.currentCorrect.key;
        CallRokect();
    }
    protected override void ShowResult()
    {
        rocket.audioPlayer.Stop();
        base.ShowResult();
    }
    private void AddOnClickTextButtonListener(DoubleClick504 button)
    {
        button.onClickFirst.AddListener(() =>
        {
            if (button.data.Digraphs == currentQuestion.currentCorrect.Digraphs)
                audioPlayer.Play(button.data.audio.phanics);
            else
                audioPlayer.PlayIncorrect(button.data.audio.phanics);

        });
        button.onClickData += (value) =>
        {
            CorrectButtonMotion(button);
        };
    }
    private void CorrectButtonMotion(DoubleClick504 button)
    {
        var value = button.data;
        var window = rocket.mask.GetComponent<RectTransform>();
        var rt = button.GetComponent<RectTransform>();

        if (value.Digraphs == currentQuestion.currentCorrect.Digraphs)
        {
            eventSystem.enabled = false;
            PlayWord(value);
            if (finger != null)
                finger.gameObject.SetActive(false);

            for (int i = 0; i < buttons.Length; i++)
                buttons[i].button.interactable = false;

            DoMove(window, rt, () =>
            {
                button.gameObject.SetActive(false);
                rocket.Away(value.sprite, () =>
                {
                    AddAnswer(value);

                    if (!CheckOver())
                        CallRokect();
                    isRoutine = true;
                    isStop = false;
                    eventSystem.enabled = true;
                });
            });
        }
        else
            audioPlayer.PlayIncorrect(button.data.audio.phanics);
    }
    private void DoMove(RectTransform window, RectTransform rt, TweenCallback callback)
    {
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

        seq.onComplete += callback;
        seq.Play();
    }

    private void PlayCurrentWord(Action action = null)
    {
        PlayWord(currentQuestion.currentCorrect, action);
    }
    private void PlayWord(DigraphsWordsData word, Action action = null) => audioPlayer.Play(word.clip, action);
    private void CallRokect()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].isOn = false;
        if (finger != null)
            finger.gameObject.SetActive(false);

        rocket.Call(() =>
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].button.interactable = true;
            PlayCurrentWord(() => isStop = true);
            if (finger != null)
                finger.gameObject.SetActive(true);
        });
    }

    protected override void EndGuidnce()
    {
        rocket.ResetPosition();

        base.EndGuidnce();
    }
}

public class Question5_104 : MultiQuestion<DigraphsWordsData>
{
    public int currentIndex { get; private set; } = 0;
    public DigraphsWordsData currentCorrect => correct[currentIndex];

    public Question5_104(DigraphsWordsData[] correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(DigraphsWordsData answer) => currentCorrect == answer;
    public override void SetAnswer(DigraphsWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}
