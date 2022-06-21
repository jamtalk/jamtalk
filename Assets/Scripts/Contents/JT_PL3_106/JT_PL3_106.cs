using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_106 : SingleAnswerContents<Question2_106, WordsData.WordSources>
{
    protected override eContents contents => eContents.JT_PL2_106;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 3;
    private int index = 0;
    private WordsData.WordSources[] words;

    public Thrower306 thrower;
    public Text[] texts;
    public Text currentText;
    public Button currentButton;
    public Image currentImage;
    public Image bagImage;
    [SerializeField]
    private List<BubbleElement> elements = new List<BubbleElement>();

    protected override void Awake()
    {
        base.Awake();

        currentButton.onClick.AddListener(() => audioPlayer.Play(currentQuestion.correct.clip));
        currentText.text = words[index].value;
        currentImage.sprite = words[index].sprite;
        currentImage.name = words[index].value;
        currentImage.preserveAspect = true;
    }

    private void SetCurrentImage()
    {
        index = index > 2 ? index = 2 : index ;

        currentText.text = words[index].value;
        currentImage.sprite = words[index].sprite;
        currentImage.name = words[index].value;
        currentImage.preserveAspect = true;
        currentImage.gameObject.SetActive(true);

        for (int i = 0; i < elements.Count; i++)
            elements[i].gameObject.SetActive(true);

        thrower.gameObject.SetActive(false);
        bagImage.gameObject.SetActive(false);
    }

    private void SetBagImage()
    {
        bagImage.sprite = currentImage.sprite;
        bagImage.preserveAspect = true;
        bagImage.gameObject.SetActive(true);
    }

    protected override List<Question2_106> MakeQuestion()
    {
        var questions = new List<Question2_106>();
        words = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f)).ToArray()
            .Take(QuestionCount)
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(words.Length - 1)
                .ToArray();
            questions.Add(new Question2_106(words[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question2_106 question)
    {
        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            var data = question.totalQuestion[i];
            elements[i].Init(question.totalQuestion[i]);    // init - text 음가 제외하고 설정해야 함
            elements[i].isOn = false;                       // script 변경 해야 할 수 있음 
            AddDoubleClickListener(elements[i], data);
        }
    }

    protected virtual void AddDoubleClickListener(BubbleElement element, WordsData.WordSources data)
    {
        element.onClickFirst.RemoveAllListeners();
        element.onClick.RemoveAllListeners();

        element.onClickFirst.AddListener(() =>
        {
            //audioPlayer.Play(element.); 해당 음가 호출
            audioPlayer.Play(currentQuestion.correct.clip);
        });

        element.onClick.AddListener(() =>
        {   // text 음가 채워저야 ㅇ
            if (currentQuestion.correct.value.Contains(currentImage.name))
            {
                AddAnswer(data);

                index += 1;

                for (int i = 0; i < elements.Count; i++)
                    elements[i].gameObject.SetActive(false);
                currentImage.gameObject.SetActive(false);

                audioPlayer.Play(words[index - 1].act3, () => SetCurrentImage());
                thrower.Throw(currentImage, bagImage.GetComponent<RectTransform>(), () => SetBagImage());

                thrower.GetComponent<Image>().sprite = currentImage.sprite;
                thrower.gameObject.SetActive(true);
            }
            else
            {

            }
        });
    }
}

public class Question2_106 : SingleQuestion<WordsData.WordSources>
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
    public Question2_106(WordsData.WordSources correct, WordsData.WordSources[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}
