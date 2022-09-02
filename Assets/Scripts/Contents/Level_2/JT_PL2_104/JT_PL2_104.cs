using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

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
    public Button[] charactors;
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

    protected override void Awake()
    {
        base.Awake();

        textPot.text = GameManager.Instance.currentAlphabet.ToString().ToLower();
        SetButtonAddListener(charactors);
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
    }

    protected virtual void AddDoubleClickListener(BubbleElement bubble, VowelWordsData data)
    {
        bubble.onClickFirst.RemoveAllListeners();
        bubble.onClick.RemoveAllListeners();

        bubble.onClickFirst.AddListener(() =>
        {
            if (currentQuestion.correct == data)
            {
                audioPlayer.Play(1f, tabClip);
                for (int i = 0; i < elements.Count; i++)
                    elements[i].gameObject.SetActive(false);
                bubble.gameObject.SetActive(true);

                point.gameObject.SetActive(true);
                point.gameObject.transform.position = bubble.transform.position;
            }
            else
            {
                audioPlayer.Play(1f, errorClip);
                bubble.isOn = false;
                BubblesPlay(elements, 1f);
            }
        });

        bubble.onClick.AddListener(() =>
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
                    audioPlayer.Play(1f, putClip);
                    smallBubbles.isOn = false;
                    if (GameManager.Instance.currentAlphabet.ToString().ToLower() == smallBubbles.textValue.text)
                    {
                        ThrowElement(smallBubbles, data);

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
                    else
                        BubblesPlay(bubbles, smallBubbleSize);
                });
            }
            StartCoroutine(Init());
        });
    }
    protected virtual void ThrowElement(BubbleElement bubble, VowelWordsData data)
    {
        thrower.Throw(bubble, textPot.GetComponent<RectTransform>(), () => AddAnswer(data));
    }

    private IEnumerator Init()
    {
        bubbleParent.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < bubbles.Count; i++)
        {
            bubbles[i].gameObject.transform.SetParent(bubbleParents[i].transform);
            bubbles[i].transform.DOLocalMove(Vector2.zero, 1f);
        }
    }

    private void BubblesPlay(List<BubbleElement> bubbles, float max)
    {
        audioPlayer.Play(1f, errorClip);
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