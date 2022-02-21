using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Contents
public abstract class BaseContents : MonoBehaviour
{
    [SerializeField]
    private GameObject popupResult;
    protected abstract eContents contents { get; }
    protected virtual eGameResult GetResult() => eGameResult.Perfect;
    protected virtual void ShowResult()
    {
        var result = PopupManager.Instance.Popup<PopupResult>(popupResult);
        result.SetResult(GetResult());
    }
    protected abstract bool CheckOver();
}
public abstract class SingleAnswerContents<TQuestion,TAnswer> : BaseContents
    where TQuestion : Question<TAnswer>
{
    public List<TQuestion> questions;
    protected abstract int QuestionCount { get; }
    protected int currentQuestion = 0;
    public AudioSinglePlayer audioPlayer;
    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);

    protected virtual void Awake()
    {
        questions = MakeQuestion();
        currentQuestion = 0;
        ShowQuestion(questions[currentQuestion]);
    }
    protected virtual void AddAnswer(TAnswer answer)
    {
        var question = questions[currentQuestion];
        question.SetAnswer(answer);
        if (CheckOver())
            ShowResult();
        else
        {
            if (question.isCorrect)
                audioPlayer.Play(GameManager.Instance.GetClipCorrectEffect());
            currentQuestion += 1;
            ShowQuestion(questions[currentQuestion]);
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
}
public abstract class MultiAnswerContents<TQuestion,TAnswer> : SingleAnswerContents<TQuestion,TAnswer>
    where TQuestion:MultiQuestion<TAnswer>
{
    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);
    protected override void AddAnswer(TAnswer answer)
    {
        var question = questions[currentQuestion];
        question.SetAnswer(answer);
        if (CheckOver())
        {
            ShowResult();
        }
        else if(question.isCompleted)
        {
            if (question.isCorrect)
                audioPlayer.Play(GameManager.Instance.GetClipCorrectEffect());
            currentQuestion += 1;
            ShowQuestion(questions[currentQuestion]);
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
    public TAnswer correct { get; protected set; }
    public TAnswer[] questions { get; protected set; }

    protected SingleQuestion(TAnswer correct, TAnswer[] questions) : base()
    {
        this.correct = correct;
        this.questions = questions;
    }

    protected sealed override bool CheckCorrect(TAnswer answer) => correct.Equals(answer);

}
public abstract class MultiQuestion<TAnswer> : Question<TAnswer>
{
    public int correctCount { get; protected set; }
    public TAnswer[] correct { get; protected set; }
    public TAnswer[] questions { get; protected set; }

    protected List<TAnswer> currentAnswers;

    protected MultiQuestion(int correctCount, TAnswer[] correct, TAnswer[] questions):base()
    {
        currentAnswers = new List<TAnswer>();
        this.correctCount = correctCount;
        this.correct = correct;
        this.questions = questions;
    }

    protected override bool CheckCorrect(TAnswer answer)
    {
        currentAnswers.Add(answer);
        if (correctCount == currentAnswers.Count)
            return currentAnswers.Where(x => !correct.Contains(x)).Count() > 0;
        else
            return false;
    }
    public override void SetAnswer(TAnswer answer)
    {
        isCorrect = CheckCorrect(answer);
        isCompleted = correctCount == currentAnswers.Count;
    }
}
#endregion
