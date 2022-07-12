using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class JT_PL4_107 : SingleAnswerContents<Question4_107, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL4_107;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 3;

    public Text currentText;
    public Button buttonCharactor;
    public Button[] buttonQuestions;

    [SerializeField]
    private GameObject[] charactor;
    private DigraphsWordsData[] current;
    [SerializeField]
    private EventSystem eventSystem;
    private float colorFillamount = 0f;

    protected override void Awake()
    {
        base.Awake();

        buttonCharactor.onClick.AddListener(() => CharactorAddListener());
    }
    protected override List<Question4_107> MakeQuestion()
    {
        var questions = new List<Question4_107>();

        current = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        
        for (int i = 0; i < QuestionCount; i++)
        {
            var temp = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(buttonQuestions.Length - 1)
                .ToArray();
            
            questions.Add(new Question4_107(current[i], temp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question4_107 question)
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            var data = question.totalQuestion[i];
            buttonQuestions[i].name = question.totalQuestion[i].key;
            buttonQuestions[i].image.sprite = question.totalQuestion[i].sprite;
            buttonQuestions[i].image.preserveAspect = true;
            buttonQuestions[i].interactable = false;

            AddListener(buttonQuestions[i], data);
        }

        currentText.text = current[currentQuestionIndex].key;
    }

    private void AddListener(Button button, DigraphsWordsData data)
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            if (currentQuestion.correct == data)
            {   // 우측 캐릭터 기뻐하는 애니매이팅
                Debug.Log(button.name);
                audioPlayer.Play(data.clip, () =>
                {
                    colorFillamount = 0f;
                    var color = Color.black;
                    color.a = colorFillamount;
                    currentText.color = color;

                    AddAnswer(data);
                });
            }
            else
            {
                // 우측 캐릭터 실망하는 애니매이팅 
            }
        });
    }

    private void CharactorAddListener()
    {
        DoMove(() =>
        {
            colorFillamount += 0.33f;
            var color = Color.black;
            color.a = colorFillamount;
            currentText.color = color;

            if (colorFillamount >= 0.9f)
            {
                for (int i = 0; i < buttonQuestions.Length; i++)
                    buttonQuestions[i].interactable = true;
            }
            eventSystem.enabled = true;
        });
    }

    private void DoMove(TweenCallback callback)
    {
        eventSystem.enabled = false;
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < charactor.Length; i++)
        {
            var duration = 1f;
            var transform = charactor[i].transform.position.y;
            Tween startTween = charactor[i].transform.DOMoveY(transform - 50, duration);
            Tween lastTween = charactor[i].transform.DOMoveY(transform, duration);
            startTween.SetEase(Ease.Linear);
            lastTween.SetEase(Ease.Linear);
            seq.Insert(0, startTween);
            seq.Insert(duration, lastTween);
        }

        seq.onComplete += callback;
        seq.Play();
    }
}

public class Question4_107 : SingleQuestion<DigraphsWordsData>
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
    public Question4_107(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}