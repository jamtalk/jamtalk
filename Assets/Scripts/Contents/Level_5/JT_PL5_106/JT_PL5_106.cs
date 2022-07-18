using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JT_PL5_106 : SingleAnswerContents<Question_PL5_106, DigraphsWordsData>
{
    public float duration;
    protected override int QuestionCount => 3;
    protected override eContents contents => eContents.JT_PL5_106;
    public StarElement506 orizinal;
    public RectTransform elementsParent;
    public List<StarElement506> elements;
    public RectTransform[] paths;
    public ResultStart506 resultStar;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override List<Question_PL5_106> MakeQuestion()
    {
        return GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            //.Take(QuestionCount)
            .Select(x => new Question_PL5_106(x))
            .ToList();
    }

    protected override void ShowQuestion(Question_PL5_106 question)
    {
        resultStar.gameObject.SetActive(false);
        var pos = paths
            .OrderBy(x => Random.Range(0f, 100f))
            .Select(x=>x.transform.position)
            .ToArray();
        if (elements.Count < question.words.Length)
        {
            var craetCount = question.words.Length - elements.Count;
            for (int i = 0; i < craetCount; i++)
                elements.Add(Instantiate(orizinal, elementsParent));
        }

        for(int i = 0;i < elements.Count; i++)
        {
            if (i < question.words.Length)
                elements[i].Init(question.words[i], pos[i], duration, OnDrop);
            else
                elements[i].gameObject.SetActive(false);
        }
    }
    private void OnDrop(string value)
    {
        var stars = elements.Where(x => x.gameObject.activeSelf).ToArray();
        var correct = currentQuestion.correct.key.ToLower();
        if (correct == value)
        {
            resultStar.gameObject.SetActive(true);
            var seq = resultStar.Show(value, 1f);
            seq.onComplete += () =>
            {
                audioPlayer.Play(currentQuestion.correct.act, () => AddAnswer(currentQuestion.correct));
            };
        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
                stars[i].ResetLine();
        }
    }
}
public class Question_PL5_106 : SingleQuestion<DigraphsWordsData>
{
    public string[] words;
    public Question_PL5_106(DigraphsWordsData correct) : base(correct, new DigraphsWordsData[] { })
    {
        words = correct.key
            .Replace(correct.digraphs.ToLower(), " ")
            .Split(' ')
            .Where(x => !string.IsNullOrEmpty(x))
            .Union(new string[] { correct.digraphs.ToLower() })
            .OrderBy(x=>Random.Range(0f,100f))
            .ToArray();

    }
}
