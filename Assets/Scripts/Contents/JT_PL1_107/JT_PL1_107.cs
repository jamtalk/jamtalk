using System.Linq;
using UnityEngine;

public class JT_PL1_107 : BaseMatchImage<WordSource>
{
    protected override eContents contents => eContents.JT_PL1_107;
    protected override int GetTotalScore() => drops.Length;
    protected override bool CheckOver() => !drops.Select(x => x.isConnected).Contains(false);

    protected override void GetWords()
    {
        words = GameManager.Instance.GetResources().Words
            .OrderBy(x => Random.Range(0f, 100f))
            .Take(drops.Length)
            .ToArray();
        SetElement(words);
    }

    protected override void PlayAudio(DataSource word)
    {
        var data = (WordSource)word;
        audioPlayer.Play(data.clip);
    }

    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}

