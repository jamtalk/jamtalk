using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class JT_PL1_117 : BingoContents<AlphabetContentsSetting,AlphabetData, BingoButton, Image, BingoBoard>
{
    protected override eContents contents => eContents.JT_PL1_117;
    public override int BingoCount => 2;

    protected override string GetValue()
    {
        return currentQuestion.Alphabet.ToString();
    }
    protected override AlphabetData[] correctsTarget
    {
        get
        {
            if (_correctsTarget == null)
            {
                var target = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
                //if (GameManager.Instance.currentAlphabet > eAlphabet.B)
                //{
                //    var privious = GameManager.Instance.alphabets.Where(x => x < GameManager.Instance.currentAlphabet)
                //        .OrderBy(x => Random.Range(0f, 100f))
                //        .Take(board.size - 2);
                //    target = target.Union(privious).ToArray();
                //}

                //_correctsTarget = target
                //    .Select(x => GameManager.Instance.GetResources(x))
                //    .ToArray();

                var privious = GameManager.Instance.alphabets
                    .Where(x=>x< GameManager.Instance.currentAlphabet)
                    .OrderBy(x => Random.Range(0, 100))
                    .Take(board.size - 2);
                target = target.Union(privious).ToArray();
                if (target.Length < board.size)
                    target = target.Union(target
                        .OrderBy(x => Random.Range(0, 100))
                        .Take(board.size - target.Length))
                        .ToArray();

                _correctsTarget = target
                    .Select(x => GameManager.Instance.GetResources(x))
                    .ToArray();
            }

            return _correctsTarget;
        }
    }

    public override AlphabetData[] GetQuestionType()
    {
        var result = GameManager.Instance.alphabets
            .Where(x => !correctsTarget.Select(y => y.Alphabet).Contains(x))
            .Take((int)Mathf.Pow(board.size, 2f))
            .OrderBy(x => Random.Range(0f, 100f))
            .Select(x => GameManager.Instance.GetResources(x))
            .ToArray();
        var sounds = result.Select(x => x.AudioData.clip).Distinct().ToArray();
        for (int i = 0; i < sounds.Length; i++)
            SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(sounds[i]));
        return result;
    }

    protected override void PlayClip() => audioPlayer.Play(currentQuestion.AudioData.clip, () => isNext = true);

    protected override bool IsCurrentAnswer(AlphabetData value) => value.Alphabet == currentQuestion.Alphabet;
}
