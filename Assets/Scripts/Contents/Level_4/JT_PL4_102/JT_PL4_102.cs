using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL4_102 : MultiAnswerContents<AlphabetContentsSetting, Question4_102, DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL4_102;
    protected override int QuestionCount => 1;
    private int answerCount = 6;
    protected override bool CheckOver() => answerCount == currentCnt;
    private DigraphsWordsData[] current;
    private int currentCnt;

    public EventSystem eventSystem;
    public Image successEffect;
    public Text successText;
    public Image successImage;
    public Sprite successedImage;
    public Sprite defaultImage;
    public FishingElement charactor;
    //public ImageButton[] buttons;
    public BubbileButtons[] buttons;
    public Image[] childrenImages;


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        var target = buttons.Where(x => x.button.interactable).OrderBy(x => Random.Range(0, 100)).First();

        guideFinger.DoMove(target.image.transform.position, () =>
        {
            guideFinger.DoClick(() =>
            {
                guideFinger.gameObject.SetActive(false);
                charactor.ThrowBobber(target.gameObject, () => ClickMotion(target));
                //ClickMotion(target);
            });
        });
        while (!isNext) yield return null;
        isNext = false;
    }

    protected override List<Question4_102> MakeQuestion()
    {
        var questions = new List<Question4_102>();


        for( int i = 0; i < QuestionCount; i++)
        {
            current = GameManager.Instance.digrpahs
                .SelectMany(x => GameManager.Instance.GetDigraphs(x))
                .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(answerCount)
                .ToArray();

            questions.Add(new Question4_102(current, new DigraphsWordsData[] { }));
        }
        
        return questions;
    }

    protected override void ShowQuestion(Question4_102 question)
    { 
        ResetElement();
        eventSystem.enabled = true;

        Debug.Log(question.totalQuestion.Length);
        currentCnt = 0;
        for(int i = 0; i < buttons.Length; i ++)
        {
            var data = question.totalQuestion[i];

            buttons[i].GetComponent<Image>().sprite = defaultImage;
            buttons[i].Init(data);
            AddListener(buttons[i]);
        }
    }

    private void AddListener(BubbileButtons imageButton)
    { 
        var button = imageButton.button;
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            button.interactable = false;
            charactor.ThrowBobber(imageButton.gameObject, () => ClickMotion(imageButton));
        });
    }

    private void ClickMotion(BubbileButtons imageButton)
    {
        eventSystem.enabled = false;
        currentCnt++;
        Debug.Log(currentCnt);
        var button = imageButton.button;
        var data = imageButton.data;

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);

        successImage.sprite = data.sprite;
        successImage.preserveAspect = true;
        successText.text = data.key.Replace(data.IncludedDigraphs,
                "<color=\"red\">" + data.IncludedDigraphs + "</color>");

        successEffect.gameObject.SetActive(true);
        audioPlayer.Play(data.act, () =>
        {
            successEffect.gameObject.SetActive(false);
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].gameObject.SetActive(true);
            AddAnswer(currentQuestion.currentCorrect);
            isNext = true;
            eventSystem.enabled = true;
        });

        button.image.sprite = successedImage;
    }

    protected override void EndGuidnce()
    {
        charactor.Stop();
        base.EndGuidnce();

        ResetElement();
    }

    private void ResetElement()
    {
        foreach(var item in buttons)
        {
            item.transform.localScale = new Vector3(1f, 1f, 1f);
            item.rt.offsetMin = new Vector2(0f, 0f);
            item.rt.offsetMax = new Vector2(0f, 0f);
        }
    }
}

public class Question4_102 : MultiQuestion<DigraphsWordsData>
{
    public int currentIndex { get; private set; } = 0;
    public DigraphsWordsData currentCorrect => correct[currentIndex];

    public Question4_102(DigraphsWordsData[] correct, DigraphsWordsData[] questions) : base(correct, questions)
    {
    }

    public void ResetCurrentIndex() { currentIndex = 0; }
    protected override bool CheckCorrect(DigraphsWordsData answer) => true;
    public override void SetAnswer(DigraphsWordsData answer)
    {
        base.SetAnswer(answer);
        currentIndex += 1;
    }
}