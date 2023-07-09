using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Book_Words_Quiz : SingleAnswerContents<BookContentsSetting, BookWordQuizeQuestion, BookWordData>, IBookContentsRunner
{
    public override eSceneName NextScene => eSceneName.AC_004;
    public EventSystem eventSystem;
    public Image quizImage;
    public Button[] buttons;
    public Button buttonQuestionAudio;
    protected override bool includeExitButton => false;
    protected override bool showPopupOnEnd => false;
    protected override bool showQuestionOnAwake => false;
    protected override int QuestionCount => 5;
    protected override bool isGuidence => false;

    protected override eContents contents => eContents.Book_Words;
    protected override void OnAwake()
    {
        isGuide = false;
        base.OnAwake();
        buttonQuestionAudio.onClick.AddListener(() =>
        {
            audioPlayer.Play(currentQuestion.correct.value);
        });
    }

    protected override List<BookWordQuizeQuestion> MakeQuestion()
    {
        //현재 진행중이지 않은 책 종류 선별
        var targetBook = Enum.GetNames(typeof(eBookType))
            .Select(x => (eBookType)Enum.Parse(typeof(eBookType), x))
            .Where(x => x != GameManager.Instance.currentBook)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        //진행중이지 않은 책의 랜덤한 3권을 선택하여 책 단어 데이터 받아옴
        var incorrectsBooks = GameManager.Instance.GetBookNumbers(targetBook)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(3)
            .SelectMany(x => GameManager.Instance.GetBookMetaDatas(targetBook, x))
            .SelectMany(x => x.words)
            .Distinct();

        return GameManager.Instance.GetCurrentBookWords()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x =>
            {
                var incorrects = incorrectsBooks
                    .Where(y=>y.value != x.value)
                    .OrderBy(x => Random.Range(0f, 100f))
                    .Take(buttons.Length-1)
                    .ToArray();
                return new BookWordQuizeQuestion(x, incorrects);
            }).ToList();
    }

    protected override void ShowQuestion(BookWordQuizeQuestion question)
    {
        quizImage.sprite = question.correct.sprite;
        quizImage.preserveAspect = true;
        buttons = buttons.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        for(int i = 0;i < question.totalQuestion.Length; i++)
        {
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = question.totalQuestion[i].value;
            AddListener(buttons[i], question.totalQuestion[i]);
        }
    }
    private void AddListener(Button button, BookWordData data)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            eventSystem.enabled = false;
            AndroidPluginManager.Instance.PlayTTS(data.value, () =>
            {
                if (data == currentQuestion.correct)
                    AddAnswer(data);
                eventSystem.enabled = true;
            });
        });
    }
}
public class BookWordQuizeQuestion : SingleQuestion<BookWordData>
{
    public BookWordQuizeQuestion(BookWordData correct, BookWordData[] questions) : base(correct, questions)
    {
    }
}
