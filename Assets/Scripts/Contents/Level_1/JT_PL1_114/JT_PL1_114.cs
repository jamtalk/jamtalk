using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class JT_PL1_114 : SingleAnswerContents<AlphabetContentsSetting, Question114, AlphabetWordsData>
{
    public DropSapceShip_114 ship;
    public DragObject_114[] drags;
    public GameObject finger;
    protected override int QuestionCount => 4;

    protected override eContents contents => eContents.JT_PL1_114;
    protected override void Awake()
    {
        base.Awake();
        ship.button.onClick.AddListener(() =>
        {
            if (finger != null)
            {
                Destroy(finger);
                finger = null;
            }
        });
        ship.onInner += (value) =>
        {
            AddAnswer(value);
            //if (!CheckOver())
                //ship.OutObject(currentQuestion.alphabet, () => SetIntractable(true));
        };
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i].onDrag += () =>
            {
                if (finger != null)
                    finger.gameObject.SetActive(false);
            };
            drags[i].onAnswer += (correct) =>
            {
                if (finger != null)
                    finger.gameObject.SetActive(!correct);
            };
            AddDragCallback(drags[i]);
        }

        var tmp = questions.SelectMany(x => x.questions).Select(x => x.clip).ToArray();
        for (int i = 0;i < tmp.Length; i++)
            SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(tmp));
    }
    private void AddDragCallback(DragObject_114 drag)
    {
        drag.onAnswer += (value) =>
        {
            audioPlayer.Stop();
            if (!value)
                audioPlayer.PlayIncorrect(drag.data.clip);
            else
                SetIntractable(false);
        };
    }


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;

        isNext = false;
        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
        guideFinger.gameObject.SetActive(true);
        var target = drags.Where(x => x.data == currentQuestion.correct).First();

        guideFinger.DoMove(target.transform.position, () =>
        {
            guideFinger.DoPress(() =>
            {
                guideFinger.DoMove(target.gameObject, ship.transform.position, () =>
                {
                    guideFinger.gameObject.SetActive(false);
                    target.gameObject.SetActive(false);
                    ship.InObject(target.data);
                    target.rt.anchoredPosition = Vector2.zero;
                });
            });
        });

    }

    protected override List<Question114> MakeQuestion()
    {
        var correct = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(QuestionCount/2)
            .SelectMany(x=>new eAlphabet[] {x,x})
            .ToArray();
        var list = new List<Question114>();
        for(int i = 0;i < QuestionCount; i++)
        {
            var correctWord = GameManager.Instance.GetResources(correct[i]).Words
            .OrderBy(y => UnityEngine.Random.Range(0f, 100f))
            .First();
            //var correctWord = GameManager.Instance.GetResources(eAlphabet.X).Words.Where(x => x.key == "fox").First();

            var incorrect = GameManager.Instance.alphabets
                .Where(x => !correct.Contains(x))
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
                .Take(drags.Length - 1)
                .ToArray();

            list.Add(new Question114(correctWord, incorrect));
        }
        return list;
    }

    protected override void ShowQuestion(Question114 question)
    {
        SetIntractable(false);
        ship.SetInner();
        //if (isGuide)
            ship.OutObject(question.alphabet, () =>
            {
                Debug.Log("????");
                isNext = true;
                SetIntractable(true);
            });
        var questions = question.questions.Union(new AlphabetWordsData[] { question.correct })
        .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
        .ToArray();
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i].Init(questions[i]);
        }
    }

    private void SetIntractable(bool intracable)
    {
        Debug.LogFormat("Intractable : {0}", intracable);
        for (int i = 0; i < drags.Length; i++)
        {
            drags[i].intracable = intracable;
        }
    }
    protected override void EndGuidnce()
    {
        ship.KillTween();
        ship.SetInner();
        audioPlayer.Stop();
        SetIntractable(false);
        foreach (var item in drags)
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        base.EndGuidnce();
    }
}
public class Question114 : SingleQuestion<AlphabetWordsData>
{
    public eAlphabet alphabet => correct.Key;
    public Question114(AlphabetWordsData correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
    }
}
