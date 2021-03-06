using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

#region Contents
public abstract class BaseContents : MonoBehaviour
{
    public eAlphabet targetAlphabet;
    [SerializeField]
    private GameObject popupResult;
    protected abstract eContents contents { get; }
    protected eAlphabetType type = eAlphabetType.Upper;
    protected virtual eGameResult GetResult() => eGameResult.Perfect;
    private float time;
    private DateTime startTime;
    private DateTime endTime;
    public AudioSinglePlayer audioPlayer;
    protected virtual void ShowResult()
    {
        GC.Collect();
        endTime = DateTime.Now;
        var result = PopupManager.Instance.Popup<PopupResult>(popupResult);
        result.SetResult(GetResult());
        RequestManager.Instance.RequestAct(param, response =>
        {
            Debug.Log("결과 받기 완료");
        });
    }
    protected virtual EduLogParam param => new EduLogParam(
        "",
        DateTime.Now,
        contents,
        GameManager.Instance.currentAlphabet,
        type,
        GetLevel(),
        DateTime.Now,
        endTime - startTime,
        GetTotalScore(),
        GetCorrectScore(),
        GetDuration()
        );
    protected virtual void Awake()
    {
#if UNITY_EDITOR
        GameManager.Instance.currentAlphabet = targetAlphabet;
#endif
        startTime = DateTime.Now;
    }
    protected abstract bool CheckOver();
    protected virtual int GetLevel() => 1;
    protected abstract int GetTotalScore();
    protected virtual int GetCorrectScore()=>GetTotalScore();
    protected virtual float GetDuration()=>100f;
}
public abstract class SingleAnswerContents<TQuestion,TAnswer> : BaseContents
    where TQuestion : Question<TAnswer>
{
    public List<TQuestion> questions;
    protected abstract int QuestionCount { get; }
    protected int currentQuestionIndex = 0;
    protected TQuestion currentQuestion => questions[currentQuestionIndex];
    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);

    protected override void Awake()
    {
        base.Awake();
        questions = MakeQuestion();
        currentQuestionIndex = 0;
        ShowQuestion(questions[currentQuestionIndex]);
    }
    protected virtual void AddAnswer(TAnswer answer)
    {
        var question = questions[currentQuestionIndex];
        question.SetAnswer(answer);
        if (CheckOver())
            ShowResult();
        else
        {
            if (question.isCorrect)
                audioPlayer.Play(1f,GameManager.Instance.GetClipCorrectEffect());
            currentQuestionIndex += 1;
            ShowQuestion(questions[currentQuestionIndex]);
        }
    }
    protected override eGameResult GetResult()
    {
        var corrects = questions.Select(x => x.isCorrect);
        if (!corrects.Contains(false))
            return eGameResult.Perfect;
        else if (!corrects.Contains(true))
            return eGameResult.Fail;
        else
            return eGameResult.Greate;
    }
    protected abstract void ShowQuestion(TQuestion question);
    protected abstract List<TQuestion> MakeQuestion();
    protected override int GetTotalScore() => QuestionCount;
    protected override int GetCorrectScore() => questions.Where(x => x.isCorrect).Count();
    protected override float GetDuration() => (float)questions.Where(x => x.isCompleted).Count() / (float)QuestionCount;
}
public abstract class MultiAnswerContents<TQuestion,TAnswer> : SingleAnswerContents<TQuestion,TAnswer>
    where TQuestion:MultiQuestion<TAnswer>
{
    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);
    protected override void AddAnswer(TAnswer answer)
    {
        currentQuestion.SetAnswer(answer);
        if (CheckOver())
        {
            ShowResult();
        }
        else if(currentQuestion.isCompleted)
        {
            if (currentQuestion.isCorrect)
                audioPlayer.Play(1f,GameManager.Instance.GetClipCorrectEffect());
            currentQuestionIndex += 1;
            ShowQuestion(questions[currentQuestionIndex]);
        }
    }
}
#endregion

#region BaseQuestions
public abstract class Question<TAnswer>
{
    protected Question()
    {
        isCorrect = false;
        isCompleted = false;
    }

    public bool isCorrect { get; protected set; }
    public bool isCompleted { get; protected set; }
    public virtual void SetAnswer(TAnswer answer)
    {
        isCorrect = CheckCorrect(answer);
        isCompleted = true;
    }
    protected abstract bool CheckCorrect(TAnswer answer);
}
public abstract class SingleQuestion<TAnswer> : Question<TAnswer>
{
    public TAnswer[] totalQuestion { get; protected set; }
    public TAnswer correct { get; protected set; }
    public TAnswer[] questions { get; protected set; }

    protected SingleQuestion(TAnswer correct, TAnswer[] questions) : base()
    {
        this.correct = correct;
        this.questions = questions;
        totalQuestion = Merge();
    }
    protected virtual TAnswer[] Merge()
    {
        var result = questions.Union(new TAnswer[] { correct })
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        return result;
    }

    protected sealed override bool CheckCorrect(TAnswer answer) => correct.Equals(answer);

}
public abstract class MultiQuestion<TAnswer> : Question<TAnswer>
{
    public TAnswer[] totalQuestion { get; private set; }
    public int correctCount { get; protected set; }
    public TAnswer[] correct { get; protected set; }
    public TAnswer[] questions { get; protected set; }

    protected List<TAnswer> currentAnswers;

    protected MultiQuestion(TAnswer[] correct, TAnswer[] questions):base()
    {
        currentAnswers = new List<TAnswer>();
        this.correctCount = correct.Length;
        this.correct = correct;
        this.questions = questions;
        totalQuestion = Merge();
    }
    protected virtual TAnswer[] Merge()
    {
        return questions.Union(correct)
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
    }


    public override void SetAnswer(TAnswer answer)
    {
        currentAnswers.Add(answer);
        isCorrect = CheckCorrect(answer);
        isCompleted = correctCount == currentAnswers.Count;
        if (isCompleted)
        {
            isCorrect = true;
            for(int i = 0;i < currentAnswers.Count; i++)
            {
                if (!CheckCorrect(currentAnswers[i]))
                {
                    isCorrect = false;
                    break;
                }
            }
        }
    }
}
#endregion
