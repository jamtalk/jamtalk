using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JT_PL1_107 : BaseContents
{
    public CanvasScaler scaler;
    public DropSpaceShip_107[] drops;
    public DragKnob_107[] drags;
    public AudioSinglePlayer audioPlayer;
    protected override eContents contents => eContents.JT_PL1_107;
    protected override int GetTotalScore() => drops.Length;
    protected override bool CheckOver() => !drops.Select(x => x.isConnected).Contains(false);
    protected override void Awake()
    {
        GetWords();
    }
    protected virtual void GetWords()
    {
        var words = GameManager.Instance.GetResources().Words
            .Take(drops.Length)
            .ToArray();
        SetElement(words);
    }

    protected void SetElement(WordsData.WordSources[] words)
    {
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);

        drops = drops.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        drags = drags.OrderBy(x => Random.Range(0f, 100f)).ToArray();

        for (int i = 0; i < words.Length; i++)
        {
            drops[i].Init(words[i]);
            drags[i].Init(words[i]);

            drops[i].onClick += PlayAudio;
            drags[i].onClick += PlayAudio;

            drags[i].onDrop += () =>
            {
                if (CheckOver())
                    ShowResult();
                else
                    audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
            };
            drops[i].onDrop += () =>
            {
                if (CheckOver())
                    ShowResult();
                else
                    audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
            };
        }
    }

    private void PlayAudio(WordsData.WordSources word)
    {
        audioPlayer.Play(word.clip);
    }
    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}

