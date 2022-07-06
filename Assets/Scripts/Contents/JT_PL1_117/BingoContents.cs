using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public abstract class BingoContents<TValue, TButton, TViewer, TBoard> : BaseContents
    where TBoard : BaseBingoBoard<TValue, TViewer, TButton>
    where TButton : BaseBingoButton<TValue, TViewer>
    where TViewer : MonoBehaviour
{
    public TBoard board;
    public BingoScoreBoard scoreBoard;
    public Button buttonSound;
    public int BingoCount => 1;
    public RectTransform bingo;
    public AudioSinglePlayer audioPlayer;
    //????
    public int currentIndex = 0;
    public TValue[] questions;
    public TValue[] corrects;
    public TValue currentQuestion => corrects[currentIndex];
    protected override bool CheckOver() => board.GetBingoCount() >= BingoCount;
    protected override int GetTotalScore() => BingoCount;

    protected abstract TValue[] correctsTarget { get; }
    protected override void Awake()
    {
        base.Awake();
        buttonSound.onClick.AddListener(PlaySound);
        scoreBoard.onFailed += ShowResult;

        questions = GetQuestion();

        currentIndex = 0;
        PlaySound();
        board.Init(questions, corrects, IsCurrentAnswer, OnClick);
    }

    public abstract TValue[] GetQuestionType();

    public TValue[] GetQuestion()
    {
        var questions = GetQuestionType();
        //var questions = GameManager.Instance.alphabets
        //    .Where(x => !correctsTarget.Contains(x))
        //    .Take((int)Mathf.Pow(board.size, 2f))
        //    .OrderBy(x => Random.Range(0f, 100f))
        //    .ToArray();
        //?????? ????
        var corrects = new List<TValue>();
        for (int i = 0; i < board.size; i++)
            corrects.Add(correctsTarget.OrderBy(x => Random.Range(0f, 100f)).First());
        this.corrects = corrects.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        //???? ?????? ????
        var startPosX = Random.Range(0, board.size);
        var startPosY = Random.Range(0, board.size);
        var correctpos = new List<int>();
        if (startPosY == 0)
        {
            if (startPosX == 0 || startPosX == board.size - 1)
            {
                var type = Random.Range(0, 3);
                switch (type)
                {
                    case 0: //??????
                        var _startPos = startPosX;
                        for (int i = 0; i < board.size; i++)
                        {
                            correctpos.Add(board.size * i + _startPos);

                            if (startPosX == 0)
                                _startPos += 1;     //?????? ??????
                            else
                                _startPos -= 1;     //?????? ??????
                        }
                        break;
                    case 1: //????
                        for (int i = 0; i < board.size; i++)
                            correctpos.Add(board.size * i + startPosX);
                        break;
                    case 2: //????
                        for (int i = 0; i < board.size; i++)
                            correctpos.Add(i);
                        break;
                }
            }
            else
            {
                for (int i = 0; i < board.size; i++)
                    correctpos.Add(board.size * i + startPosX);
            }
        }
        else
        {
            for (int i = 0; i < board.size; i++)
                correctpos.Add(startPosY * board.size + i);
        }

        for (int i = 0; i < board.size; i++)
        {
            questions[correctpos[i]] = corrects[i];
        }

        var question = string.Empty;
        for (int i = 0; i < questions.Length; i++)
        {
            if (i % board.size == 0)
                question += "\n";
            question += questions[i];
        }
        Debug.Log(question);
        return questions;
    }
    protected override eGameResult GetResult()
    {
        if (currentIndex == 0)
            return eGameResult.Fail;
        else if (board.GetBingoCount() < BingoCount)
            return eGameResult.Greate;
        else
            return eGameResult.Perfect;
    }
    protected override void ShowResult()
    {
        bingo.gameObject.SetActive(true);

        var tween = bingo.DOScale(1.5f, .5f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(2, LoopType.Yoyo);
        tween.onComplete += () => base.ShowResult();
        tween.Play();
    }
    protected abstract bool IsCurrentAnswer(TValue value);
    public void OnClick(TValue value)
    {
        if (IsCurrentAnswer(value))
        {
            currentIndex += 1;
            scoreBoard.AddScore(100);
        }
        else
            scoreBoard.IncreaseIncorrect();

        PlaySound();

    }
    protected abstract void GetClip();
    public void PlaySound()
    {
        if (CheckOver())
            ShowResult();
        else
            GetClip();
    }
}
