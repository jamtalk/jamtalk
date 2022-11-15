using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL3_102 : MultiAnswerContents<Question3_102, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL3_102;
    protected override int QuestionCount => 3;

    public Sprite backImage;
    public Sprite frontImage;
    public SpatulaElement spatulaImage;
    public PanCakeElement[] pancakes;


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();
        spatulaImage.isGuide = true;

        var target = pancakes.Where(x => !x.isCheck).OrderBy(x => Random.Range(0, 100)).First();

        guideFinger.DoMove(target.transform.position, () =>
        {
            //    guideFinger.DoClick(() =>
            //    {
            //        target.isOn = true;

            //        audioPlayer.Play(target.data.audio.phanics, () =>
            //        guideFinger.DoClick(() =>
            //        {
            //            guideFinger.gameObject.SetActive(false);
            //            ClickMotion(target);
            //        }));
            //    });

            guideFinger.DoPress(() =>
            {
                guideFinger.DoShake(spatulaImage.gameObject, () =>
                {
                    target.BG.sprite = target.secondSprite;
                    audioPlayer.Play(target.data.audio.phanics, () =>
                    {
                        guideFinger.DoShake(spatulaImage.gameObject, () =>
                        {
                            guideFinger.gameObject.SetActive(false);
                            spatulaImage.gameObject.SetActive(false);
                            ClickMotion(target);
                        });
                    });
                });
            });
        });
        while (!isNext) yield return null;
        isNext = false;

        spatulaImage.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        foreach(var item in pancakes)
        {
            item.onFirst += () => audioPlayer.Play(item.data.audio.phanics);
            item.onDouble += () => ClickMotion(item);
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
        for (int i = 0; i < pancakes.Length; i++)
        {
            pancakes[i].isCheck = false;
            pancakes[i].image.gameObject.SetActive(false);
            pancakes[i].text.gameObject.SetActive(true);

            pancakes[i].Init(question.totalQuestion[i]);
        }
    }

    private void ClickMotion(PanCakeElement button)
    {
        button.isCheck = true;

        button.BG.sprite = backImage;
        button.text.gameObject.SetActive(false);
        button.image.gameObject.SetActive(true);
        audioPlayer.Play(button.data.act, () =>
        {
            button.BG.sprite = frontImage;
            button.text.text = button.data.key;
            button.image.gameObject.SetActive(false);
            button.text.gameObject.SetActive(true);
            AddAnswer(button.data);
            spatulaImage.isGuide = false;
            isNext = true;
        });
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