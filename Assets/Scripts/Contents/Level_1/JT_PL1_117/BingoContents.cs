using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public abstract class BingoContents<TValue, TButton, TViewer, TBoard> : BaseContents
    where TBoard : BaseBingoBoard<TValue, TViewer, TButton>
    where TButton : BaseBingoButton<TValue, TViewer>
    where TViewer : MonoBehaviour
{
    public TBoard board;
    public TBoard alphaBoard;
    public BingoScoreBoard scoreBoard;
    public Button buttonSound;
    public virtual int BingoCount => 1;
    public int bingoCount = 0;
    public RectTransform bingo;
    public int currentIndex = 0;
    public TValue[] questions;
    public TValue[] corrects;
    public TValue currentQuestion => corrects[currentIndex];
    //protected override bool CheckOver() => board.GetBingoCount() >= BingoCount;
    protected override bool CheckOver() => bingoCount == BingoCount;
    protected override int GetTotalScore() => BingoCount;

    protected TValue[] _correctsTarget = null;
    protected abstract TValue[] correctsTarget { get; }

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for (int i = 0; i < board.size; i++)
        {
            while (!isNext) yield return null;
            isNext = false;
            var target = board.buttons.Where(x=>!x.isOn).Where(x => x.strValue == GetValue()).First();

            guideFinger.gameObject.SetActive(true);
            guideFinger.DoMove(target.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    isNext = true;
                    currentIndex += 1;
                    target.GuideClick();
                });
            });
            while (!isNext) yield return null;
            isNext = false;
            yield return new WaitForSecondsRealtime(1.5f);

            if (i < board.size)
                PlaySound();
        }
    }
    protected abstract string GetValue();
    protected override void Awake()
    {
        board.ResizeBoard(()=>
        {
            base.Awake();
            buttonSound.onClick.AddListener(PlaySound);
            scoreBoard.onFailed += ShowResult;

            questions = GetQuestion();

            currentIndex = 0;
            PlaySound();
            board.Init(questions, corrects, IsCurrentAnswer, OnClick);
        });
    }

    protected void ResizeBoard(Action action =null)
    {
        board.ResizeBoard(() =>
        {
            questions = GetQuestion();

            scoreBoard.GuideScore(0);
            currentIndex = 0;
            PlayClip();
            board.Init(questions, corrects, IsCurrentAnswer, OnClick);
        });
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
        
        if(correctsTarget.Length <board.size)
        {
            for (int i = 0; i < board.size; i++)
                corrects.Add(correctsTarget[i%board.size]);
        }
        else
        {
            corrects = correctsTarget.OrderBy(x => Random.Range(0f, 100f)).Take(board.size).ToList();
        }
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
        return questions;
    }
    protected override eGameResult GetResult()
    {
        if (currentIndex == 0)
            return eGameResult.Fail;
        //else if (board.GetBingoCount() < BingoCount)
        else if( bingoCount < BingoCount)
            return eGameResult.Success;
        else
            return eGameResult.Success;
    }
    
    protected void ShowBingo(TweenCallback callback = null)
    {
        bingoCount++;
        bingo.gameObject.SetActive(true);
        
        var tween = bingo.DOScale(1.5f, .5f);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(2, LoopType.Yoyo);
        tween.onComplete += callback;
        tween.Play();
    }
    protected abstract bool IsCurrentAnswer(TValue value);
    public void OnClick(TValue value)
    {
        if (IsCurrentAnswer(value))
        {
            currentIndex += 1;
            scoreBoard.AddScore(100);
            PlaySound();
        }
        else
        {
            audioPlayer.PlayIncorrect();
            scoreBoard.IncreaseIncorrect();
        }


    }
    protected abstract void PlayClip();

    public void PlaySound()
    {
        //if (CheckOver())
        if(board.GetBingoCount() >= 1)
            ShowResult();
        else
            PlayClip();
    }

    protected override void EndGuidnce()
    {
        foreach (var item in board.buttons)
            item.EndGuide();

        base.EndGuidnce();

        scoreBoard.GuideScore(board.size * 100);
        bingo.gameObject.SetActive(false);
        ResizeBoard();
        bingoCount = 0;
    }

    protected override void ShowResult()
    {
        Debug.Log("showResult");
        ShowBingo(() =>
        {
            if (isGuide)
            {
                EndGuidnce();
                
            }
            else
            {
                if(CheckOver())
                    base.ShowResult();
                else
                {
                    bingo.gameObject.SetActive(false);
                    board.gameObject.SetActive(false);
                    alphaBoard.gameObject.SetActive(true);
                    _correctsTarget = null;

                    board = alphaBoard;
                    scoreBoard.GuideScore(board.size * 100);
                    ResizeBoard();
                }
            }
        });
    }
}
