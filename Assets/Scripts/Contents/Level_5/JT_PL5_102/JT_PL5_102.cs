using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL5_102 : SingleAnswerContents<AlphabetContentsSetting, Question5_102, DigraphsWordsData>
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL5_102;

    protected override int QuestionCount => 3;

    public Text orizinalText;
    public SlotMachine502 orizinalSlot;
    public RectTransform content;
    public Image image;
    public Button buttonSloting;

    [SerializeField]
    private SlotMachine502 targetSlot;


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        guideFinger.DoMove(buttonSloting.transform.position, () => isNext = true);
        while (!isNext) yield return null;
        isNext = false;


        guideFinger.DoClick(() =>
        {
            guideFinger.gameObject.SetActive(false);
            buttonSloting.interactable = false;
            //eventSystem.enabled = false;
            image.gameObject.SetActive(false);
            targetSlot.Sloting(currentQuestion.values, () =>
            {
                if (isGuide)
                {
                    image.sprite = currentQuestion.correct.sprite;
                    image.preserveAspect = true;
                    image.gameObject.SetActive(true);
                    audioPlayer.Play(currentQuestion.correct.clip, () =>
                    {
                        if (isGuide)
                        {
                            image.gameObject.SetActive(false);
                            AddAnswer(currentQuestion.correct);
                            isNext = true;
                            buttonSloting.interactable = true;
                        }
                    });
                }
            });
        });
        while (!isNext) yield return null;
        isNext = false;

    }

    protected override void OnAwake()
    {
        base.OnAwake();
        buttonSloting.onClick.AddListener(OnClickSloting);
    }
    protected override void EndGuidnce()
    {
        base.EndGuidnce();
        targetSlot.Kill();
        buttonSloting.interactable = true;
    }
    protected override void ShowQuestion(Question5_102 question)
    {
        Clear();
        var values = question.correct.key
            .Replace(question.correct.IncludedDigraphs, string.Format(" {0} ", question.correct.IncludedDigraphs))
            .Split(' ');
        for(int i = 0;i < values.Length; i++)
        {
            if(values[i] == question.correct.IncludedDigraphs)
            {
                targetSlot = Instantiate(orizinalSlot, content);
            }
            else
            {
                Instantiate(orizinalText,content).text = values[i];
            }
        }

        eventSystem.enabled = true;
    }
    private void Clear()
    {
        var tmp = new List<GameObject>();
        for (int i = 0; i < content.childCount; i++)
            tmp.Add(content.GetChild(i).gameObject);
        for (int i = 0; i < tmp.Count; i++)
            Destroy(tmp[i]);
        tmp.Clear();
        tmp = null;
    }

    protected override List<Question5_102> MakeQuestion()
    {
        var corrects = GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();

        var incorrects = GameManager.Instance.schema.data.digraphsWords
            .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
            .Where(x => !corrects.Select(y => y.IncludedDigraphs).Contains(x.IncludedDigraphs));
            

        var list = new List<Question5_102>();
        for(int i= 0;i < corrects.Length; i++)
        {
            var incorrectTarget = incorrects
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(5)
                .SelectMany(x => new DigraphsWordsData[] { x, x, x })
                .OrderBy(x => Random.Range(0f, 100f))
                .ToArray();
            list.Add(new Question5_102(corrects[i], incorrectTarget));
        }
        return list;
    }
    protected void OnClickSloting()
    {
        guideFinger.gameObject.SetActive(false);
        buttonSloting.interactable = false;
        //eventSystem.enabled = false;
        image.gameObject.SetActive(false);
        targetSlot.Sloting(currentQuestion.values, () =>
        {
            image.sprite = currentQuestion.correct.sprite;
            image.preserveAspect = true;
            image.gameObject.SetActive(true);
            audioPlayer.Play(currentQuestion.correct.clip, () =>
            {
                image.gameObject.SetActive(false);
                AddAnswer(currentQuestion.correct);
                isNext = true;
                buttonSloting.interactable = true;
            });
        });
    }
}
public class Question5_102 : SingleQuestion<DigraphsWordsData>
{
    public string[] values => questions.Select(x => x.IncludedDigraphs).Union(new string[] { correct.IncludedDigraphs }).ToArray();
    public Question5_102(DigraphsWordsData correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
        totalQuestion = questions.Union(new DigraphsWordsData[] { correct }).ToArray();
    }
}
