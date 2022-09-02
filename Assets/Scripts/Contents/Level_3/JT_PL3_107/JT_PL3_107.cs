using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JT_PL3_107 : BaseMatchImage<DigraphsWordsData>
{
    protected override eContents contents => eContents.JT_PL3_107;
    protected override int GetTotalScore() => drops.Length;
    protected override bool CheckOver() => digraphsIndex == QuestionCnt;
    protected int digraphsIndex = 0;
    private int QuestionCnt => 4;
    private int dropCount = 0;


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
    protected override void onDrop()
    {
        dropCount += 1;

        if (dropCount == words.Length)
        {
            digraphsIndex += 1;
            if (QuestionCnt == digraphsIndex)
                base.onDrop();
            else
            {
                GetWords();
                for (int i = 0; i < words.Length; i++)
                {
                    drags[i].Reset();
                    drops[i].Reset();
                }

                dropCount = 0;
            }
        }
        audioPlayer.Play(1f, GameManager.Instance.GetClipCorrectEffect());
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
