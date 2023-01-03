using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using Random = UnityEngine.Random;

public class JT_PL2_104 : SingleAnswerContents<Question2_104, VowelWordsData>
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL2_104;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 6;

    private float smallBubbleSize = 0.7f;
    protected List<BubbleElement> bubbles = new List<BubbleElement>();
    private List<RectTransform> bubbleParents = new List<RectTransform>();

    [Header("UI")]
    public RectTransform bubbleParent;
    public Thrower204 thrower;
    public GameObject bubbleElement;
    public Text textPot;
    public FingerAnimation point;

    [Header("List")]
    public Button[] charactorButtons;
    [SerializeField]
    private RectTransform[] smallBubbles;
    [SerializeField]
    private List<BubbleElement> elements = new List<BubbleElement>();
    [SerializeField]
    private Animator[] ani;

    [Header("Audio")]
    public AudioClip tabClip;
    public AudioClip putClip;
    public AudioClip errorClip;

    bool isSmall = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        var target = elements.Where(x => x.data == currentQuestion.correct).First();

        guideFinger.gameObject.SetActive(true);

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                FirstClickMotion(target);

                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    SecondsClickMotion(target, target.data);
                });
            });

        });

        while (!isSmall) yield return null;
        isSmall = false;
        isNext = false;


        var smallTarget = bubbles.Where(x => x.textValue.text == GameManager.Instance.currentAlphabet.ToString().ToLower()).First();

        guideFinger.gameObject.SetActive(true);
        guideFinger.DoMove(smallTarget.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                guideFinger.gameObject.SetActive(false);
                SmallBubbleClickMotion(target, smallTarget, smallTarget.data, () =>
                {
                    isNext = true;
                });
            });
        });

        while (!isNext) yield return null;
    }


    protected override void Awake()
    {
        base.Awake();

        textPot.text = GameManager.Instance.currentAlphabet.ToString().ToLower();
        SetButtonAddListener(charactorButtons);
    }
    private void SetButtonAddListener(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.AddListener(() => Speak());
    }

    protected override List<Question2_104> MakeQuestion()
    {
        var questions = new List<Question2_104>();

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

        for ( int i = 0; i < QuestionCount; i++)
        {
            var vowelType = vowels[i].VowelType == eVowelType.Long ? eVowelType.Short : eVowelType.Long; 
            var tmp = GameManager.Instance.vowels
                .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
                .Where(x => x.VowelType == vowelType)
                .Where(x => x.Vowel == GameManager.Instance.currentAlphabet)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(elements.Count - 1)
                .ToArray();
            questions.Add(new Question2_104(vowels[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question2_104 question)
    {
        Speak();
        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            var data = question.totalQuestion[i];
            elements[i].Init(question.totalQuestion[i]);
            elements[i].isOn = false;
            AddDoubleClickListener(elements[i], data);
        }

        eventSystem.enabled = true;
    }

    protected virtual void AddDoubleClickListener(BubbleElement bubble, VowelWordsData data)
    {
        bubble.onClickFirst.RemoveAllListeners();
        bubble.onClick.RemoveAllListeners();

        bubble.onClickFirst.AddListener(() =>
        {
            if (currentQuestion.correct == data)
            {
                FirstClickMotion(bubble);
            }
            else
            {
                bubble.isOn = false;
                BubblesPlay(elements, 1f);
            }
        });

        bubble.onClick.AddListener(() =>
        {
            SecondsClickMotion(bubble, data);
        });
    }

    private void FirstClickMotion(BubbleElement bubble)
    {
        audioPlayer.Play(1f, tabClip);
        for (int i = 0; i < elements.Count; i++)
            elements[i].gameObject.SetActive(false);
        bubble.gameObject.SetActive(true);

        point.gameObject.SetActive(true);
        point.gameObject.transform.position = bubble.transform.position;
    }

    private void SecondsClickMotion(BubbleElement bubble, VowelWordsData data)
    {
        point.gameObject.SetActive(false);

        audioPlayer.Play(1f, tabClip);
        bubble.gameObject.SetActive(false);
        Vector3 vector3 = new Vector3(smallBubbleSize, smallBubbleSize, smallBubbleSize);

        var values = new List<string>();
        foreach (var item in bubble.textValue.text)
            values.Add(item.ToString());

        for (int i = 0; i < values.Count; i++)
        {
            var smallBubbles = Instantiate(bubbleElement, bubble.transform.parent).GetComponent<BubbleElement>();
            smallBubbles.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            smallBubbles.Init(values[i]);
            smallBubbles.transform.localScale = vector3;
            bubbles.Add(smallBubbles);

            var bubbleUI = Instantiate(bubbleParent, bubbleParent.transform.parent).GetComponent<RectTransform>();
            bubbleUI.name = smallBubbles.name;
            bubbleParents.Add(bubbleUI);

            smallBubbles.onClickFirst.AddListener(() =>
            {
                if (GameManager.Instance.currentAlphabet.ToString().ToLower() == smallBubbles.textValue.text)
                    SmallBubbleClickMotion(bubble, smallBubbles, data);
                else
                    BubblesPlay(bubbles, smallBubbleSize);
            });
        }
        StartCoroutine(Init(() => isSmall = true));
    }

    private void SmallBubbleClickMotion(BubbleElement bubble, BubbleElement smallBubbles, VowelWordsData data, Action action = null)
    {
        ThrowElement(smallBubbles, data, action);
        smallBubbles.isOn = false;

        var targets = new List<GameObject>();
        for (int i = 1; i < bubbles.Count + 1; i++)
            targets.Add(bubbleParent.parent.GetChild(i).gameObject);

        foreach (var item in targets)
            Destroy(item);

        bubbles.Clear();
        bubbleParents.Clear();

        bubble.image.gameObject.SetActive(true);
        bubble.textValue.gameObject.SetActive(true);
        bubbleParent.gameObject.SetActive(true);
    }

    protected virtual void ThrowElement(BubbleElement bubble, VowelWordsData data, Action action)
    {
        eventSystem.enabled = false;
        thrower.Throw(bubble, textPot.GetComponent<RectTransform>(), () =>
        {
            CorrectAction();
            audioPlayer.Play(1f, putClip, () =>
            {
                AddAnswer(data);
            });
        });
    }

    private IEnumerator Init(TweenCallback callback = null)
    {
        bubbleParent.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].gameObject.transform.SetParent(bubbleParents[i].transform);
            seq.Insert(0f, bubbles[i].transform.DOLocalMove(Vector2.zero, 1f));
        }

        seq.onComplete += callback;
        seq.Play();
    }

    private void BubblesPlay(List<BubbleElement> bubbles, float max)
    {
        //audioPlayer.Play(1f, errorClip);
        audioPlayer.PlayIncorrect();
        eventSystem.enabled = false;
        for (int i = 0; i < bubbles.Count; i++)
            bubbles[i].InOut(() => eventSystem.enabled = true, 0.5f ,max);
    }

    protected virtual void Speak()
    {
        foreach(var item in ani)
            item.SetBool("Speak", true);
        audioPlayer.Play(currentQuestion.correct.clip);
    }

    protected override void EndGuidnce()
    {
        if (bubbles.Count > 0)
        {
            foreach (var item in bubbles)
                Destroy(item.gameObject);
            bubbles.Clear();
        }
        base.EndGuidnce();
    }
}

public class Question2_104 : SingleQuestion<VowelWordsData>
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
    public Question2_104(VowelWordsData correct, VowelWordsData[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}