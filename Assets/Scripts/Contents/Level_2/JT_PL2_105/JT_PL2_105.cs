using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using Random = UnityEngine.Random;

public class JT_PL2_105 : SingleAnswerContents<Question2_105, VowelWordsData>
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL2_105;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 6;

    [SerializeField]
    private List<BubbleElement> elements = new List<BubbleElement>();

    public GameObject astronaut;
    public Sprite imageBigbang;
    public Sprite[] imageShootingStars;
    public Thrower204 thrower;
    public RectTransform thorwerTarget;

    public AudioClip tabClip;
    public AudioClip boomClip;
    public AudioClip dropClip;
    public AudioClip errorClip;

    private Vector3 defaultPosition;
    private BubbleElement currentElement;
    private List<Tween> tweens = new List<Tween>();

    bool isNext = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for (int i = 0; i < QuestionCount; i++)
        {
            isNext = false;
            var target = elements.Where(x => x.data == currentQuestion.correct).First();
            guideFinger.gameObject.SetActive(true);

            guideFinger.DoMove(target.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    ClickMotion(target, target.data);
                });
            });

            while(!isNext) yield return null;
            yield return new WaitForSecondsRealtime(1.5f);
        }
    }

    protected override void Awake()
    {   
        base.Awake();

        var charButton = astronaut.GetComponent<Button>();
        charButton.onClick.AddListener(() => Speak());

        Speak();
    }

    protected override List<Question2_105> MakeQuestion()
    {
        var questions = new List<Question2_105>();

        var longVowel = GameManager.Instance.vowels
            .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
            .Where(x => x.VowelType == eVowelType.Long)
            .Where(x => x.Vowel == GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount / 2)
            .ToArray();

        var shortVowel = GameManager.Instance.vowels
            .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
            .Where(x => x.VowelType == eVowelType.Short)
            .Where(x => x.Vowel == GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount / 2)
            .ToArray();

        VowelWordsData[] vowels = longVowel.Concat(shortVowel).ToArray();
        vowels = vowels.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < QuestionCount; i++)
        {
            var vowelType = vowels[i].VowelType == eVowelType.Long ? eVowelType.Short : eVowelType.Long;
            var tmp = GameManager.Instance.vowels
                .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
                .Where(x => x.VowelType == vowelType)
                .Where(x => x.Vowel == GameManager.Instance.currentAlphabet)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(elements.Count - 1)
                .ToArray();
            questions.Add(new Question2_105(vowels[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question2_105 question)
    {
        Speak();
        if (tweens.Count > 0)
        {
            foreach (var item in tweens)
                item.Kill();
            tweens.Clear();
        }

        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            var data = question.totalQuestion[i];
            elements[i].transform.localScale = new Vector3(1f, 1f, 1f);
            elements[i].textValue.gameObject.SetActive(true);

            elements[i].Init(question.totalQuestion[i]);
            elements[i].isOn = false;
            elements[i].name = i.ToString();

            elements[i].transform.localPosition = Vector2.zero;
            var tween = elements[i].transform.DOLocalMoveY(30, 1).SetLoops(-1, LoopType.Yoyo);
            tween.SetDelay(0.3f * i);
            tweens.Add(tween);
            AddClickListener(elements[i], data);
        }
    }

    private void ClickMotion(BubbleElement planet, VowelWordsData data)
    {
        currentElement = planet;
        planet.transform.DOShakePosition(1f, 5f);
        StartCoroutine(InitPlanet(data));
    }
    private void AddClickListener(BubbleElement planet, VowelWordsData data)
    {
        planet.onClickFirst.RemoveAllListeners();

        planet.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(tabClip);
            if (currentQuestion.correct == data)
            {
                ClickMotion(planet, data);
            }
            else
            {
                audioPlayer.Play(errorClip);
                planet.transform.DOShakePosition(1f, 5f);
                planet.isOn = false;
            }
        });

    }

    private IEnumerator InitPlanet(VowelWordsData data)
    {
        if(isGuide)
            audioPlayer.Play(tabClip);
        yield return new WaitForSecondsRealtime(1f);

        audioPlayer.Play(boomClip);
        currentElement.textValue.gameObject.SetActive(false);
        currentElement.sprite = imageBigbang;
        currentElement.OutIn(() => eventSystem.enabled = true, 0f, 1f);

        yield return new WaitForSecondsRealtime(1.5f);
        thrower.textValue.gameObject.SetActive(false);
        thrower.imageProduct.sprite = imageShootingStars[int.Parse(currentElement.name)];
        thrower.Throw(currentElement, thorwerTarget, () =>
        {
            isNext = true;
            AddAnswer(data);
        });

        audioPlayer.Play(dropClip);
    }
        
    private void Speak()
    {
        var ani = astronaut.GetComponent<Animator>();
        ani.SetBool("Speak", true);
        audioPlayer.Play(currentQuestion.correct.clip);
    }
}



public class Question2_105 : SingleQuestion<VowelWordsData>
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
    public Question2_105(VowelWordsData correct, VowelWordsData[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}


