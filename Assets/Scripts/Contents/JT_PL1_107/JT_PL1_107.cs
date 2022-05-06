using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        var words = GameManager.Instance.GetWords()
            .Where(x => x.First().ToString().ToUpper() == GameManager.Instance.currentAlphabet.ToString())
            .Take(drops.Length)
            .ToArray();

        drops = drops.OrderBy(x => Guid.NewGuid().ToString()).ToArray();
        drags = drags.OrderBy(x => Guid.NewGuid().ToString()).ToArray();

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
    private void PlayAudio(string word)
    {
        audioPlayer.Play(GameManager.Instance.GetClipWord(word));
    }
    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetClipAct2(GameManager.Instance.currentAlphabet), base.ShowResult);
    }
}

