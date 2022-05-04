using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class JT_PL1_117 : BaseContents
{
    public BingoBoard board;
    public BingoScoreBoard scoreBoard;
    public Button buttonSound;
    public int BingoCount=>1;
    public RectTransform bingo;
    public AudioSinglePlayer audioPlayer;
    //문제
    public int currentIndex = 0;
    public eAlphabet[] questions;
    public eAlphabet[] corrects;
    public eAlphabet currentQuestion => corrects[currentIndex];
    protected override eContents contents => eContents.JT_PL1_117;
    protected override bool CheckOver() => board.GetBingoCount() >= BingoCount;
    protected override int GetTotalScore() => BingoCount;

    private eAlphabet[] correctsTarget => new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
    protected override void Awake()
    {
        base.Awake();
        buttonSound.onClick.AddListener(PlaySound);
        board.onClick += OnClick;
        scoreBoard.onFailed += ShowResult;

        questions = GetQuestion();
        
        currentIndex = 0;
        PlaySound();
        board.Init(questions.ToArray(),corrects);
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
        this.corrects = corrects.OrderBy(x=>Random.Range(0f,100f)).ToArray();
        //정답 인덱스 뽑기
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
                    case 0: //대각선
                        var _startPos = startPosX;
                        for (int i = 0; i < board.size; i++)
                        {
                            correctpos.Add(board.size * i + _startPos);

                            if (startPosX == 0)
                                _startPos += 1;     //첫번째 대각선
                            else
                                _startPos -= 1;     //마지막 대각선
                        }
                        break;
                    case 1: //세로
                        for (int i = 0; i < board.size; i++)
                            correctpos.Add(board.size * i + startPosX);
                        break;
                    case 2: //가로
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
                correctpos.Add(startPosY*board.size+i);
        }


        for (int i = 0; i < board.size; i++)
            questions[correctpos[i]] = corrects[i];

        var question = string.Empty;
        for(int i= 0;i < questions.Length; i++)
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
