using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_120 : MultiAnswerContents<Question120,AlphabetWordsData>
{
    public EventSystem eventSystem;
    public Button buttonRocket;
    public ImageRocket roket;
    public Card120[] cards;
    public GameObject finger;
    public Text selectText;

    private int correctCount => 2;
    private int sameAlphabetCount => 2;
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_120;


    
    bool isRocketed = false;
    int guideIndex = 0;
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        //for (int i = 0; i < QuestionCount; i++)
        //{
        var targets = cards.Where(x => x.data.alphabet == currentQuestion.correct[currentQuestionIndex].alphabet).ToArray();
        foreach (var item in targets)
        {
            while (!isRocketed) yield return null;
            isRocketed = false;

            guideFinger.DoMove(item.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    item.card.Turnning(1f, () =>
                    {
                        CardClickMotion(item);
                        currentQuestionIndex++;
                    });
                });
            });

        }

        

        //}
    }

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
        card.onClick += (data) =>
        {
            selectText.text = data.key;
            eventSystem.enabled = false;
            if (currentQuestion.correct.Select(x=>x.key).Contains(data.key))
            {
                CardClickMotion(card);
            }
            else
            {
                audioPlayer.Play(data.act, () =>
                {
                    card.card.Turnning(onCompleted: () => eventSystem.enabled = true);
                    selectText.text = string.Empty;
                });
            }
        };
    }

    private void CardClickMotion(Card120 card)
    {
        selectText.text = card.data.key;
        eventSystem.enabled = false;

        if (finger != null)
            finger.gameObject.SetActive(false);
        var sprite = card.imageButton.image.sprite;
        roket.valueUI.sprite = sprite;
        if(isGuide)
            guideIndex++;
        audioPlayer.Play(card.data.act, () =>
        {
            roket.Away(sprite, () =>
            {
                AddAnswer(card.data);
                if (!CheckOver())
                    if(guideIndex == correctCount)
                    {
                        guideIndex = 0;
                        isGuide = false;
                        guideFinger.gameObject.SetActive(false);
                        questions = MakeQuestion();
                        currentQuestionIndex = 0;
                        ShowQuestion(questions[currentQuestionIndex]);
                    }
                    else
                        CallRocket();
                else
                    eventSystem.enabled = true;
                selectText.text = string.Empty;
            });
        });
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
            isRocketed = true;
        });
        roket.mask.gameObject.SetActive(true);
        roket.valueUI.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.White, eAlphabetType.Lower, currentQuestion.correct[currentQuestion.currentIndex].Key);
    }
    private void PlayAudio()
    {
        var alphabet = currentQuestion.correct[currentQuestion.currentIndex].Key;
        audioPlayer.Play(GameManager.Instance.GetResources((eAlphabet)alphabet).AudioData.phanics);
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
                var correct = GameManager.Instance.GetResources(alphabet).Words
                    .OrderBy(x => Random.Range(0f, 100f))
                    .Take(correctCount)
                    .ToArray();

                var incorrect = GameManager.Instance.alphabets
                    .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
                    .Where(x=>!correct.Select(y=>y.key).Contains(x.key))
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
        for(int i = 0;i < question.totalQuestion.Length; i++)
            cards[i].Init(question.totalQuestion[i]);
    }
}
public class Question120 : MultiQuestion<AlphabetWordsData>
{
    public int currentIndex { get; private set; }
    public Question120(AlphabetWordsData[] correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
        currentIndex = 0;
    }
    public override void SetAnswer(AlphabetWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }

    protected override bool CheckCorrect(AlphabetWordsData answer) => correct.Contains(answer);
}
