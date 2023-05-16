using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookConversationContents : STTContents<DigraphsWordsData, Text>
{
    protected override eContents contents => eContents.Book_Conversation;
    protected override bool isGuidence => false;
    protected override int QuestionCount => 3;
    protected override bool PlayClipOnShow => false;
    public Listening_BtnCtr btnCtr;
    public Button buttonPlay;
    public Button buttonReplay;

    protected override void Awake()
    {
        base.Awake();

        //btnCtr.action += PlaySentance;
        buttonPlay.onClick.AddListener(() => PlaySentance());
        buttonReplay.onClick.AddListener(() => PlaySentance());
    }
    protected override List<Question_STT<DigraphsWordsData>> MakeQuestion()
    {
        return GameManager.Instance.GetDigraphs()
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .Select(x => new Question_STT<DigraphsWordsData>(x))
            .ToList();
    }

    protected override void ShowValue(Question_STT<DigraphsWordsData> question)
    {
        valueViewer.text = question.correct.key;
    }

    private void PlaySentance(int value = 0)
    {
        audioPlayer.Play(questions[currentQuestionIndex].correct.clip);
        //currentQuestionIndex++;
    }
}
