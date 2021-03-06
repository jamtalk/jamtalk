using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JT_PL1_116 : BaseContents
{
    public EventSystem eventSystem;
    public AnswerImage116 answerImage;
    public AlphabetButton[] upper;
    public AlphabetButton[] lower;
    public Button[] buttonPlayer;
    public AudioClip clipClick;
    private int length => upper.Length;
    protected override eContents contents => eContents.JT_PL1_116;

    protected override bool CheckOver() => currentIndex == length;

    private AlphabetWordsData[] words;
    private eAlphabet[] alphabets;

    private int currentIndex=0;

    private List<AlphabetButton> selected = new List<AlphabetButton>();
    protected override int GetTotalScore() => upper.Length;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < buttonPlayer.Length; i++)
            buttonPlayer[i].onClick.AddListener(PlayWord);
        var current = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        if (GameManager.Instance.currentAlphabet < eAlphabet.C)
            alphabets = current.SelectMany(x => new eAlphabet[] { x, x }).ToArray();
        else
        {
            var pre = GameManager.Instance.alphabets
                .Where(x => x < GameManager.Instance.currentAlphabet)
                .OrderBy(x => Random.Range(0f, 100f))
                .Take(2);

            alphabets = current.Union(pre).ToArray();
        }

        //alphabets = alphabets
        //    .SelectMany(x => new eAlphabet[] { x, x })
        //    .ToArray();

        words = alphabets
            .Select(x => GameManager.Instance.GetResources(x).Words.OrderBy(y => Random.Range(0f, 100f)).First())
            .ToArray();

        Debug.Log(string.Join("\n", words.Select(x => string.Format("{0} : {1}", x.alphabet, x.key))));

        upper = upper.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        lower = lower.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for(int i = 0;i < length; i++)
        {
            upper[i].Init(alphabets[i], eAlphabetStyle.Brown, eAlphabetType.Upper);
            lower[i].Init(alphabets[i], eAlphabetStyle.Brown, eAlphabetType.Lower);

            AddButtonListener(upper[i]);
            AddButtonListener(lower[i]);
        }
        PlayWord();
    }
    private void PlayWord()
    {
        audioPlayer.Play(words[currentIndex].audio.act2);
    }
    
    private void AddButtonListener(AlphabetButton button)
    {
        button.onClick += (value) =>
        {
            button.button.interactable = false;
            selected.Add(button);

            if (selected.Count == 2)
            {
                if (selected[0].value == alphabets[currentIndex] &&  selected[1].value == alphabets[currentIndex] && selected[0].type != selected[1].type)
                {
                    var clip = words[currentIndex].act;
                    answerImage.Show(words[currentIndex].sprite);
                    audioPlayer.Play(clip,()=>
                    {
                        answerImage.gameObject.SetActive(false);
                        eventSystem.enabled = true;
                        currentIndex += 1;
                        if (CheckOver())
                        {
                            ShowResult();
                        }
                        else
                        {
                            PlayWord();
                        }
                    });
                }
                else
                {
                    selected[0].button.interactable = true;
                    selected[1].button.interactable = true;
                }
                selected.Clear();
            }
            else
            {
                audioPlayer.Play(1f,clipClick);
            }

        };
    }
}
