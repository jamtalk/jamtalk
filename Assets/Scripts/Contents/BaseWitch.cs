using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWitch<T> : SingleAnswerContents<Question_Witch<T>, T>
    where T : DataSource
{
    protected override int QuestionCount => 6;
    protected override eContents contents => eContents.JT_PL2_108;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;

    protected T[] words;

    [SerializeField]
    protected PotionElement<T>[] elements;

    public GameObject prefabPotionElement;
    public PotElement prefabPot;
    public GameObject prefabMark;
    public MagicWand<T> magicWand;
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
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].onDrag += OnDrag;
            elements[i].onDrop += OnDrop;
        }
    }

    protected override void ShowQuestion(Question_Witch<T> question)
    {
        Debug.Log(question.totalQuestion.Length);
        Speak();
        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            elements[i].Init(question.totalQuestion[i]);
            elements[i].image.gameObject.SetActive(true);
            elements[i].textValue.gameObject.SetActive(true);
        }
    }

    protected virtual void OnDrop(PotionElement<T> target)
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
    protected virtual void OnDrag(PotionElement<T> target)
    {
        target.image.gameObject.SetActive(false);
        target.textValue.gameObject.SetActive(false);
        audioPlayer.Play(potionSound);
        magicWand.SetDrag(target);
    }

    protected virtual void Speak()
    {
        ani.SetBool("Speak", true);
        //audioPlayer.Play(currentQuestion.correct.clip);
        //currentQuestion.correct.PlayClip();
    }

    protected override void AddAnswer(T answer)
    {
        base.AddAnswer(answer);
        for (int i = 0; i < elements.Length; i++)
            elements[i].ResetPosition();
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
        audioPlayer.Play(1f, effectSound, () => AddAnswer(currentQuestion.correct));

        yield return new WaitForSecondsRealtime(1);
        successEffect.SetActive(false);
    }
}



public class Question_Witch<T> : SingleQuestion<T>
    where T : DataSource
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
    public Question_Witch(T correct, T[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}
