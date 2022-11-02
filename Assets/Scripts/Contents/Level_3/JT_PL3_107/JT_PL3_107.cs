using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_107 : BaseMatchImage<DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL3_107;


    protected override void GetWords()
    {
        words = GameManager.Instance.digrpahs
            .SelectMany(x=>GameManager.Instance.GetDigraphs(x))
            .Where( x => x.Digraphs == GameManager.Instance.currentDigrpahs)
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(drops.Length)
            .ToArray();
        SetElement(words);
    }

    protected override void PlayAudio(ResourceWordsElement word)
    {
        var data = (DigraphsWordsData)word;
        audioPlayer.Play(data.clip);
    }

    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}
