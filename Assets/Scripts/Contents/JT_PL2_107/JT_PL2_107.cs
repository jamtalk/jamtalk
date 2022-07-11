using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL2_107 : BaseMatchImage<VowelSource>
{
    protected override eContents contents => eContents.JT_PL2_107;

    protected override void GetWords()
    {
        words = GameManager.Instance.GetResources().Vowels
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(drops.Length)
            .ToArray();
        SetElement(words);
    }

    protected override void PlayAudio(DataSource word)
    {
        var data = (VowelSource)word;
        audioPlayer.Play(data.clip);
    }

    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}
