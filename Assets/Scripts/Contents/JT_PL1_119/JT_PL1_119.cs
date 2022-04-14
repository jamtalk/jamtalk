using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_119 : SingleAnswerContents<Question119, string>
{
    public EventSystem eventSystem;
    public ButtonExitnction[] buttons;
    public Image imageAlphabetUpper;
    public Image imageAlphabetLower;
    public Button[] buttonSound;
    public GameObject finger;
    protected override int QuestionCount => 5;

    protected override eContents contents => eContents.JT_PL1_119;
    protected override void Awake()
    {
        base.Awake();
        for(int i = 0;i < buttons.Length; i++)
            AddButtonListener(buttons[i]);
        for(int i = 0;i < buttonSound.Length; i++)
            buttonSound[i].onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.GetClipPhanics(GameManager.Instance.currentAlphabet)));
        audioPlayer.Play(GameManager.Instance.GetClipPhanics(GameManager.Instance.currentAlphabet),()=> finger.SetActive(true));
    }
    private void AddButtonListener(ButtonExitnction button)
    {
        button.onClick += (value) =>
        {
            eventSystem.enabled = false;
            finger.gameObject.SetActive(false);
            if (currentQuestion.correct == value)
                button.Exitnction();
            else
                button.Incorrect();
            audioPlayer.Play(GameManager.Instance.GetClipWord(value), () =>
            {
                eventSystem.enabled = true;
                if (currentQuestion.correct == value)
                {
                    button.Exitnction();
                    AddAnswer(value);
                }
                else
                {
                    finger.gameObject.SetActive(true);
                }
            });
        };
    }

    protected override List<Question119> MakeQuestion()
    {
        var correct = GameManager.Instance.GetWords(GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();
        var list = new List<Question119>();
        for (int i = 0;i < QuestionCount; i++)
        {
            var incorrect = GameManager.Instance.GetWords()
                .Where(x => !correct.Contains(x))
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(buttons.Length - 1)
                .ToArray();
            list.Add(new Question119(correct[i], incorrect));
        }
        return list;
    }

    protected override void ShowQuestion(Question119 question)
    {
        finger.SetActive(true);
        imageAlphabetUpper.sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.Yellow, eAlphbetType.Upper, question.correct);
        imageAlphabetLower.sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.Yellow, eAlphbetType.Lower, question.correct);
        var questions = question.RandomQuestions;
        for(int i = 0;i < buttons.Length; i++)
        {
            buttons[i].Init(GameManager.Instance.GetSpriteWord(questions[i]));
        }
    }
}
public class Question119 : SingleQuestion<string>
{
    public Question119(string correct, string[] questions) : base(correct, questions)
    {
    }
}
