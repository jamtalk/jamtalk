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
    public Button charactorButton;
    public DoubleClickButton4_104[] buttons;


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();


        while (!isNext) yield return null;
        isNext = false;

        var target = buttons.Where(x => x.data.key == currentQuestion.correct.key).First();

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                ButtonClickMotion(target);
            });
        });

        while (!isNext) yield return null;
        isNext = false;
        guideFinger.DoClick(() =>
        {
            guideFinger.gameObject.SetActive(false);
            ButtonDoubleClickMotion(target);
        });

        while (!isNext) yield return null;
        isNext = false;

    }

    protected override void Awake()
    {
        base.Awake();

        charactorButton.onClick.AddListener(() =>
        {
            audioPlayer.Play(currentQuestion.correct.clip);
        });
    }
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
            buttons[i].Init(question.totalQuestion[i]);
            ButtonAddListener(buttons[i]);
        }
        audioPlayer.Play(question.correct.clip, () => isNext = true);
        currentText.text = question.correct.key.Replace(question.correct.IncludedDigraphs
                , "<color=\"red\">" + question.correct.IncludedDigraphs + "</color>");
    }
    private void ButtonAddListener(DoubleClickButton4_104 button)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            ButtonClickMotion(button);
        });

        button.onClick.AddListener(() =>
        {
            ButtonDoubleClickMotion(button);
        });
    }

    private void ButtonClickMotion(DoubleClickButton4_104 button)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].isOn = buttons[i] == button;

        eventSystem.enabled = false;

        button.inCorrectCnt++;
        if (button.inCorrectCnt > 1)
        {
            var isCorrect = currentQuestion.correct.IncludedDigraphs == button.data.IncludedDigraphs;
            button.incorrectMark.SetActive(!isCorrect);
            button.button.interactable = isCorrect;
        }
        audioPlayer.Play(button.data.audio.phanics, () =>
        {
            isNext = true;
            button.isOn = currentQuestion.correct.Digraphs == button.data.Digraphs;
            button.SetFirstImages();
            eventSystem.enabled = true;
        });
    }
    private void ButtonDoubleClickMotion(DoubleClickButton4_104 button)
    {
        eventSystem.enabled = false;
        button.SetLastImages();
        audioPlayer.Play(button.data.act, () =>
        {
            isNext = true;
            eventSystem.enabled = true;
            if (CheckOver())
                ShowResult();
            else
                AddAnswer(currentQuestion.correct);
        });
    }
}

public class Question4_106 : SingleQuestion<DigraphsWordsData>
{
    public Question4_106(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }
}
