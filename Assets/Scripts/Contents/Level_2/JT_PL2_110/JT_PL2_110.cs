using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class JT_PL2_110 : BingoContents<AlphabetContentsSetting, AlphabetWordsData, WordBingoButton, Text, WordBingoBoard>
{
    protected override eContents contents => eContents.JT_PL2_110;

    protected override AlphabetWordsData[] correctsTarget
    {
        get
        {
            if(_correctsTarget == null)
            {
                _correctsTarget =  GameManager.Instance.alphabets
                .SelectMany(x => GameManager.Instance.GetResources(x).Words)
                .Where(x => x.Key == GameManager.Instance.currentAlphabet)
                .Where(x => x.key.Length < 6)
                .Distinct()
                .OrderBy(x => Random.Range(0f, 100f))
                .ToArray();
            }

            return _correctsTarget;
        }
    }

    public override AlphabetWordsData[] GetQuestionType()
    {
        var result = GameManager.Instance.alphabets
            .Select(x => GameManager.Instance.GetResources(x))
            .Where(x => !correctsTarget.Select(y => y.Key).Contains(x.Alphabet))
            .SelectMany(x=>x.Words)
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

    protected override bool IsCurrentAnswer(AlphabetWordsData value) => value.key == currentQuestion.key;

    protected override string GetValue()
    {
        return currentQuestion.key;
    }
}