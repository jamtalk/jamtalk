using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_107 : BaseMatchImage<DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL3_107;
    protected override int GetTotalScore() => 12;
    protected override bool CheckOver() =>
        !drops.Select(x => x.isConnected).Contains(false);

    private eDigraphs[] eDig = { eDigraphs.CH, eDigraphs.SH, eDigraphs.TH };
    private int digraphsIndex = 0;
    private int dropCount = 0;

    protected override void GetWords()
    {
        words = GameManager.Instance.digrpahs
            .SelectMany(x=>GameManager.Instance.GetDigraphs(x))
            .Where( x => x.type == eDig[digraphsIndex])
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(drops.Length)
            .ToArray();

        SetElement(words);
        digraphsIndex += 1;
        Debug.Log("digraphgs " + digraphsIndex);
    }
    protected override void onDrop()
    {
        dropCount += 1;

        if (dropCount == words.Length)
        {
            GetWords();
            for(int i = 0; i < words.Length; i++)
            {
                drags[i].Reset();
                drops[i].Reset();
            }
            dropCount = 0;
        }

        if (CheckOver())
            ShowResult();
        else
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
    }

    protected override void PlayAudio(DataSource word)
    {
        var data = (DigraphsSource)word;
        data.PlayClip();
    }

    protected override void ShowResult()
    {
        base.ShowResult();
        //audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}
