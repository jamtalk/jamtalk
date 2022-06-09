using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL1_112 : BaseContents
{
    public DropToggle[] toggles;
    public DragAlphabet[] drags;
    public AudioSinglePlayer audioPlayer;
    protected override eContents contents => eContents.JT_PL1_112;

    protected override bool CheckOver() => !toggles.Select(x => x.isOn).Contains(false);
    protected override int GetTotalScore() => toggles.Length;
    protected override void Awake()
    {
        base.Awake();
        var corrects = GameManager.Instance.alphabets
            .Where(x => x >= GameManager.Instance.currentAlphabet)
            .OrderBy(x=>x)
            .Take(toggles.Length)
            .ToArray();
        var incorrects = GameManager.Instance.alphabets
            .Where(x => !corrects.Contains(x))
            .OrderBy(x => UnityEngine.Random.Range(0f, 100f))
            .Take(drags.Length - toggles.Length)
            .Union(corrects)
            .OrderBy(x => UnityEngine.Random.Range(0f,100f))
            .ToArray();
        for(int i = 0;i < toggles.Length; i++)
        {
            AddToggleDropListener(toggles[i]);
            toggles[i].Init(corrects[i], false);
        }
        for(int i = 0;i < drags.Length; i++)
            drags[i].Init(incorrects[i]);
    }
    private void AddToggleDropListener(DropToggle toggle)
    {
        toggle.onDroped += () =>
        {
            for (int i = 0; i < drags.Length; i++)
                drags[i].intracable = false;
            audioPlayer.Play(GameManager.Instance.GetResources(toggle.alphabet).AudioData.act2,()=>
            {
                if (CheckOver())
                    ShowResult();
                else
                {
                    for (int i = 0; i < drags.Length; i++)
                        drags[i].intracable = true;
                }
            });
        };
    }
}
