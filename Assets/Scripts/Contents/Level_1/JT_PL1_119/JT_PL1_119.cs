using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_119 : SingleAnswerContents<Question119, AlphabetWordsData>
{
    public EventSystem eventSystem;
    public ButtonExitnction[] buttons;
    public Image imageAlphabetUpper;
    public Image imageAlphabetLower;
    public Button[] buttonSound;
    public GameObject finger;
    private AlphabetWordsData data;
    protected override int QuestionCount
    {
        get
        {
            var value = 5;
            var words = GameManager.Instance.GetResources().Words;
            if (words.Length < value)
                value = words.Length;
            return value;
        }
    }

    bool isNext = false;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for(int i = 0; i < QuestionCount; i++)
        {
            var target = buttons.Where(x => x.name == questions[i].correct.key).First();

            guideFinger.gameObject.SetActive(true);

            guideFinger.DoMoveCorrect(target.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    ButtonClickMotion(target, questions[i].correct);
                    guideFinger.gameObject.SetActive(false);
                });
            });

            while (!isNext) yield return null;
            isNext = false;
        }
    }

    protected override eContents contents => eContents.JT_PL1_119;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < buttonSound.Length; i++)
            buttonSound[i].onClick.AddListener(() =>
            {
                audioPlayer.Play(currentQuestion.correct.audio.phanics);
                if (finger!=null)
                {
                    Destroy(finger);
                    finger = null;
                }
            });
        audioPlayer.Play(currentQuestion.correct.audio.phanics, () =>
        {
            if (finger != null)
                finger.SetActive(true);
        });
    }

    private void ButtonClickMotion(ButtonExitnction button, AlphabetWordsData data)
    {
        eventSystem.enabled = false;
        if (finger != null)
            finger.gameObject.SetActive(false);
        if (currentQuestion.correct == data)
            button.Exitnction();
        else
            button.Incorrect();
        audioPlayer.Play(data.clip, () =>
        {
            eventSystem.enabled = true;
            if (currentQuestion.correct == data)
            {
                button.Exitnction();
                AddAnswer(data);
                isNext = true;
            }
            else
            {
                if (finger != null)
                    finger.gameObject.SetActive(true);
            }
        });
    }

    private void AddButtonListener(ButtonExitnction button, AlphabetWordsData data)
    {
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            ButtonClickMotion(button, data);
        });
    }

    protected override List<Question119> MakeQuestion()
    {
        var correct = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 }
            .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();
        var list = new List<Question119>();
        for (int i = 0;i < QuestionCount; i++)
        {
            var words = GameManager.Instance.GetResources(correct[i].Key).Words.Select(x=>x.key);
            var incorrect = GameManager.Instance.alphabets
                .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
                .Where(x => x.Key != GameManager.Instance.currentAlphabet)
                .Where(x=>!words.Contains(x.key))
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(buttons.Length - 1)
                .ToArray();
            list.Add(new Question119(correct[i], incorrect));
        }
        return list;
    }

    protected override void ShowQuestion(Question119 question)
    {
        if (finger != null)
            finger.SetActive(true);
        imageAlphabetUpper.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Yellow, eAlphabetType.Upper, question.correct.Key);
        imageAlphabetLower.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Yellow, eAlphabetType.Lower, question.correct.Key);
        var questions = question.totalQuestion;
        for(int i = 0;i < buttons.Length; i++)
        {
            buttons[i].Init(questions[i].sprite);
            buttons[i].name = questions[i].key;
            AddButtonListener(buttons[i], questions[i]);
        }
    }
}
public class Question119 : SingleQuestion<AlphabetWordsData>
{
    public Question119(AlphabetWordsData correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
    }
}
