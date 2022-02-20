using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseContents : MonoBehaviour
{
    [SerializeField]
    private GameObject popupResult;
    protected abstract eContents contents { get; }
    protected abstract eGameResult GetResult();
    protected virtual void ShowResult()
    {
        var result = PopupManager.Instance.Popup<PopupResult>(popupResult);
        result.SetResult(GetResult());
    }
}
public abstract class SingleAnswerContents : BaseContents
{
    protected override eGameResult GetResult() => eGameResult.Perfect;
    protected abstract bool CheckOver();
}
public abstract class MultiAnswerContents : SingleAnswerContents
{
    protected abstract int AnswerCount { get; }
    protected List<bool> answers = new List<bool>();
    protected sealed override bool CheckOver() => answers.Count == AnswerCount;
    protected virtual void AddAnswer(bool answer)
    {
        answers.Add(answer);
        if (CheckOver())
            ShowResult();
    }
    protected override eGameResult GetResult()
    {
        var correctCount = answers.Where(x => x).Count();
        var failCount = answers.Where(x => !x).Count();
        if (correctCount == AnswerCount)
            return eGameResult.Perfect;
        else if (failCount == AnswerCount)
            return eGameResult.Fail;
        else
            return eGameResult.Greate;
    }
}
