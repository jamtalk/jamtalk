using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class JT_PL3_102 : MultiAnswerContents<AlphabetContentsSetting, Question3_102, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL3_102;
    protected override int QuestionCount => 3;

    public Sprite backImage;
    public Sprite frontImage;
    public SpatulaElement spatulaImage;
    public PanCakeElement[] pancakes;
    public Image[] panckede;
    private int index = 0;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();
        spatulaImage.isGuide = true;

        var target = pancakes.Where(x => !x.isCheck).OrderBy(x => Random.Range(0, 100)).First();

        guideFinger.DoMove(target.transform.position, () =>
        {

            guideFinger.DoPress(() =>
            {
                guideFinger.DoMove(target.rects, spatulaImage.gameObject, () =>
                {
                    target.BG.sprite = target.secondSprite;
                    audioPlayer.Play(target.data.audio.phanics, () =>
                    {
                        guideFinger.DoMove(target.rects, spatulaImage.gameObject, () =>
                         {
                             guideFinger.gameObject.SetActive(false);
                             spatulaImage.gameObject.SetActive(false);
                             guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
                             ClickMotion(target, () =>
                             {
                                 guideFinger.DoClick(() =>
                                 {
                                     panckede[0].gameObject.SetActive(true);
                                     target.gameObject.SetActive(false);
                                     guideFinger.gameObject.SetActive(false);

                                     isNext = true;
                                 });
                             });
                         });
                    });
                });
            });
        });
        while (!isNext) yield return null;
        isNext = false;

        yield return new WaitForSecondsRealtime(1f);

        AddAnswer(target.data);

        spatulaImage.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (var item in pancakes)
        {
            item.onFirst += () => audioPlayer.Play(item.data.audio.phanics);
            item.onDouble += () => ClickMotion(item);
            item.onClick += () => StackCake(item);
        }
    } 

    protected override List<Question3_102> MakeQuestion()
    {
        var questions = new List<Question3_102>();
        for (int i = 0; i < QuestionCount; i++)
        {
            var corrects = GameManager.Instance.GetDigraphs().Take(pancakes.Length).ToList();
            if (corrects.Count < pancakes.Length)
            {
                var tmp = new List<DigraphsWordsData>();
                for (int j = 0; j < pancakes.Length; j++)
                {
                    tmp.Add(corrects[j % corrects.Count]);
                }
                corrects = tmp;
            }
            questions.Add(new Question3_102(corrects.OrderBy(x => Random.Range(0f, 100f)).ToArray(), new DigraphsWordsData[] { }));
        }
        return questions;
    }

    protected override void ShowQuestion(Question3_102 question)
    {
        ResetElements();

        for (int i = 0; i < pancakes.Length; i++)
            pancakes[i].Init(question.totalQuestion[i]);
    }

    private void ClickMotion(PanCakeElement button, Action action = null)
    {
        button.isCheck = true;

        button.BG.sprite = backImage;
        button.text.gameObject.SetActive(false);
        button.image.gameObject.SetActive(true);
        audioPlayer.Play(button.data.act, () =>
        {
            button.isCompleted = true;
            button.BG.sprite = frontImage;
            button.text.text = button.data.key;
            button.image.gameObject.SetActive(false);
            button.text.gameObject.SetActive(true);

            spatulaImage.isGuide = false;
            action?.Invoke();
        });
    }

    private void StackCake(PanCakeElement item)
    {
        if (!item.isCompleted)
            return;

        panckede[index].gameObject.SetActive(true);
        index++;
        item.gameObject.SetActive(false);

        AddAnswer(item.data);
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        ResetElements();
    }

    private void ResetElements()
    {
        foreach (var item in pancakes)
            item.gameObject.SetActive(true);

        foreach (var item in panckede)
            item.gameObject.SetActive(false);

        index = 0;
    }
}


public class Question3_102 : MultiQuestion<DigraphsWordsData>
{
    public int currentIndex { get; private set; } = 0;
    public DigraphsWordsData currentCorrect => correct[currentIndex];

    public Question3_102(DigraphsWordsData[] correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }

    protected override bool CheckCorrect(DigraphsWordsData answer) => true;
    public override void SetAnswer(DigraphsWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}