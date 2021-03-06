using System.Linq;
using UnityEngine;

public class JT_PL1_107 : BaseMatchImage<AlphabetWordsData>
{
    protected override eContents contents => eContents.JT_PL1_107;
    protected override int GetTotalScore() => drops.Length;
    protected override bool CheckOver() => !drops.Select(x => x.isConnected).Contains(false);

    protected override void GetWords()
    {
        eAlphabet[] targetAlphabets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        var halfLength = drops.Length / targetAlphabets.Length;
        words = targetAlphabets.SelectMany(x=>GameManager.Instance.GetResources(x).Words
                .OrderBy(y=>Random.Range(0f,100f)).Take(halfLength))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();

        SetElement(words);
    }

    protected override void PlayAudio(ResourceWordsElement word)
    {
        var data = (AlphabetWordsData)word;
        audioPlayer.Play(data.clip);
    }

    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetResources().AudioData.act2, base.ShowResult);
    }
}

