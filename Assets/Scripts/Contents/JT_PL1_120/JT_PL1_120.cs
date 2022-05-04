using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_120 : MultiAnswerContents<Question120,string>
{
    public EventSystem eventSystem;
    public Button buttonRocket;
    public ImageRocket roket;
    public Card120[] cards;
    public GameObject finger;

    private int correctCount => 2;
    private int sameAlphabetCount => 2;
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_120;
    protected override void Awake()
    {
        base.Awake();
        buttonRocket.onClick.AddListener(PlayAudio);
        buttonRocket.onClick.AddListener(() =>
        {
            if (finger != null)
            {
                Destroy(finger);
                finger = null;
            }
        });
        for (int i = 0; i < cards.Length; i++)
            AddCardOnClickListener(cards[i]);
    }
    private void AddCardOnClickListener(Card120 card)
    {
        card.onClick += () =>
        {
            var value = card.imageButton.image.sprite.name;
            eventSystem.enabled = false;
            if (currentQuestion.correct.Contains(value))
            {
                if (finger != null)
                    finger.gameObject.SetActive(false);
                var sprite = card.imageButton.image.sprite;
                roket.valueUI.sprite = sprite;

                var clip = GameManager.Instance.GetClipAct3(value);
                audioPlayer.Play(clip, () =>
                {
                    roket.Away(sprite, () =>
                    {
                        AddAnswer(value);
                        if (!CheckOver())
                            CallRocket();
                        else
                            eventSystem.enabled = true;
                    });
                });
            }
            else
            {
                var clip = GameManager.Instance.GetClipAct3(value);
                audioPlayer.Play(clip, () =>
                {
                    card.card.Turnning(onCompleted: () => eventSystem.enabled = true);
                });
            }
        };
    }
    private void CallRocket()
    {
        if (finger != null)
            finger.gameObject.SetActive(false);
        eventSystem.enabled = false;
        roket.Call(() =>
        {
            if (finger != null)
                finger.gameObject.SetActive(true);
            PlayAudio();
            eventSystem.enabled = true;
        });
        roket.mask.gameObject.SetActive(true);
        var currentValue = GameManager.Instance.ParsingAlphabet(currentQuestion.correct[currentQuestion.currentIndex]);
        roket.valueUI.sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.White, eAlphbetType.Lower, currentValue);
    }
    private void PlayAudio()
    {
        var alphabet = GameManager.Instance.ParsingAlphabet(currentQuestion.correct[currentQuestion.currentIndex]);
        audioPlayer.Play(GameManager.Instance.GetClipPhanics(alphabet));
    }
    protected override List<Question120> MakeQuestion()
    {
        var alphabets = GameManager.Instance.alphabets.
            Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(QuestionCount/ sameAlphabetCount)
            .ToArray();
        
        var list = new List<Question120>();
        for(int i = 0;i < alphabets.Length; i++)
        {
            var alphabet = alphabets[i];
            for(int j = 0; j < sameAlphabetCount; j++)
            {
                var correct = GameManager.Instance.GetWords(alphabet)
                    .OrderBy(x => Random.Range(0f, 100f))
                    .Take(correctCount)
                    .ToArray();

                var incorrect = GameManager.Instance.GetWords()
                    .Where(x=>!correct.Contains(x))
                    .OrderBy(x => Random.Range(0f, 100f))
                    .Take(cards.Length - correctCount)
                    .ToArray();

                list.Add(new Question120(correct, incorrect));
            }
        }
        return list;
    }

    protected override void ShowQuestion(Question120 question)
    {
        roket.Init();
        CallRocket();
        var questions = question.RandomQuestions;
        for(int i = 0;i < questions.Length; i++)
            cards[i].Init(GameManager.Instance.GetSpriteWord(questions[i]));
    }
}
public class Question120 : MultiQuestion<string>
{
    public int currentIndex { get; private set; }
    public Question120(string[] correct, string[] questions) : base(correct, questions)
    {
        currentIndex = 0;
    }
    public override void SetAnswer(string answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }

    protected override bool CheckCorrect(string answer) => correct.Contains(answer);
}
