using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_119 : SingleAnswerContents<Question119, WordsData.WordSources>
{
    public EventSystem eventSystem;
    public ButtonExitnction[] buttons;
    public Image imageAlphabetUpper;
    public Image imageAlphabetLower;
    public Button[] buttonSound;
    public GameObject finger;
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

    protected override eContents contents => eContents.JT_PL1_119;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < buttonSound.Length; i++)
            buttonSound[i].onClick.AddListener(() =>
            {
                audioPlayer.Play(GameManager.Instance.GetResources().AudioData.phanics);
                if(finger!=null)
                {
                    Destroy(finger);
                    finger = null;
                }
            });
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.phanics, () =>
        {
            if (finger != null)
                finger.SetActive(true);
        });
    }
    private void AddButtonListener(ButtonExitnction button, WordsData.WordSources data)
    {
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
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
                }
                else
                {
                    if (finger != null)
                        finger.gameObject.SetActive(true);
                }
            });
        });
    }

    protected override List<Question119> MakeQuestion()
    {
        var correct = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();
        Debug.LogFormat("{0}=>{1}", correct.Length, QuestionCount);
        var list = new List<Question119>();
        for (int i = 0;i < QuestionCount; i++)
        {
            var incorrect = GameManager.Instance.GetResources().Words
                .Where(x => !correct.Select(y=>y.value).Contains(x.value))
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
        imageAlphabetUpper.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Yellow, eAlphabetType.Upper, question.correct.alphabet);
        imageAlphabetLower.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Yellow, eAlphabetType.Lower, question.correct.alphabet);
        var questions = question.totalQuestion;
        for(int i = 0;i < buttons.Length; i++)
        {
            buttons[i].Init(questions[i].sprite);
            AddButtonListener(buttons[i], questions[i]);
        }
    }
}
public class Question119 : SingleQuestion<WordsData.WordSources>
{
    public Question119(WordsData.WordSources correct, WordsData.WordSources[] questions) : base(correct, questions)
    {
    }
}
