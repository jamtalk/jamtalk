using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_118 : SingleAnswerContents<Question118, AlphabetWordsData>
{
    public Text textQuestion;
    public Button buttonQuestion;
    public ImageButton[] buttonsAnswer;
    protected override int QuestionCount => 8;
    private int alphabetCount = 2;

    protected override eContents contents => eContents.JT_PL1_118;

    bool isPlayed = false;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for( int i = 0; i < QuestionCount; i ++)
        {
            while (!isPlayed) yield return null;
            isPlayed = false;

            var isNext = false;
            var target = buttonsAnswer.Where(x => x.name == questions[i].correct.key).First();
            guideFinger.gameObject.SetActive(true);
            Debug.Log(questions[i].correct.key);
            guideFinger.DoMoveCorrect(target.transform.position, () =>
            {
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    guideFinger.transform.position = Vector3.zero;
                    ButtonClickMotion(target, questions[i].correct);
                    isNext = true;
                });
            });

            while (!isNext) yield return null;
            isNext = false;
        }
    }

    private void ButtonClickMotion(ImageButton button, AlphabetWordsData data)
    {
        button.button.interactable = false;
        audioPlayer.Play(data.clip, () =>
        {
            AddAnswer(data);
            if (!CheckOver())
                button.button.interactable = true;
        });
    }

    private void AddButtonListener(ImageButton button, AlphabetWordsData data)
    {
        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            if(data == currentQuestion.correct)
            {
                ButtonClickMotion(button, data);
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
            list.Add(new Question118(corrects[i], new AlphabetWordsData[] { incorrects[i] }));
        return list.OrderBy(x=>Random.Range(0f,100f)).ToList();
    }

    protected override void ShowQuestion(Question118 question)
    {
        for (int i = 0; i < buttonsAnswer.Length; i++)
        {
            AddButtonListener(buttonsAnswer[i], question.totalQuestion[i]);
            buttonsAnswer[i].sprite = question.totalQuestion[i].sprite;
            buttonsAnswer[i].name = question.totalQuestion[i].key;
        }

        audioPlayer.Play(question.correct.act, () => isPlayed = true);
        buttonQuestion.onClick.RemoveAllListeners();
        buttonQuestion.onClick.AddListener(() => audioPlayer.Play(question.correct.act));

        var randomImage = question.totalQuestion
            .Select(x => x.sprite)
            .ToArray();

        textQuestion.text = question.correct.key;
    }
}
public class Question118 : SingleQuestion<AlphabetWordsData>
{
    public Question118(AlphabetWordsData correct, AlphabetWordsData[] questions) : base(correct, questions)
    {
        
    }
}

