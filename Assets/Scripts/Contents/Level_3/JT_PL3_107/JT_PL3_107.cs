using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class JT_PL3_107 : BaseMatchImage<DigraphsContentsSetting,DigraphsWordsData>
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
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < words.Length; i++)
            SceneLoadingPopup.SpriteLoader.Add(Addressables.LoadAssetAsync<AudioClip>(words[i].clip));
    }

    protected override void PlayAudio(ResourceWordsElement word)
    {
        var data = (DigraphsWordsData)word;
        Debug.Log(data.clip);
        audioPlayer.Play(data.clip);
    }

    protected override void ShowResult()
    {
        audioPlayer.Play(GameManager.Instance.GetDigraphs().First().audio.phanics, base.ShowResult);
    }
}
