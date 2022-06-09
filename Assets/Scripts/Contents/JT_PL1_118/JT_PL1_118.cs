using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_118 : SingleAnswerContents<Question118, WordsData.WordSources>
{
    public Text textQuestion;
    public Button buttonQuestion;
    public ImageButton[] buttonsAnswer;
    protected override int QuestionCount => 8;
    private int alphabetCount = 2;

    protected override eContents contents => eContents.JT_PL1_118;
    private void AddButtonListener(ImageButton button, WordsData.WordSources data)
    {
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            if(data == currentQuestion.correct)
            {
                button.button.interactable = false;
                audioPlayer.Play(data.clip, () =>
                 {
                     AddAnswer(data);
                     if (!CheckOver())
                         button.button.interactable = true;
                 });
            }
        });
    }
    protected override List<Question118> MakeQuestion()
    {
        var corrects = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .Take(alphabetCount)
            .SelectMany(x=>GameManager.Instance.GetResources(x).Words.OrderBy(x => Random.Range(0f, 100f)).Take(QuestionCount/alphabetCount))
            .ToArray();

        var incorrects = GameManager.Instance.GetResources().Words
            .Where(x => !corrects.Select(y=>y.value).Contains(x.value))
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();

        var list = new List<Question118>();
        for(int i = 0; i< corrects.Length; i++)
            list.Add(new Question118(corrects[i], new WordsData.WordSources[] { incorrects[i] }));
        return list;
    }

    protected override void ShowQuestion(Question118 question)
    {
        for(int i = 0;i < buttonsAnswer.Length; i++)
        {
            AddButtonListener(buttonsAnswer[i], question.totalQuestion[i]);
            buttonsAnswer[i].sprite = question.totalQuestion[i].sprite;
        }

        audioPlayer.Play(question.correct.act3);
        buttonQuestion.onClick.RemoveAllListeners();
        buttonQuestion.onClick.AddListener(() => audioPlayer.Play(question.correct.act3));

        var randomImage = question.totalQuestion
            .Select(x => x.sprite)
            .ToArray();

        textQuestion.text = question.correct.value;
    }
}
public class Question118 : SingleQuestion<WordsData.WordSources>
{
    public Question118(WordsData.WordSources correct, WordsData.WordSources[] questions) : base(correct, questions)
    {
        
    }
}

