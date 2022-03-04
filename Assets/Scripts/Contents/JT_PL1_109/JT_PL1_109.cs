using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_109 : BaseContents
{
    public ImageButton[] buttonsQuestion;
    public AlphabetDragToggle109[] toggles;
    public AudioSinglePlayer audioPlayer;
    public AudioSinglePlayer alphabetPlayer;
    private Question109[] questions;
    private int currentIndex = 0;
    public Image[] correct;
    private Question109 currentQuestion => questions[currentIndex];
    private ImageButton currentButton => buttonsQuestion[currentIndex];

    protected override eContents contents => eContents.JT_PL1_109;

    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);
    private void Awake()
    {
        var words = GameManager.Instance.GetWords(GameManager.Instance.currentAlphabet)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(buttonsQuestion.Length)
            .ToArray();

        questions = words.Select(x => new Question109(x)).ToArray();

        for(int i = 0;i < buttonsQuestion.Length; i++)
        {
            var word = questions[i].word;
            buttonsQuestion[i].name = word;
            buttonsQuestion[i].SetSprite(GameManager.Instance.GetSpriteWord(word));
            AddQuestionButtonListener(buttonsQuestion[i], i);
        }

        for (int i = 0; i < toggles.Length; i++)
            toggles[i].onEndDrag += OnEndDrag;

        ShowQuestion();
    }
    private void ShowQuestion()
    {
        var word = currentQuestion.word;
        for (int i = 0; i < toggles.Length; i++)
            toggles[i].Init(currentQuestion.alphabets[i], i);
        alphabetPlayer.Play(GameManager.Instance.GetClipWord(currentQuestion.word));
        for(int i = 0;i < correct.Length; i++)
        {
            Debug.LogFormat("{0}/{1}", i, word.Length);
            if (word.Length > i)
            {
                var type = eAlphbetType.Lower;
                var eAlphabet = (eAlphabet)System.Enum.Parse(typeof(eAlphabet), word[i].ToString().ToUpper());
                correct[i].gameObject.SetActive(true);
                correct[i].sprite = GameManager.Instance.GetAlphbetSprite(eAlphbetStyle.NeonYellow, type, eAlphabet);
                correct[i].preserveAspect = true;
            }
            else
            {
                correct[i].gameObject.SetActive(false);
            }
        }
    }
    private void AddQuestionButtonListener(ImageButton button, int index)
    {
        button.button.onClick.AddListener(() =>
        {
            currentIndex = index;
            ShowQuestion();
        });
    }
    private void OnEndDrag()
    {
        var selected = toggles
            .Where(x => x.isOn)
            .OrderBy(x => x.index)
            .Select(x => x.value.ToString())
            .ToArray();
        
        var word = currentQuestion.word
            .Select(x => x.ToString().ToUpper())
            .ToArray();

        if (selected.Length == word.Length)
        {
            var result = true;
            for (int i = 0; i < selected.Length; i++)
            {
                if (selected[i] != word[i])
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                currentButton.button.interactable = false;
                currentQuestion.isCompleted = true;
                if (CheckOver())
                {
                    ShowResult();
                }
                else
                {
                    currentIndex = questions
                        .Where(x => !x.isCompleted)
                        .Select(x => questions.ToList().IndexOf(x))
                        .OrderBy(x => x)
                        .First();
                    audioPlayer.Play(1f,GameManager.Instance.GetClipCorrectEffect());
                    ShowQuestion();
                }
                return;
            }
        }
        ResetToggles();
    }
    private void ResetToggles()
    {
        for (int i = 0; i < toggles.Length; i++)
            toggles[i].isOn = false;
    }
}
public class Question109
{
    private int width=>7;
    private int height=>5;


    public eAlphabet[] alphabets;
    public string word;
    public bool isCompleted;
    public Question109(string word)
    {
        this.word = word;
        isCompleted = false;
        var wordAlphabets = word
            .Select(x => (eAlphabet)System.Enum.Parse(typeof(eAlphabet), x.ToString().ToUpper()))
            .ToArray();

        var incorrects = GameManager.Instance.alphabets
            .Where(x => !wordAlphabets.Contains(x))
            .ToArray();
        Debug.LogFormat("´Ü¾î : {0}({1})\n{2}\n({3})", word, word.Length, string.Join(", ", incorrects), incorrects.Length);

        var alphabetList = new List<eAlphabet>();
        for(int i = 0;i < width*height; i++)
        {
            var alphabetIndex = 0;
            if (i > 0)
                alphabetIndex = i % incorrects.Length;

            alphabetList.Add(incorrects[alphabetIndex]);
            alphabetIndex += 1;
        }

        alphabets = alphabetList
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();

        var correctPosition = GetVaildPositions(word.Length)
            .OrderBy(x => Random.Range(0f, 100f))
            .First();

        for(int i = 0;i < wordAlphabets.Length; i++)
        {
            var pos = GetIndex(correctPosition);
            Debug.LogFormat("{0} : {1}", correctPosition, pos);
            alphabets[pos] = wordAlphabets[i];
            if (correctPosition.x + 1 < width)
                correctPosition.x += 1;
            else
                correctPosition.y += 1;
        }
    }
    private int GetIndex(Vector2 pos) => (int)pos.y * width + (int)pos.x;
    private int GetMax(int x, int y)
    {
        if (x < width && y < height)
            return width - x + height - y - 1;
        else
            return 0;
    }
    public Vector2[] GetVaildPositions(int length)
    {
        var vectors = new List<Vector2>();
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                vectors.Add(new Vector2(x, y));
        return vectors
            .Where(x => GetMax((int)x.x, (int)x.y)>=length)
            .ToArray();
    }
}