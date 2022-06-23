using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL3_106 : SingleAnswerContents<Question3_106, VowelSource>
{
    protected override eContents contents => eContents.JT_PL2_106;
    protected override bool CheckOver() => currentQuestionIndex == questions.Count - 1;
    protected override int GetTotalScore() => QuestionCount;
    protected override int QuestionCount => 3;
    private int index = 0;
    //private WordSource[] words;
    private VowelSource[] vowels;

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

        //currentButton.onClick.AddListener(() => audioPlayer.Play(currentQuestion.correct.clip));
        currentButton.onClick.AddListener(() => currentQuestion.correct.PlayClip());
        currentText.text = vowels[index].value;
        currentImage.sprite = vowels[index].sprite;
        currentImage.name = vowels[index].value;
        currentImage.preserveAspect = true;
    }

    private void SetCurrentImage()
    {
        index = index > 2 ? index = 2 : index ;

        currentText.text = vowels[index].value;
        currentImage.sprite = vowels[index].sprite;
        currentImage.name = vowels[index].value;
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

    protected override List<Question3_106> MakeQuestion()
    {
        var questions = new List<Question3_106>();
        vowels = GameManager.Instance.vowels
            .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
            .Where(x => x.alphabet == GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        for (int i = 0; i < QuestionCount; i++)
        {
            var tmp = GameManager.Instance.alphabets
                .Where(x => x != GameManager.Instance.currentAlphabet)
                .SelectMany(x => GameManager.Instance.GetResources(x).Vowels)
                .OrderBy(x => Random.Range(0f, 100f)).ToArray()
                .Take(elements.Count - 1)
                .ToArray();
            questions.Add(new Question3_106(vowels[i], tmp));
        }
        return questions;
    }

    protected override void ShowQuestion(Question3_106 question)
    {
        for (int i = 0; i < question.totalQuestion.Length; i++)
        {
            var data = question.totalQuestion[i];
            elements[i].Init(question.totalQuestion[i]);    // init - text 음가 제외하고 설정해야 함
            elements[i].isOn = false;                       // script 변경 해야 할 수 있음 
            AddDoubleClickListener(elements[i], data);
        }
    }

    protected virtual void AddDoubleClickListener(BubbleElement element, VowelSource data)
    {
        element.onClickFirst.RemoveAllListeners();
        element.onClick.RemoveAllListeners();

        element.onClickFirst.AddListener(() =>
        {
            //audioPlayer.Play(element.); 해당 음가 호출
            currentQuestion.correct.PlayClip();
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

                //audioPlayer.Play(vowels[index - 1], () => SetCurrentImage()); 
                currentQuestion.correct.PlayClip(); // current audio play 후
                SetCurrentImage();                  // 호출되어야 함
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

public class Question3_106 : SingleQuestion<VowelSource>
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
    public Question3_106(VowelSource correct, VowelSource[] questions) : base(correct, questions)
    {
        spriteCorrect = correct.sprite;
        spriteQuestions = questions.Select(x => x.sprite).ToArray();
    }
}
