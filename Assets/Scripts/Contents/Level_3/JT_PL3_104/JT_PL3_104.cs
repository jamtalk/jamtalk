using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using Random = UnityEngine.Random;

public class JT_PL3_104 : SingleAnswerContents<AlphabetContentsSetting, Question3_104, DigraphsWordsData>
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL3_104;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 3;

    private float smallBubbleSize = 0.7f;
    private DigraphsWordsData[] currents;
    protected List<BubbleElement> bubbles = new List<BubbleElement>();
    private List<RectTransform> bubbleParents = new List<RectTransform>();

    [Header("UI")]
    public RectTransform bubbleParent;
    public Thrower204 thrower;
    public GameObject bubbleElement;
    public Text textPot;
    public Image effectImage;
    public FingerAnimation point;
    public Text textCurrent;

    [Header("List")]
    public Button[] charactors;
    [SerializeField]
    private RectTransform[] smallBubbles;
    [SerializeField]
    private List<BubbleElement> elements = new List<BubbleElement>();
    [SerializeField]
    private Animator[] ani;

    [Header("Audio")]
    public AudioSinglePlayer putSource;
    public AudioClip effectClip;
    public AudioClip tabClip;
    public AudioClip putClip;
    public AudioClip errorClip;

    
    bool isSmall = false;
    private string digraphs = string.Empty;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        
        var target = elements.Where(x => x.digraphs == currentQuestion.correct).First();

        guideFinger.gameObject.SetActive(true);

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                FirstClickMotion(target);

                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    SecondsClickMotion(target, target.digraphs);
                });
            });

        });

        while (!isSmall) yield return null;
        isSmall = false;

        isNext = false;

        var smallTarget = bubbles.Where(x => x.textValue.text == digraphs).First();

        guideFinger.gameObject.SetActive(true);
        guideFinger.DoMove(smallTarget.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                guideFinger.gameObject.SetActive(false);
                SmallBubbleClickMotion(target, smallTarget, smallTarget.digraphs, () =>
                {
                    PotInMotion(target.digraphs);
                });
            });
        });

        while (!isNext) yield return null;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        textPot.text = GameManager.Instance.currentDigrpahs.ToString().ToLower();
        SetButtonAddListener(charactors);
    }
    private void SetButtonAddListener(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].onClick.AddListener(() => Speak());
    }

    protected override List<Question3_104> MakeQuestion()
    {
        var questions = new List<Question3_104>();
        currents = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();

        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(elements.Count - 1)
                .ToArray();
            
            questions.Add(new Question3_104(currents[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question3_104 question)
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

    protected virtual void AddDoubleClickListener(BubbleElement bubble, DigraphsWordsData data)
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
    private void SecondsClickMotion(BubbleElement bubble, DigraphsWordsData data)
    {
        point.gameObject.SetActive(false);

        audioPlayer.Play(1f, tabClip);
        bubble.gameObject.SetActive(false);
        Vector3 vector3 = new Vector3(smallBubbleSize, smallBubbleSize, smallBubbleSize);

        var current = currents[currentQuestionIndex];
        digraphs = string.Empty;
        if (current.key.IndexOf(current.digraphs.ToLower()) < 0)
            digraphs = current.PairDigrpahs.ToString().ToLower();
        else
            digraphs = current.Digraphs.ToString().ToLower();

        var temp = bubble.textValue.text.Replace(digraphs, string.Empty);

        var tempList = new List<string>();
        foreach (var item in temp)
            tempList.Add(item.ToString());
        tempList.Add(digraphs);

        var digraphsIndex = currentQuestion.correct.key.IndexOf(digraphs);
        var values = new List<string>();
        foreach (var item in temp)
            values.Add(item.ToString());
        values.Insert(digraphsIndex, digraphs);

        for (int i = 0; i < tempList.Count; i++)
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
                if (digraphs == smallBubbles.textValue.text)
                    SmallBubbleClickMotion(bubble, smallBubbles, data);
                else
                    BubblesPlay(bubbles, smallBubbleSize);
            });
        }
        StartCoroutine(Init(() => isSmall = true));
    }

    private void SmallBubbleClickMotion(BubbleElement bubble, BubbleElement smallBubbles, DigraphsWordsData data, Action action = null)
    {
        eventSystem.enabled = false;
        ThrowElement(smallBubbles, data, action);
        smallBubbles.isOn = false;
        audioPlayer.Play(1f, putClip);

        var targets = new List<GameObject>();
        for (int i = 1; i < bubbles.Count + 1; i++)
            targets.Add(bubbleParent.parent.GetChild(i).gameObject);

        for (int i = 0; i < targets.Count; i++)
        {
            var destory = targets[i];
            Destroy(destory);
        }
        bubbles.Clear();
        bubbleParents.Clear();

        bubble.image.gameObject.SetActive(true);
        bubble.textValue.gameObject.SetActive(true);
        bubbleParent.gameObject.SetActive(true);
    }
    protected virtual void ThrowElement(BubbleElement bubble, DigraphsWordsData data, Action action = null)
    {
        if (isGuide)
        {
            thrower.Throw(bubble, textPot.GetComponent<RectTransform>(), action);
        }
        else
        {
            thrower.Throw(bubble, textPot.GetComponent<RectTransform>(), () =>
            {
                PotInMotion(data);
            });
        }
    }

    public void PotInMotion(DigraphsWordsData data)
    {
        textCurrent.text = data.key;
        effectImage.sprite = data.sprite;
        effectImage.gameObject.SetActive(true);
        effectImage.preserveAspect = true;
        putSource.Play(1f, effectClip, () =>
        {
            effectImage.gameObject.SetActive(false);
            AddAnswer(data);
            textCurrent.text = string.Empty;
            isNext = true;
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
            seq.Insert(0f,bubbles[i].transform.DOLocalMove(Vector2.zero, 1f));
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
            bubbles[i].InOut(() => eventSystem.enabled = true, 0.5f, max);
    }

    protected virtual void Speak()
    {
        foreach (var item in ani)
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

public class Question3_104 : SingleQuestion<DigraphsWordsData>
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
    public Question3_104(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}