using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public class JT_PL1_106 : SingleAnswerContents<Question106, AlphabetWordsData>
{
    protected override eContents contents => eContents.JT_PL1_106;

    protected override int QuestionCount => 4;

    public DoubleClickButton[] buttonQuestions;
    private List<AlphabetWordsData> buttonDatas = new List<AlphabetWordsData>();
    public ImageButton buttonPhanics;
    public Sprite spritePop;
    public AudioClip clipPop;

    
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return new WaitForEndOfFrame();


        guideFinger.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);


        //for (int j = 0; j < QuestionCount; j++)
        //{
            var correctIndex = 0;
            isNext = false;
            for (int i = 0; i < buttonDatas.Count(); i++)
            {
                Debug.Log(buttonDatas[i].key);
                if (buttonDatas[i].key == currentQuestion.correct.key)
                {
                    correctIndex = i;
                    break;
                }
            }
            guideFinger.gameObject.SetActive(true);

            guideFinger.DoMove(buttonQuestions[correctIndex].transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    buttonQuestions[correctIndex].isOn = true;
                    audioPlayer.Play(buttonDatas[correctIndex].clip);

                    guideFinger.DoClick(() =>
                    {
                        CorrectClickMotion(buttonQuestions[correctIndex], buttonDatas[correctIndex]);

                        guideFinger.gameObject.SetActive(false);;
                    });
                });
            });

            while (!isNext) yield return null;
        //}
    }

    protected override void Awake()
    {
        GameManager.Instance.currentAlphabet = eAlphabet.E;

        base.Awake();
    }
    protected override List<Question106> MakeQuestion()
    {
        var questions = new List<Question106>();
        var targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        var correctWord = targets
            .SelectMany(x=> 
                GameManager.Instance.GetResources(x).Words
                .OrderBy(y=>Random.Range(0f,100f))
                .Take(QuestionCount/2))
            .OrderBy(x => Random.Range(0f,100f))
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x=>x!=GameManager.Instance.currentAlphabet)
                .Where(x => x != GameManager.Instance.currentAlphabet+1)
                .SelectMany(x=>GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(4)
                .ToArray();
            questions.Add(new Question106(correctWord[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question106 question)
    {
        buttonDatas.Clear();
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            var data = question.totalQuestion[i];
            buttonQuestions[i].isOn = false;
            buttonQuestions[i].sprite = data.sprite;
            buttonDatas.Add(data);
            AddDoubleClickListener(buttonQuestions[i],data);
        }
        var phanics = currentQuestion.correct.audio.phanics;
        audioPlayer.Play(phanics);
        buttonPhanics.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown,eAlphabetType.Upper,currentQuestion.correct.Key);
        buttonPhanics.button.onClick.RemoveAllListeners();
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(phanics));
    }
    private void ResetQuestion()
    {
        for (int i = 0; i < buttonQuestions.Length; i++)
        {
            buttonQuestions[i].isOn = false;
        }
        var phanics = currentQuestion.correct.audio.phanics;
        audioPlayer.Play(phanics);
        buttonPhanics.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Brown, eAlphabetType.Upper, currentQuestion.correct.Key);
        buttonPhanics.button.onClick.RemoveAllListeners();
        buttonPhanics.button.onClick.AddListener(() => audioPlayer.Play(phanics));
    }
    private void AddDoubleClickListener(DoubleClickButton button, AlphabetWordsData data)
    {
        button.onClickFirst.RemoveAllListeners();
        button.onClick.RemoveAllListeners();

        button.onClickFirst.AddListener(() =>
        {
            for (int i = 0; i < buttonQuestions.Length; i++)
            {
                buttonQuestions[i].isOn = buttonQuestions[i] == button;
                audioPlayer.Play(data.clip);
            }
        });

        button.onClick.AddListener(() =>
        {
            if(currentQuestion.correct == data)
            {
                CorrectClickMotion(button, data);
            }
            else
            {
                ResetQuestion();
            }
        });
    }
    private void CorrectClickMotion(DoubleClickButton button, AlphabetWordsData data)
    {
        button.button.targetGraphic.gameObject.GetComponent<Image>().sprite = spritePop;
        button.image.gameObject.SetActive(false);
        audioPlayer.Play(1f, clipPop);
        var tween = button.GetComponent<RectTransform>().DOScale(1.5f, .25f);
        tween.SetLoops(2, LoopType.Yoyo);
        tween.SetEase(Ease.Linear);
        tween.onComplete += () =>
        {
            isNext = true;
            AddAnswer(data);
            if (!CheckOver())
                button.image.gameObject.SetActive(true);
        };
        tween.Play();
    }
    protected override void ShowResult()
    {
        var value = GameManager.Instance.GetResources().AudioData.act2;
        var nextAlphabet = (GameManager.Instance.currentAlphabet) +1;
        var nextAudio = GameManager.Instance.GetResources(nextAlphabet).AudioData.act2;

        audioPlayer.Play(value, () =>
        {
            audioPlayer.Play(nextAudio, base.ShowResult);
        });
    }
}
[Serializable]
public class Question106 : SingleQuestion<AlphabetWordsData>
{
    private Sprite spriteCorrect;
    private Sprite[] spriteQuestions;
    public Sprite[] SpriteQuestions
    {
        get
        {
            return spriteQuestions.Union(new Sprite[] { spriteCorrect })
                .OrderBy(x => Random.Range(0f, 100f))
                .ToArray();
        }
    }
    public Question106(AlphabetWordsData correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x=>x.sprite).ToArray();
    }
}
