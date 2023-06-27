using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL5_106 : SingleAnswerContents<AlphabetContentsSetting, Question_PL5_106, DigraphsWordsData>
{
    public float duration;
    protected override int QuestionCount => 3;
    protected override eContents contents => eContents.JT_PL5_106;
    public StarElement506 orizinal;
    public RectTransform elementsParent;
    public List<StarElement506> elements;
    public RectTransform[] paths;
    public ResultStart506 resultStar;
    public CanvasScaler scaler;


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;
        isNext = false;

        for (int j = 0; j < currentQuestion.orizinWords.Count; j++)
        {
            var target = elements.Where(x => x.orizinalValue == currentQuestion.orizinWords[j]).First();

            guideFinger.DoMove(target.transform.position, () =>
            {
                guideFinger.DoPress(() => { isNext = true; });

                if (j < elements.Count)
                {
                    var nextTarget = elements.Where(x => x.orizinalValue == currentQuestion.orizinWords[j + 1]).First();
                    target.SetGuideLine(1f, nextTarget);
                }
            });
            while (!isNext) yield return null;
            isNext = false;
        }

        guideFinger.gameObject.SetActive(false);
        OnDrop(currentQuestion.correct.key.ToLower());
    }

    protected override void OnAwake()
    {
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        new Question_PL5_106(GameManager.Instance.schema.data.digraphsWords.ToList().Find(x => x.key.ToLower() == "church"));
        base.OnAwake();
    }
    protected override List<Question_PL5_106> MakeQuestion()
    {
        return GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new Question_PL5_106(x))
            .ToList();
    }

    protected override void ShowQuestion(Question_PL5_106 question)
    {
        StartCoroutine(ShowQuestionRoutine(question));
    }
    private void OnDrop(string value)
    {
        var stars = elements.Where(x => x.gameObject.activeSelf).ToArray();
        var correct = currentQuestion.correct.key.ToLower();
        if (correct == value)
        {
            CorrectMotion(value);
        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
                stars[i].ResetLine();

            audioPlayer.PlayIncorrect();
        }
    }

    private void CorrectMotion(string value)
    {
        resultStar.gameObject.SetActive(true);
        var seq = resultStar.Show(value, 1f);
        seq.onComplete += () =>
        {
            audioPlayer.Play(currentQuestion.correct.act, () =>
            {
                AddAnswer(currentQuestion.correct);
                isNext = true;

                foreach (var item in elements)
                {
                    var width = item.line_rt.rect.width;
                    width = 0f;
                    var size = item.line_rt.sizeDelta;
                    size.x = width;
                    item.line_rt.sizeDelta = size;
                }
            });
        };
    }

    private IEnumerator ShowQuestionRoutine(Question_PL5_106 question)
    {
        audioPlayer.Play(currentQuestion.correct.clip);
        yield return new WaitForEndOfFrame();
        resultStar.gameObject.SetActive(false);
        var pos = paths
            .OrderBy(x => Random.Range(0f, 100f))
            .Select(x => x.transform.position)
            .ToArray();
        if (elements.Count < question.words.Length)
        {
            var craetCount = question.words.Length - elements.Count;

            for (int i = 0; i < craetCount; i++)
            {
                var starElement = (Instantiate(orizinal, elementsParent));
                elements.Add(starElement);
            }
        }

        for (int i = 0; i < elements.Count; i++)
        {
            if (i < question.words.Length)
                elements[i].Init(question.words[i], pos[i], duration, OnDrop);
            else
                elements[i].gameObject.SetActive(false);

            //Debug.Log(question.words[i] + " : "+ i );
        }
        isNext = true;
    }
}
public class Question_PL5_106 : SingleQuestion<DigraphsWordsData>
{
    public string[] words;
    public List<string> orizinWords= new List<string>();
    public int digraphsIndex;
    public Question_PL5_106(DigraphsWordsData correct) : base(correct, new DigraphsWordsData[] { })
    {
        var digraphs = correct.IncludedDigraphs;
        digraphsIndex = correct.key.IndexOf(digraphs);
        var options = correct.key.Replace(digraphs, string.Empty).ToArray();

        var list = new List<string>();
        foreach (var item in options)
        {
            list.Add(item.ToString());
            orizinWords.Add(item.ToString());
        }
        orizinWords.Insert(digraphsIndex, digraphs);
        list.Add(digraphs);
        words = list.OrderBy(x=> Random.Range(0,100)).ToArray();



        //words = correct.key
        //    .Replace(correct.IncludedDigraphs, string.Format(" {0} ",correct.IncludedDigraphs))
        //    .Split(' ')
        //    .Where(x => !string.IsNullOrEmpty(x))
        //    .Where(x=>!string.IsNullOrWhiteSpace(x))
        //    .OrderBy(x=>Random.Range(0f,100f))
        //    .ToArray();
        //Debug.LogFormat("{0}??\n{1}", words.Length, string.Join("\n",words));
    }
}
