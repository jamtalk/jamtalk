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

    private DigraphsWordsData[] current;
    [SerializeField]
    private EventSystem eventSystem;
    private float colorFillamount = 0f;
    private AnimationCharactor doughCharactor => animationCharactors.First();


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();


        guideFinger.DoMove(buttonCharactor.transform.position, () => isNext = true);
        for (float j = colorFillamount; j < .9f; j += .3f)
        {
            while (!isNext) yield return null;
            isNext = false;
            guideFinger.DoClick(() => OnClickCharactor());
        }

        while (!isNext) yield return null;
        isNext = false;

        var target = buttonQuestions.Where(x => x.name == questions[currentQuestionIndex].correct.key).First();

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                guideFinger.gameObject.SetActive(false);
                CorrectClickMotion(questions[currentQuestionIndex].correct);
            });
        });

        while (!isNext) yield return null;
        isNext = false;


    }

    protected override void Awake()
    {
        base.Awake();

        buttonCharactor.onClick.AddListener(() => OnClickCharactor());
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
            {   
                CorrectClickMotion(data);
            }
            else
            {
                audioPlayer.PlayIncorrect();
                // 우측 캐릭터 실망하는 애니매이팅 
            }
        });
    }

    private void CorrectClickMotion(DigraphsWordsData data)
    {
        // 우측 캐릭터 기뻐하는 애니매이팅

        audioPlayer.Play(data.clip, () =>
        {
            colorFillamount = 0f;
            var color = Color.black;
            color.a = colorFillamount;
            currentText.color = color;

            AddAnswer(data);

            isNext = true;
        });
    }

    private void OnClickCharactor(TweenCallback callback = null)
    {
        eventSystem.enabled = false;

        doughCharactor.DetailChange(eCharactorDetail.edgege_dough_action, false);

        StartCoroutine(AnimationCompleted());
    }
    private IEnumerator AnimationCompleted()
    {
        if(!doughCharactor.isCompleted)
            yield return null;

        colorFillamount += 0.33f;
        var color = Color.black;
        color.a = colorFillamount;
        currentText.color = color;

        if (colorFillamount >= 0.9f)
        {
            for (int i = 0; i < buttonQuestions.Length; i++)
                buttonQuestions[i].interactable = true;
        }
        isNext = true;
        eventSystem.enabled = true;
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