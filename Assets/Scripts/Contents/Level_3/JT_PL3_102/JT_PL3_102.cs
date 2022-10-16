using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_102 : MultiAnswerContents<Question3_102, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL3_102;
    protected override int QuestionCount => 3;

    public Sprite backImage;
    public Sprite frontImage;
    public Image spatulaImage;
    public DoubleClick302[] pancakes;

    
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for (int i = 0; i < QuestionCount; i++)
        {
            for (int j = 0; j < pancakes.Length; j++)
            {
                var target = pancakes.Where(x => !x.isCheck).OrderBy(x => Random.Range(0, 100)).First();

                guideFinger.DoMove(target.transform.position, () =>
                {
                    guideFinger.DoClick(() =>
                    {
                        target.isOn = true;

                        audioPlayer.Play(target.data.audio.phanics, () =>
                        guideFinger.DoClick(() =>
                        {
                            guideFinger.gameObject.SetActive(false);
                            ClickMotion(target);
                        }));
                    });
                });
                while (!isNext) yield return null;
                isNext = false;
            }
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
            pancakes[i].isOn = false;
            pancakes[i].images.gameObject.SetActive(false);
            pancakes[i].textPhanix.gameObject.SetActive(true);

            pancakes[i].Init(question.totalQuestion[i]);
            AddListener(pancakes[i]);
        }
    }

    private void AddListener(DoubleClick302 button)
    {
        button.onClick.RemoveAllListeners();
        button.onClickFirst.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            audioPlayer.Play(button.data.audio.phanics);
        });

        button.onClick.AddListener(() =>
        {
            if (!button.isCheck)
            {
                ClickMotion(button);
            }
        });
    }

    private void ClickMotion(DoubleClick302 button)
    {
        button.isCheck = true;

        button.image.sprite = backImage;
        button.textPhanix.gameObject.SetActive(false);
        button.images.gameObject.SetActive(true);
        button.images.preserveAspect = true;
        audioPlayer.Play(button.data.act, () =>
        {
            button.image.sprite = frontImage;
            button.textPhanix.text = button.data.key;
            button.images.gameObject.SetActive(false);
            button.textPhanix.gameObject.SetActive(true);
            AddAnswer(button.data);
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