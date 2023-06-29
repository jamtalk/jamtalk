using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JT_PL3_106 : SingleAnswerContents<AlphabetContentsSetting, Question_PL3_106, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL2_106;
    //protected override bool CheckOver() => index == QuestionCount;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 5;
    //private int index = 0;

    //private DigraphsWordsData currentDigraphs;
    //private string digraphs;

    public EventSystem eventSystem;
    public Thrower306 thrower;
    public Text[] texts;
    public Text currentText;
    public Button currentButton;
    public Image currentImage;
    public BagElement bag;
    [SerializeField]
    private List<DoubleClick306> elements = new List<DoubleClick306>();


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();


        while (!isNext) yield return null;
        isNext = false;

        var target = elements.Where(x => x.value.key == currentQuestion.correct.key).First();

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                audioPlayer.Play(target.value.audio.phanics, () =>
                    guideFinger.DoClick(() =>
                    {
                        guideFinger.gameObject.SetActive(false);
                        DoubleClickMotion(target.value);
                    }));
            });
        });

        while (!isNext) yield return null;
        isNext = false;

    }
    protected override void OnAwake()
    {
        base.OnAwake();
        currentButton.onClick.AddListener(() =>
        {
            bag.MouseSpeak();
            audioPlayer.Play(currentQuestion.correct.clip);
        });
    }


    protected override List<Question_PL3_106> MakeQuestion()
    {
        var questions = new List<Question_PL3_106>();
        var corrects = GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToList();
        for (int i = 0; i < corrects.Count; i++)
        {
            var incorrects = GameManager.Instance.digrpahs
                .Select(x => GameManager.Instance.GetDigraphs(x).OrderBy(y => Random.Range(0f, 100f)).First())
                .Where(x => !corrects.Select(y => y.digraphs).Contains(x.digraphs))
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(elements.Count - 1)
                .ToArray();
            questions.Add(new Question_PL3_106(corrects[i], incorrects));
        }
        return questions;
    }

    protected override void ShowQuestion(Question_PL3_106 question)
    {
        eventSystem.enabled = true;
        currentImage.sprite = question.correct.sprite;
        currentImage.name = question.correct.key;
        currentImage.preserveAspect = true;
        currentImage.gameObject.SetActive(true);
        currentText.text = question.correct.key.Replace(question.correct.IncludedDigraphs, "__");

        thrower.gameObject.SetActive(false);
        bag.image.gameObject.SetActive(false);

        //var incorrects = question.questions.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        for (int i = 0;i < elements.Count; i++)
        {
            elements[i].Init(question.totalQuestion[i]);
            elements[i].isOn = false;
            elements[i].gameObject.SetActive(true);
            AddDoubleClickListener(elements[i], question.totalQuestion[i]);
        }

        audioPlayer.Play(currentQuestion.correct.clip, () => isNext = true);
    }

    protected virtual void AddDoubleClickListener(DoubleClick306 element, DigraphsWordsData data)
    {
        element.onClickFirst.RemoveAllListeners();
        element.onClick.RemoveAllListeners();

        element.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(data.audio.phanics);
            //audioPlayer.PlayIncorrect(data.audio.phanics);
        });

        element.onClick.AddListener(() =>
        {
            if (currentQuestion.correct.key.Contains(element.name))
                DoubleClickMotion(data);
            else
                audioPlayer.PlayIncorrect(data.audio.phanics);
        });
    }

    private void DoubleClickMotion(DigraphsWordsData data)
    {
        eventSystem.enabled = false;
        bag.EyeAni(BagElement.eAnis.Tracking);
        for (int i = 0; i < elements.Count; i++)
            elements[i].gameObject.SetActive(false);
        currentImage.gameObject.SetActive(false);

        var trowerImage = thrower.GetComponent<Image>();
        trowerImage.sprite = currentImage.sprite;
        trowerImage.preserveAspect = true;

        thrower.gameObject.SetActive(true);
        thrower.Throw(currentImage, bag.imageRt, () =>
        {
            currentText.text = data.key;
            bag.SetBagImage(currentImage.sprite);

            audioPlayer.Play(currentQuestion.correct.act, () =>
            {
                eventSystem.enabled = true;
                bag.EyeAni(BagElement.eAnis.EyeIdle);
                AddAnswer(currentQuestion.correct);
                isNext = true;
            });
        });
    }
}

public class Question_PL3_106 : SingleQuestion<DigraphsWordsData>
{
    public Question_PL3_106(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }
}
