using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class JT_PL2_108 : SingleAnswerContents<Question2_108, WordsData.WordSources>
{
    protected override eContents contents => eContents.JT_PL2_108;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;

    protected override int QuestionCount => 6;

    protected WordsData.WordSources[] words;
    [SerializeField]
    protected PotionElement[] elements;

    public GameObject prefabPotionElement;
    public PotElement prefabPot;
    public GameObject prefabMark;
    public MagicWand magicWand;
    public GameObject successEffect;
    public Sprite successImage;
    public AudioClip potSound;
    public AudioClip potionSound;
    public AudioClip effectSound;

    [SerializeField]
    private Animator ani;

    protected override void Awake()
    {
        base.Awake();

        var button = prefabMark.GetComponent<Button>();
        button.onClick.AddListener(() => Speak());

        StartCoroutine(SetPosition());
        for(int i = 0;i< elements.Length; i++)
        {
            elements[i].onDrag += OnDrag;
            elements[i].onDrop += OnDrop;
        }
    }
    
    protected override List<Question2_108> MakeQuestion()
    {
        var questions = new List<Question2_108>();
        words = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(elements.Length-1)
                .ToArray();
            questions.Add(new Question2_108(words[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question2_108 question)
    {
        Speak();
        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            elements[i].Init(question.totalQuestion[i]);
            elements[i].image.gameObject.SetActive(true);
            elements[i].textValue.gameObject.SetActive(true);
        }
    }

    private void OnDrop(PotionElement target)
    {
        if (currentQuestion.correct == target.data)
        {
            magicWand.gameObject.SetActive(false);
            successEffect.GetComponent<Image>().sprite = target.data.sprite;
            successEffect.SetActive(true);
            StartCoroutine(SetEffectImage());
        }
        else
            target.ResetPosition();
    }
    private void OnDrag(PotionElement target)
    {
        target.image.gameObject.SetActive(false);
        target.textValue.gameObject.SetActive(false);
        audioPlayer.Play(potionSound);
        magicWand.SetDrag(target);
    }

    private void Speak()
    {
        ani.SetBool("Speak", true);
        audioPlayer.Play(currentQuestion.correct.clip);
    }

    private IEnumerator SetPosition()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < words.Length; i++)
            elements[i].SetDefaultPosition();
    }

    private IEnumerator SetEffectImage()
    {
        successEffect.GetComponent<Image>().sprite = successImage;
        successEffect.GetComponent<Image>().preserveAspect = true;
        yield return new WaitForSecondsRealtime(1);

        successEffect.SetActive(false);
        yield return new WaitForSecondsRealtime(0.25f);
        successEffect.GetComponent<Image>().sprite = currentQuestion.correct.sprite;

        successEffect.SetActive(true);
        audioPlayer.Play(1f,effectSound,()=> AddAnswer(currentQuestion.correct));

        yield return new WaitForSecondsRealtime(1);
        successEffect.SetActive(false);
    }
    protected override void AddAnswer(WordsData.WordSources answer)
    {
        base.AddAnswer(answer);
        for (int i = 0; i < elements.Length; i++)
            elements[i].ResetPosition();
    }
}


public class Question2_108 : SingleQuestion<WordsData.WordSources>
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
    public Question2_108(WordsData.WordSources correct, WordsData.WordSources[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}