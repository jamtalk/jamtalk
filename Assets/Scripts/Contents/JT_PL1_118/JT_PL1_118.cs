using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_118 : SingleAnswerContents<Question118, WordSource>
{
    public Text textQuestion;
    public Button buttonQuestion;
    public ImageButton[] buttonsAnswer;
    protected override int QuestionCount => 8;
    private int alphabetCount = 2;

    protected override eContents contents => eContents.JT_PL1_118;
    private void AddButtonListener(ImageButton button, WordSource data)
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
        var targetAlphabets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        //var targetAlphabets = GameManager.Instance.alphabets
        //    .Where(x => x >= GameManager.Instance.currentAlphabet)
        //    .Take(alphabetCount);

        var corrects = targetAlphabets
            .Select(x => GameManager.Instance.GetResources(x))
            .SelectMany(x => x.Words.OrderBy(y => Random.Range(0f, 100f)).Take(QuestionCount / alphabetCount))
            .ToArray();

        var incorrects = GameManager.Instance.alphabets
            .Where(x => !targetAlphabets.Contains(x))
            .SelectMany(x => GameManager.Instance.GetResources(x).Words)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();

        Debug.LogFormat("{0} / {1}", corrects.Length, incorrects.Length);
        var list = new List<Question118>();
        for(int i = 0; i< corrects.Length; i++)
            list.Add(new Question118(corrects[i], new WordSource[] { incorrects[i] }));
        return list.OrderBy(x=>Random.Range(0f,100f)).ToList();
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
public class Question118 : SingleQuestion<WordSource>
{
    public Question118(WordSource correct, WordSource[] questions) : base(correct, questions)
    {
        
    }
}

