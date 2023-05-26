using GJGameLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

#region Contents
public abstract class BaseContents<TTestSetting> : MonoBehaviour
    where TTestSetting:ContentsTestSetting
{
    [Space(20)]
#if UNITY_EDITOR
    [SerializeField] private TTestSetting testSetting;
#endif
    [Space(20)]
    [SerializeField] private GameObject popupResult;

    public virtual eSceneName NextScene => eSceneName.AD_003;
    protected abstract eContents contents { get; }
    protected eAlphabetType type = eAlphabetType.Upper;
    protected virtual eGameResult GetResult() => eGameResult.Success;
    private float time;
    public DateTime startTime { get; private set; }
    public DateTime endTime { get; private set; }
    public AudioSinglePlayer audioPlayer;
    public GuidePopup guide;
    public ExitButton exitButton;
    public AnimationCharactor[] animationCharactors;
    protected GuidePopup guidePopup;
    protected GuideFingerAnimation guideFinger;
    protected Coroutine guideRoutine;
    protected bool isGuide = true;
    protected bool isNext = false;
    public Action onEndGame;
    protected virtual bool showQuestionOnAwake => true;
    protected virtual bool showPopupOnEnd => true;
    protected virtual bool isGuidence => true;
    protected virtual bool includeExitButton => true;

    private EventSystem eventSystem => FindObjectOfType<EventSystem>();

    private void SetCharactorAnimation(bool isQuestion = true, bool isLoop = false)
    {
        if (animationCharactors != null)
        {
            foreach (var item in animationCharactors)
            {
                var motion = isQuestion ? item.eIdleMotion : item.eCorrectMotion;
                var detail = isQuestion ? item.eIdleDetail : item.eCorrectDetail;
                item.MotionChange(motion, detail, isQuestion ? isQuestion : isLoop);
            }
        }
    }
    protected virtual void ShowQeustionAction()
    {
        SetCharactorAnimation();
    }

    protected virtual void CorrectAction(bool isLoop = false)
    {
        SetCharactorAnimation(false, isLoop);
    }

    protected virtual void ShowResult()
    {
        CorrectAction();
        eventSystem.enabled = true;
        GC.Collect();
        endTime = DateTime.Now;
        if (showPopupOnEnd)
            ShowResultPopup();
        onEndGame?.Invoke();
    }

    protected virtual void ShowResultPopup()
    {
        var result = PopupManager.Instance.Popup<PopupResult>(popupResult);
        result.Init(() =>
        {
            GJSceneLoader.Instance.LoadScene(NextScene);
        }, () =>
        {
            if (GameManager.Instance.currentAlphabet + 1 < eAlphabet.Z)
                GJSceneLoader.Instance.LoadScene(NextScene);
            else
            {
                GameManager.Instance.currentAlphabet += 2;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }, () =>
        {
            GJSceneLoader.Instance.LoadScene(GJSceneLoader.Instance.currentScene + 1);
            GJSceneLoader.Instance.LoadScene(NextScene);
        });

        result.SetResult(GetResult());
        RequestManager.Instance.Request(param, response =>
        {
            Debug.Log("결과 받기 완료");
        });
    }
    public virtual EduLogParam param => new EduLogParam(
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
        testSetting.Apply();
#endif
        startTime = DateTime.Now;
        if (includeExitButton)
            Instantiate(exitButton, transform);

        if(isGuidence)
            ShowGuidnce();
    }
    protected virtual void ShowGuidnce()
    {
        if (guidePopup == null)
            guidePopup = Instantiate(guide, transform);

        guideFinger = guidePopup.guideFinger;
        guideFinger.gameObject.SetActive(false);
        guideRoutine = StartCoroutine(ShowGuidnceRoutine());
        guidePopup.exitButton.onClick.AddListener(() => EndGuidnce());
    }
    protected virtual IEnumerator ShowGuidnceRoutine()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual void EndGuidnce()
    {
        Debug.Log("endgudie");
        isGuide = false;
        guidePopup.gameObject.SetActive(false);
        guidePopup.guideFinger.GuideStop();
        StopCoroutine(guideRoutine);
    }

    protected abstract bool CheckOver();
    protected virtual int GetLevel() => 1;
    protected abstract int GetTotalScore();
    protected virtual int GetCorrectScore()=>GetTotalScore();
    protected virtual float GetDuration()=>100f;
}
public abstract class SingleAnswerContents<TTestSetting,TQuestion, TAnswer> : BaseContents<TTestSetting>
    where TTestSetting : ContentsTestSetting
    where TQuestion : Question<TAnswer>
{
    public List<TQuestion> questions = null;
    protected abstract int QuestionCount { get; }
    protected int currentQuestionIndex = 0;
    protected TQuestion currentQuestion => questions[currentQuestionIndex];
    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);

    protected override void EndGuidnce()
    {
        base.EndGuidnce();

        questions = MakeQuestion();
        currentQuestionIndex = 0;
        ShowQuestion(questions[currentQuestionIndex]);
    }
    protected override void ShowGuidnce()
    {
        base.ShowGuidnce();

        //Debug.Log(GameManager.Instance.currentAlphabet);
        //questions = MakeQuestion();
        //currentQuestionIndex = 0;
        //ShowQuestion(questions[currentQuestionIndex]);
    }

    protected override void Awake()
    {
        if (showQuestionOnAwake)
            StartQuestion();

        base.Awake();
    }
    public void StartQuestion()
    {
        Debug.Log("문제 시작");
        questions = MakeQuestion();
        currentQuestionIndex = 0;
        ShowQuestion(questions[currentQuestionIndex]);
    }
    protected virtual void AddAnswer(TAnswer answer)
    {
        CorrectAction();
        Debug.Log("AddAnswer");
        var question = questions[currentQuestionIndex];
        question.SetAnswer(answer);
        Debug.LogFormat("isOVer : {0}", CheckOver());
        if (CheckOver())
            ShowResult();
        else
        {
            if (isGuide)
                EndGuidnce();
            else
            {
                currentQuestionIndex += 1;
                if (question.isCorrect)
                    audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
                    {
                        ShowQeustionAction();
                        ShowQuestion(questions[currentQuestionIndex]);
                    });
                else
                {
                    audioPlayer.PlayIncorrect();
                    ShowQeustionAction();
                    ShowQuestion(questions[currentQuestionIndex]);
                }
            }
        }
    }
    protected override eGameResult GetResult()
    {
        var corrects = questions.Select(x => x.isCorrect);
        if (!corrects.Contains(false))
            return eGameResult.Success;
        else if (!corrects.Contains(true))
            return eGameResult.Fail;
        else
            return eGameResult.Success;
    }
    protected abstract void ShowQuestion(TQuestion question);
    protected abstract List<TQuestion> MakeQuestion();
    protected override int GetTotalScore() => QuestionCount;
    protected override int GetCorrectScore() => questions.Where(x => x.isCorrect).Count();
    protected override float GetDuration() => (float)questions.Where(x => x.isCompleted).Count() / (float)QuestionCount;
}
public abstract class MultiAnswerContents<TTestSetting,TQuestion, TAnswer> : SingleAnswerContents<TTestSetting,TQuestion, TAnswer>
    where TTestSetting:ContentsTestSetting
    where TQuestion:MultiQuestion<TAnswer>
{
    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);
    protected override void AddAnswer(TAnswer answer)
    {
        currentQuestion.SetAnswer(answer);
        Debug.Log("AddAnswer");
        if (CheckOver())
        {
            ShowResult();
        }
        else if (currentQuestion.isCompleted)
        {
            if (isGuide) EndGuidnce();
            else
            {
                if (currentQuestion.isCorrect)
                    audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
                else
                    audioPlayer.PlayIncorrect();

                currentQuestionIndex += 1;
                ShowQuestion(questions[currentQuestionIndex]);
            }
            Debug.Log("else if");
        }
        else if (isGuide) EndGuidnce();
            
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
    public void ResetAnswer()
    {
        isCompleted = false;
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

#region BookContents
public interface IBookContentsRunner
{
    public void StartQuestion();
}
public abstract class BaseBookContents : BaseContents<BookContentsSetting>
{
    public BookContentsRunner[] contentsRunners;
    protected override sealed bool includeExitButton => false;
    protected override sealed bool isGuidence => false;
    protected sealed override bool showPopupOnEnd => base.showPopupOnEnd;
    protected sealed override bool showQuestionOnAwake => base.showQuestionOnAwake;

    protected override void Awake()
    {
        base.Awake();
        for(int i= 0;i < contentsRunners.Length-1; i++)
            contentsRunners[i].SetNext(contentsRunners[i + 1]);
        contentsRunners.Last().SetLast(ShowResult);
    }

    protected override bool CheckOver() => true;

    protected override int GetTotalScore() => contentsRunners.Sum(x => x.contents.param.total_score);
    public sealed override EduLogParam param => new EduLogParam(
        base.param.app_token,
        base.param.regdate,
        contents,
        base.param.alphabet,
        base.param.alphabetType,
        base.param.level,
        contentsRunners.Last().contents.endTime,
        contentsRunners.First().contents.startTime - contentsRunners.Last().contents.endTime,
        GetTotalScore(),
        contentsRunners.Sum(x => x.contents.param.correct_score),
        contentsRunners.Sum(x => x.contents.param.due));
}
[System.Serializable]
public class BookContentsRunner
{
    public BaseContents<BookContentsSetting> contents;
    private BookContentsRunner nextContents = null;
    public IBookContentsRunner runner => (IBookContentsRunner)contents;
    public void SetNext(BookContentsRunner nextContents)
    {
        contents.onEndGame += OnEndGame;
        this.nextContents = nextContents;
    }
    public void SetLast(Action onEndGame)
    {
        contents.onEndGame += onEndGame;
    }

    private void OnEndGame()
    {
        if (nextContents != null)
        {
            contents.gameObject.SetActive(false);
            nextContents.contents.gameObject.SetActive(true);
            nextContents.runner.StartQuestion();
        }
    }
}
#endregion