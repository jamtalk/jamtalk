using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JT_PL1_109 : BaseContents<AlphabetContentsSetting>
{
    private int questionCount => 4;
    public AlphabetDragToggle109[] toggles;
    public AudioSinglePlayer alphabetPlayer;
    private Question109[] questions;
    private int currentIndex = 0;
    public Image[] correct;
    public ImageButton questionButton;
    private Question109 currentQuestion => questions[currentIndex];

    protected override eContents contents => eContents.JT_PL1_109;

    protected override bool CheckOver() => !questions.Select(x => x.isCompleted).Contains(false);
    protected override int GetTotalScore() => questions.Length;

    private List<eAlphabet> corretsAlphbet = new List<eAlphabet>();


    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        while (!isNext) yield return null;
        isNext = false;

        var targets = currentQuestion.word.key;
        var first = toggles.Where(x => x.value == corretsAlphbet[0]).First();
        var isPress = false;

        guideFinger.DoMove(first.transform.position, () =>
        {
            first.isOn = true;
            guideFinger.DoPress(() => isPress = true);
        });

        while (!isPress) yield return null;

        var index = 1;
        for (int j = 1; j < targets.Length; j++)
        {
            var target = toggles
                .Where(x => !x.isOn)
                .Where(x => x.value == corretsAlphbet[j]).First();
            guideFinger.DoMove(target.transform.position, () =>
            {
                index++;
                target.isOn = true;
            });

            while (!target.isOn) yield return null;
        }

        while (index != targets.Length) yield return null;

        guideFinger.gameObject.SetActive(false);
        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
        OnEndDrag();

    }

    protected override void OnAwake()
    {
        base.OnAwake();

        MakeQuestion();

        
        questionButton.button.onClick.AddListener(() => audioPlayer.Play(currentQuestion.word.clip));
    }
    private Question109[] MakeQuestion()
    {
        var targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };

        if (isGuide)
        {
            questions = targets
                .SelectMany(x =>
                    GameManager.Instance.GetResources(x).Words
                    .Where(x => x.key.Length < 4)
                    .OrderBy(y => Random.Range(0f, 100f))
                    .Take(questionCount / 2))
                .OrderBy(x => Random.Range(0f, 100f))
                .Select(x => new Question109(x))
                .ToArray();
        }
        else
        {
            questions = targets
                .SelectMany(x =>
                    GameManager.Instance.GetResources(x).Words
                    .OrderBy(y => Random.Range(0f, 100f))
                    .Take(questionCount / 2))
                .OrderBy(x => Random.Range(0f, 100f))
                .Select(x => new Question109(x))
                .ToArray();
        }

        for (int i = 0; i < toggles.Length; i++)
            toggles[i].onEndDrag += OnEndDrag;

        ShowQuestion();

        return questions;
    }
    private void ShowQuestion()
    {
        var word = currentQuestion.word;
        for (int i = 0; i < toggles.Length; i++)
            toggles[i].Init(currentQuestion.alphabets[i], i);
        alphabetPlayer.Play(currentQuestion.word.clip, () => isNext = true);
        corretsAlphbet.Clear();
        for(int i = 0;i < correct.Length; i++)
        {
            if (word.key.Length > i)
            {
                var type = i > 0 ? eAlphabetType.Lower : eAlphabetType.Upper;
                var eAlphabet = (eAlphabet)System.Enum.Parse(typeof(eAlphabet), word.key[i].ToString().ToUpper());
                correct[i].gameObject.SetActive(true);
                correct[i].sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, type, eAlphabet);
                correct[i].preserveAspect = true;
                corretsAlphbet.Add(eAlphabet);
            }
            else
            {
                correct[i].gameObject.SetActive(false);
            }
        }
        questionButton.image.sprite = word.sprite;
    }
    private void OnEndDrag()
    {
        var selected = toggles
            .Where(x => x.isOn)
            .OrderBy(x => x.index)
            .Select(x => x.value.ToString())
            .ToArray();
        
        var word = currentQuestion.word.key
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
                currentQuestion.isCompleted = true;
                if (CheckOver())
                {
                    ShowResult();
                }
                else
                {
                    if (isGuide)
                        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
                        {
                            EndGuidnce();
                        });
                    else
                    {
                        currentIndex = questions
                            .Where(x => !x.isCompleted)
                            .Select(x => questions.ToList().IndexOf(x))
                            .OrderBy(x => x)
                            .First();
                        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect(), () =>
                         {
                             ShowQuestion();
                         });
                    }
                }
                return;
            }
            else
                audioPlayer.PlayIncorrect();
        }
        else
            audioPlayer.PlayIncorrect();

        ResetToggles();
    }
    private void ResetToggles()
    {
        for (int i = 0; i < toggles.Length; i++)
            toggles[i].isOn = false;
    }

    protected override void EndGuidnce()
    {
        base.EndGuidnce();
        currentIndex = 0;
        guideFinger.gameObject.SetActive(false);
        questions = MakeQuestion();
        ShowQuestion();
    }
}
public class Question109
{
    private int width=>11;
    private int height=>5;
    public eAlphabet[] alphabets;
    public AlphabetWordsData word;
    public bool isCompleted;
    public Question109(AlphabetWordsData word)
    {
        this.word = word;
        isCompleted = false;
        var wordAlphabets = word.key
            .Select(x => (eAlphabet)System.Enum.Parse(typeof(eAlphabet), x.ToString().ToUpper()))
            .ToArray();

        var incorrects = GameManager.Instance.alphabets
            .Where(x => !wordAlphabets.Contains(x))
            .ToArray();

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
        var vaildPos = new List<int[]>();
        //???? ?????? ????
        if (word.key.Length < height)
        {
            for(int i = 0;i < width; i++)
            {
                for(int j= 0; j < height-word.key.Length; j++)
                {
                    var tmp = new List<int>();
                    for(int k = 0; k < word.key.Length; k++)
                    {
                        tmp.Add(GetIndex(new Vector2(i, j + k)));
                    }
                    vaildPos.Add(tmp.ToArray());
                }
            }
        }
        //???? ?????? ????
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width - word.key.Length; j++)
            {
                var tmp = new List<int>();
                for (int k = 0; k < word.key.Length; k++)
                {
                    tmp.Add(GetIndex(new Vector2(j + k, i)));
                }
                vaildPos.Add(tmp.ToArray());
            }
        }
        var selectPos = vaildPos
            .OrderByDescending(x => Random.Range(0f, 100f))
            .First();
        for(int i = 0;i < selectPos.Length; i++)
            alphabets[selectPos[i]] = wordAlphabets[i];

        //var correctPosition = GetVaildPositions(word.key.Length)
        //    .OrderBy(x => Random.Range(0f, 100f))
        //    .First();

        //for (int i = 0; i < wordAlphabets.Length; i++)
        //{
        //    var pos = GetIndex(correctPosition);
        //    alphabets[pos] = wordAlphabets[i];
        //    if (correctPosition.x + 1 < width)
        //        correctPosition.x += 1;
        //    else
        //        correctPosition.y += 1;
        //}
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