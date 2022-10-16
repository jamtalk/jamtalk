using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL2_101 : BaseContents
{
    protected override eContents contents => eContents.JT_PL2_101;

    private int index = 0;
    protected virtual int puzzleCount => 10;
    protected override bool CheckOver() => puzzleCount == index;
    protected override int GetTotalScore() => puzzleCount;

    public WordElement201[] parentLayout;
    public DragElement201[] puzzles;
    public Sprite[] puzzlesImage;
    public Animator ani;
    public Text text;

    public AudioSinglePlayer speakAudioPlayer;
    public AudioClip tabClip;
    public AudioClip dropClip;

    
    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        for(int i = 0; i < puzzleCount; i++)
        {
            var puzzle = puzzles.Where(x => x.gameObject.activeSelf)
                .OrderBy(x => Random.Range(0, 100))
                .First();
            var target = parentLayout.Where(x => x.name == puzzle.name).First();

            guideFinger.gameObject.SetActive(true);
            Debug.LogFormat("{0}, {1}", puzzle.name, target.name);
            guideFinger.DoMove(puzzle.rt.position, () =>
            {
                guideFinger.DoPress(() =>
                {
                    OnDrag(puzzle);

                    guideFinger.DoMove(puzzle.gameObject, target.rt.position, () =>
                    {
                        guideFinger.gameObject.SetActive(false);
                        guideFinger.transform.localScale = new Vector3(1f, 1f, 1f);
                        puzzle.gameObject.SetActive(false);
                        target.GetComponent<Image>().sprite = puzzle.GetComponent<Image>().sprite;

                        DropMotion(target);
                    });
                });
            });

            while (!isNext) yield return null;
            isNext = false;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < puzzles.Length; i ++)
        {
            puzzles[i].onDrag += OnDrag;
            puzzles[i].onDrop += OnDrop;
            puzzles[i].SetDefaultPosition();
        }
    }

    private void OnDrop(WordElement201 target)
    {
        for(int i = 0; i < puzzles.Length; i++)
        {
            if(puzzles[i].name.Contains(target.name))
            {
                DropMotion(target);
            }
        }
    }

    private void DropMotion(WordElement201 target)
    {
        index += 1;
        var value = target.name.ToUpper();
        if (index < 5)
            ShortSpeak(value, () => isNext = true);
        else if (index > 5)
            LongSpeak(value, () => isNext = true);

        audioPlayer.Play(1f, dropClip);

        Debug.Log(index);
        if (index == 5)
        {
            Reset();

            isNext = true;
        }

        if (CheckOver())
        {
            Speak();
            if(!isGuide)
                ShowResult();
            else
            {
                Reset();
                index = 0;
                guideFinger.gameObject.SetActive(false);
            }
        }

    }

    private void Reset()
    {
        Speak();

        text.text = "Long";
        for(int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].ResetPosition();
            puzzles[i].gameObject.SetActive(true);
            parentLayout[i].GetComponent<Image>().sprite = puzzlesImage[i];
        }
    }

    private void OnDrag(DragElement201 target)
    {
        var value = target.name.ToUpper();
        if(index < 5)
            ShortSpeak(value);
        else
            LongSpeak(value);

        audioPlayer.Play(1f, tabClip);
    }

    private void ShortSpeak(string value, Action action = null)
    {
        ani.SetBool("Speak", true);
        eAlphabet alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), value);
        speakAudioPlayer.Play(GameManager.Instance.schema.GetVowelAudio(alphabet).phanics_short, action);
    }

    private void LongSpeak(string value, Action action = null)
    {
        ani.SetBool("Speak", true);
        var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), value);
        speakAudioPlayer.Play(GameManager.Instance.schema.GetVowelAudio(alphabet).phanics_long, action);
    }

    private void Speak()
    {
        eAlphabet[] alphabets = { eAlphabet.A, eAlphabet.E, eAlphabet.I, eAlphabet.O, eAlphabet.U };
        
        if (index == 5)
        {
            for (int i = 0; i < alphabets.Length; i++)
                speakAudioPlayer.Play(GameManager.Instance.schema.GetVowelAudio(alphabets[i]).phanics_short);
        }
        else
        {
            for (int i = 0; i < alphabets.Length; i++)
                speakAudioPlayer.Play(GameManager.Instance.schema.GetVowelAudio(alphabets[i]).phanics_long);
        }
    }
}
