using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_107 : BaseMatchImage<DigraphsSource>
{
    protected override eContents contents => eContents.JT_PL3_107;
    protected override int GetTotalScore() => 12;
    protected override bool CheckOver() =>
        digraphsIndex == 2 && !drops.Select(x => x.isConnected).Contains(false);

    private eDigraphs[] eDig = { eDigraphs.CH, eDigraphs.SH, eDigraphs.TH };
    private int digraphsIndex = 0;
    private int dropCount = 0;

    protected override void Awake()
    {
        base.Awake();

        GetWords();
    }

    protected override void GetWords()
    {
        Debug.Log(digraphsIndex + eDig[digraphsIndex].ToString());

        words = GameManager.Instance.digrpahs
            .SelectMany(x=>GameManager.Instance.GetDigraphs(x))
            .Where( x => x.type == eDig[digraphsIndex])
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(drops.Length)
            .ToArray();

        Debug.Log(words.Length);

        SetElement(words);
        digraphsIndex += 1;
    }

    protected override void onDrop()
    {
        dropCount += 1;

        if (dropCount == words.Length)
        {
            dropCount = 0;
            GetWords(); // ch , th , sh sound
            for(int i = 0; i < words.Length; i++)
            {
                drags[i].Reset();
                drops[i].Reset();
            }
        }


        Debug.Log(CheckOver());

        if (CheckOver())
        {   // 결과 도출하기
            ShowResult();
        }
        else
            audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
    }

    protected override void PlayAudio(DataSource word)
    {
        var data = (DigraphsSource)word;
        data.PlayClip();
    }

    protected override void ShowResult()
    {   // th sound
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}
