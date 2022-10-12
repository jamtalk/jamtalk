using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWitch<T> : SingleAnswerContents<Question_Witch<T>, T>
    where T : ResourceWordsElement
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
    public WitchResult result;
    public AudioClip potSound;
    public AudioClip potionSound;
    public AudioClip effectSound;

    [SerializeField]
    private Animator ani;

    bool isNext = false;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for(int i = 0; i < QuestionCount; i++)
        {
            while (!isNext) yield return null;
            isNext = false;
            Debug.Log("I : " + i);
            var target = elements.Where(x => x.data == currentQuestion.correct).First();

            guideFinger.DoMoveCorrect(target.transform.position, () =>
            {
                guideFinger.DoPress(() =>
                {
                    guideFinger.DoMoveCorrect(target.gameObject, prefabPot.transform.position, () =>
                    {
                        guideFinger.gameObject.SetActive(false);
                        target.gameObject.SetActive(false);
                        OnDrop(target);
                        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
                    });
                });
            });

        }
    }

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
        result.gameObject.SetActive(false);
        Debug.Log(question.totalQuestion.Length);
        Speak();
        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            elements[i].gameObject.SetActive(true);
            elements[i].Init(question.totalQuestion[i]);
            elements[i].image.gameObject.SetActive(true);
            elements[i].textValue.gameObject.SetActive(true);
        }

        isNext = true;
    }

    private void DropMotion(PotionElement<T> target)
    {
        magicWand.gameObject.SetActive(false);
        result.ShowResult(target.data.sprite, .5f);
        audioPlayer.Play(1.5f, effectSound, () =>
        {
            AddAnswer(target.data);
        });
    }

    protected virtual void OnDrop(PotionElement<T> target)
    {
        if (currentQuestion.correct == target.data)
        {
            DropMotion(target);
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
}



public class Question_Witch<T> : SingleQuestion<T>
    where T : ResourceWordsElement
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
