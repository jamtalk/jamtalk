using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL4_106 : SingleAnswerContents<Question4_106, DigraphsWordsData>
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL4_106;

    protected override int QuestionCount => 4;

    public Text currentText;

    public DoubleClickButton4_104[] buttons;
    protected override List<Question4_106> MakeQuestion()
    {
        var corrects = GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();

        var incorrects = GameManager.Instance.schema.data.digraphsWords
            .Where(x => !corrects.Select(y => y.Digraphs).Contains(x.Digraphs))
            .Where(x => !corrects.Select(y => y.IncludedDigraphs).Contains(x.IncludedDigraphs));

        var list = new List<Question4_106>();
        for (int i = 0; i < QuestionCount; i++)
        {
            list.Add(new Question4_106(corrects[i], 
                incorrects
                    .OrderBy(x => Random.Range(0f, 100f))
                    .Take(buttons.Length - 1)
                    .ToArray()));
        }

        return list;
    }
    protected override void ShowQuestion(Question4_106 question)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ButtonAddListener(buttons[i], question.totalQuestion[i]);
            buttons[i].isOn = false;
            buttons[i].incorrectMark.SetActive(false);
            buttons[i].button.interactable = true;
        }
        audioPlayer.Play(question.correct.clip);
        currentText.text = question.correct.key.Replace(question.correct.IncludedDigraphs
                , "<color=\"red\">" + question.correct.IncludedDigraphs + "</color>");
    }
    private void ButtonAddListener(DoubleClickButton4_104 button, DigraphsWordsData data)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].isOn = buttons[i] == button;

            eventSystem.enabled = false;

            var isCorrect = currentQuestion.correct.IncludedDigraphs == data.IncludedDigraphs;
            button.incorrectMark.SetActive(!isCorrect);
            button.button.interactable = isCorrect;
            Debug.Log("??");
            audioPlayer.Play(data.audio.phanics, () =>
            {
                button.isOn = currentQuestion.correct.Digraphs == data.Digraphs;
                button.SetFirstImages();
                eventSystem.enabled = true;
            });
        });

        button.onClick.AddListener(() =>
        {
            eventSystem.enabled = false;
            button.SetLastImages();
            audioPlayer.Play(data.act, () =>
            {
                audioPlayer.Play(.8f,GameManager.Instance.GetClipCorrectEffect(), () =>
                {
                    eventSystem.enabled = true;
                    if (CheckOver())
                        ShowResult();
                    else
                        AddAnswer(currentQuestion.correct);
                });
            });
        });
    }
}

public class Question4_106 : SingleQuestion<DigraphsWordsData>
{
    public Question4_106(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }
}
