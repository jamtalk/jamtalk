using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_118 : SingleAnswerContents<Question118, string>
{
    public Text textQuestion;
    public Button buttonQuestion;
    public ImageButton[] buttonsAnswer;
    protected override int QuestionCount => 8;
    private int alphabetCount = 2;

    protected override eContents contents => eContents.JT_PL1_118;
    protected override void Awake()
    {
        base.Awake();
        for(int i = 0;i < buttonsAnswer.Length; i++)
            AddButtonListener(buttonsAnswer[i]);
    }
    private void AddButtonListener(ImageButton button)
    {
        button.button.onClick.AddListener(() =>
        {
            var value = button.image.sprite.name;
            if(value == currentQuestion.correct)
            {
                button.button.interactable = false;
                audioPlayer.Play(GameManager.Instance.GetClipWord(value), () =>
                 {
                     AddAnswer(value);
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
            .SelectMany(x=>GameManager.Instance.GetWords(x).OrderBy(x => Random.Range(0f, 100f)).Take(QuestionCount/alphabetCount))
            .ToArray();

        var incorrects = GameManager.Instance.GetWords()
            .Where(x => !corrects.Contains(x))
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(QuestionCount)
            .ToArray();

        var list = new List<Question118>();
        for(int i = 0; i< corrects.Length; i++)
            list.Add(new Question118(corrects[i], new string[] { incorrects[i] }));
        return list;
    }

    protected override void ShowQuestion(Question118 question)
    {
        audioPlayer.Play(GameManager.Instance.GetClipAct3(question.correct));
        buttonQuestion.onClick.RemoveAllListeners();
        buttonQuestion.onClick.AddListener(() => audioPlayer.Play(GameManager.Instance.GetClipAct3(question.correct)));

        var randomImage = question.RandomQuestions
            .Select(x => GameManager.Instance.GetSpriteWord(x))
            .ToArray();

        for (int i = 0;i < buttonsAnswer.Length; i++)
            buttonsAnswer[i].SetSprite(randomImage[i]);

        textQuestion.text = question.correct;
    }
}
public class Question118 : SingleQuestion<string>
{
    public Question118(string correct, string[] questions) : base(correct, questions)
    {
        
    }
}

