using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    public AudioSinglePlayer audioPlayer;
    public AudioClip tabClip;
    public AudioClip dropClip;

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
                index += 1;
                var value = target.name.ToUpper();
                if (index < 5)
                    Speak(value);
                else if (index > 5)
                    LongSpeak(value);

                audioPlayer.Play(1f, dropClip);

                if (index == 5)
                {
                    Reset();
                }

                if (CheckOver())
                    ShowResult();
            }
        }
    }

    private void Reset()
    {
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
            Speak(value);
        else
            LongSpeak(value);

        audioPlayer.Play(1f, tabClip);
    }

    private void Speak(string value)
    {
        ani.SetBool("Speak", true);
        eAlphabet alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), value);
        speakAudioPlayer.Play(GameManager.Instance.GetResources(alphabet).AudioData.phanics);
    }

    private void LongSpeak(string value)
    {
        var temp = GameManager.Instance.vowels;
        var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), value);
        var clips = GameManager.Instance.GetVowelClips(eVowelType.Long);
        clips[alphabet].Invoke();
    }
}
