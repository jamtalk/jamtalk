using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.EventSystems;

public class JT_PL2_106 : MultiAnswerContents<Question2_106, WordsData.WordSources>
{
    public EventSystem eventSystem;
    protected override eContents contents => eContents.JT_PL2_106;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 5;
    protected virtual int WordsCount => 6;

    public Button longButton;
    public Button shortButton;
    public Button spinButton;
    public Image[] images;
    public Sprite succesImage;
    public Image rouletteImage;
    public GameObject parent;
    public Text rouletteText;

    public AudioClip rouletteClip;
    public AudioClip tabClip;
    public AudioClip currentClip;

    private Sequence seq;
    private List<Text> textList = new List<Text>();
    private int currentIndex;
    private List<WordsData.WordSources> datas = new List<WordsData.WordSources>();
    private Question2_106 Question2;

    protected override void Awake()
    {
        base.Awake();

        spinButton.onClick.AddListener(() => Spin());

    }
    private void AddOnClickButtonListener(WordsData.WordSources data)
    {
        shortButton.onClick.AddListener(() =>
        {
            audioPlayer.Play(tabClip);

            if (currentQuestion.currentCorrect == datas[currentIndex])
            {
                images[currentQuestionIndex].sprite = succesImage;
                AddAnswer(data);
            }
        });
        longButton.onClick.AddListener(() =>
        {
            audioPlayer.Play(tabClip);
        });
    }

    private void Spin()
    {   // 시작 시 eventSystemp false > onCompleted true
        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }
        audioPlayer.Play(6f, rouletteClip);
        //eventSystem.enabled = false;

        seq = DOTween.Sequence();
        currentIndex = Random.Range(0, textList.Count);

        var targetAngle = 360 - textList[currentIndex].transform.localRotation.eulerAngles.z;
        var firstTween = rouletteImage.transform.DORotate(new Vector3(0, 0, 360f), 2f, RotateMode.FastBeyond360);
        var secondTween = rouletteImage.transform.DORotate(new Vector3(0, 0, 360f), 2f, RotateMode.FastBeyond360);
        var lastTween = rouletteImage.transform.DORotate(new Vector3(0, 0, targetAngle), 2f,RotateMode.FastBeyond360);

        Debug.LogFormat("text : {0}, rect : {1}" ,textList[currentIndex], targetAngle);
        firstTween.SetEase(Ease.InQuad);
        secondTween.SetEase(Ease.Linear);
        lastTween.SetEase(Ease.OutQuad);
        seq.Append(firstTween);
        seq.Append(secondTween);
        seq.Append(lastTween);

        //seq.onComplete += () => eventSystem.enabled = true;
        seq.Play();
    }

    protected override void ShowQuestion(Question2_106 question)
    {
        rouletteText.gameObject.SetActive(true);

        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            var angle = 15f + (float)(360 / question.totalQuestion.Length) * i;

            var text = Instantiate(rouletteText.gameObject, parent.transform).GetComponent<Text>();
            text.transform.localRotation = Quaternion.Euler(0, 0, angle);
            text.text = question.totalQuestion[i].value;
            text.name = question.totalQuestion[i].value;

            textList.Add(text);
            datas.Add(question.totalQuestion[i]);
            AddOnClickButtonListener(question.totalQuestion[i]);
        }
        rouletteText.gameObject.SetActive(false);
    }

    protected override List<Question2_106> MakeQuestion()
    {
        var list = new List<Question2_106>();
        for(int i = 0; i < QuestionCount; i++)
        {
            var shortWords = GameManager.Instance.GetResources().Words
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(WordsCount)
                .ToArray();

            var longWords = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(WordsCount)
                .ToArray();
            list.Add(new Question2_106(shortWords, longWords));
        }
        return list;
    }
}



public class Question2_106 : MultiQuestion<WordsData.WordSources>
{
    public int currentIndex;
    public Question2_106(WordsData.WordSources[] correct, WordsData.WordSources[] questions) : base(correct, questions)
    {
        currentIndex = 0;
    }
    public WordsData.WordSources currentCorrect => correct[currentIndex];

    protected override bool CheckCorrect(WordsData.WordSources answer)
    {
        return correct[currentIndex] == answer;
    }

    public override void SetAnswer(WordsData.WordSources answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }

}
