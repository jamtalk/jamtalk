using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class JT_PL3_110 : BingoContents<DigraphsContentsSetting, DigraphsWordsData, DigraphsBingoButton, Text, DigraphsBingoBoard>
{
    protected override eContents contents => eContents.JT_PL3_110;

    protected override DigraphsWordsData[] correctsTarget =>
        GameManager.Instance.digrpahs
        .SelectMany(x => GameManager.Instance.GetDigraphs(x))
        .Where(x => x.Digraphs == GameManager.Instance.currentDigrpahs)
        .Where(x => x.key.Length < 6)
        .Distinct()
        .OrderBy(x => Random.Range(0f, 100f))
        .ToArray();

    public override DigraphsWordsData[] GetQuestionType()
    {
        var result = GameManager.Instance.digrpahs
            .SelectMany(x => GameManager.Instance.GetDigraphs(x))
            .Where(x => x.Digraphs != GameManager.Instance.currentDigrpahs)
            .Where(x => x.key.Length < 6)
            .Distinct()
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .ToArray();
        var sounds = result.Select(x => x.clip).Distinct().ToArray();

        for (int i = 0; i < sounds.Length; i++)
            SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(sounds[i]));

        return result;
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.clip, () => isNext = true);

    protected override bool IsCurrentAnswer(DigraphsWordsData value) => value.key == currentQuestion.key;

    protected override string GetValue()
    {
        return currentQuestion.key;
    }
}
