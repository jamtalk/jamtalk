using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class JT_PL1_117 : BaseContents
{
    public BingoBoard board;
    public BingoScoreBoard scoreBoard;
    public int BingoCount=>1;
    public RectTransform bingo;
    public AudioSinglePlayer audioPlayer;
    //문제
    public int currentIndex = 0;
    public eAlphabet[] questions;
    public eAlphabet currentQuestion => questions[currentIndex];
    protected override eContents contents => eContents.JT_PL1_117;
    protected override bool CheckOver() => board.GetBingoCount() >= BingoCount;
    protected override int GetTotalScore() => BingoCount;

    private eAlphabet[] correctsTarget => new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
    protected override void Awake()
    {
        base.Awake();
        board.onClick += OnClick;
        scoreBoard.onFailed += ShowResult;

        questions = GetQuestion();
        
        currentIndex = 0;
        PlaySound();
        board.Init(questions
            .OrderBy(x=>Random.Range(0f,100f)).ToArray());
    }
    public eAlphabet[] GetQuestion()
    {
        var questions = GameManager.Instance.alphabets
            .Where(x => !correctsTarget.Contains(x))
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        //정답지 뽑기
        var corrects = new List<eAlphabet>();
        for(int i = 0;i < board.size; i++)
            corrects.Add(correctsTarget.OrderBy(x => Random.Range(0f, 100f)).First());

        //정답 인덱스 뽑기
        var startPos = Random.Range(0, board.size);
        var correctpos = new List<int>();
        if (startPos == 0 || startPos == board.size-1)
        {
            var type = Random.Range(0, 3);
            switch (type)
            {
                case 0: //대각선
                    var _startPos = startPos;
                    for(int i= 0;i < board.size; i++)
                    {
                        correctpos.Add(board.size * i + _startPos);

                        if (startPos == 0) 
                            _startPos += 1;     //첫번째 대각선
                        else
                            _startPos -= 1;     //마지막 대각선
                    }
                    break;
                case 1: //세로
                    for (int i = 0; i < board.size; i++)
                        correctpos.Add(board.size * i + startPos);
                    break;
                case 2: //가로
                    for (int i = 0; i < board.size; i++)
                        correctpos.Add(i);
                    break;
            }
        }
        else
        {
            for(int i = 0;i < board.size; i++)
                correctpos.Add(board.size * i + startPos);
        }


        for (int i = 0; i < board.size; i++)
            questions[correctpos[i]] = corrects[i];

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
    public void OnClick(eAlphabet value)
    {
        if (value == currentQuestion)
        {
            currentIndex += 1;
            scoreBoard.AddScore(100);
        }
        else
            scoreBoard.IncreaseIncorrect();

        PlaySound();

    }
    public void PlaySound()
    {
        if (CheckOver())
            ShowResult();
        else
            audioPlayer.Play(GameManager.Instance.GetClipAlphbet(currentQuestion));
    }
}
