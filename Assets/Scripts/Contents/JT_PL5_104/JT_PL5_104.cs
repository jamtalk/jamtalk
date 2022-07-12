using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL5_104 : MultiAnswerContents<Question5_104, DigraphsWordsData>
{
    protected override int QuestionCount => 3;
    public GameObject finger;

    protected override eContents contents => eContents.JT_PL5_104;
    public TextRocket rocket;
    public DoubleClick504[] buttons;
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
        for (int i = 0; i < buttons.Length; i++)
        {
            AddOnClickTextButtonListener(buttons[i]);
        }
    }
    protected override List<Question5_104> MakeQuestion()
    {
        int correctCount = 1;
        int incorrectCount = buttons.Length - correctCount;
        var list = new List<Question5_104>();
        for (int i = 0; i < QuestionCount; i++)
        {
            var corrects = GameManager.Instance.digrpahs
               .SelectMany(x => GameManager.Instance.GetDigraphs(x))
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
        return eGameResult.Perfect;
    }
    protected override void ShowQuestion(Question5_104 question)
    {
        var randomQuestions = question.totalQuestion;
        for (int i = 0; i < randomQuestions.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].Init(randomQuestions[i]);
            buttons[i].name = randomQuestions[i].key;
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
        var window = rocket.mask.GetComponent<RectTransform>();
        var rt = button.GetComponent<RectTransform>();

        button.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(button.data.act);
        });
        button.onClickData += (value) =>
        {
            if (value.Digraphs == currentQuestion.currentCorrect.Digraphs)
            {
                PlayWord(value);
                if (finger != null)
                    finger.gameObject.SetActive(false);

                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].button.interactable = false;

                DoMove(window, rt, () =>
                {
                    button.gameObject.SetActive(false);
                    rocket.Away(value.key, () =>
                    {
                        AddAnswer(value);

                        if (!CheckOver()) 
                            CallRokect();
                    });
                });
            }
        };
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

    private void PlayCurrentWord()
    {
        PlayWord(currentQuestion.currentCorrect);
    }
    private void PlayWord(DigraphsWordsData word) => audioPlayer.Play(word.clip);
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
            PlayCurrentWord();
            if (finger != null)
                finger.gameObject.SetActive(true);
        });
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
