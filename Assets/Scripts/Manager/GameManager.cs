using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJGameLibrary.DesignPattern;
using System;
using System.Linq;

public class GameManager : MonoSingleton<GameManager>
{
    public eAlphabet currentAlphabet { get; set; }
    public eContents currentContents { get; private set; }

    private Dictionary<eAlphabet, AlphabetData> datas;

    public override void Initialize()
    {
        base.Initialize();
        currentAlphabet = eAlphabet.A;
        currentContents = eContents.JT_PL1_102;
        var audios = LocalDB.Instance.Get<AlphabetAudioData>();
        var sprites = LocalDB.Instance.Get<AlphabetSpriteData>();
        var sentance = LocalDB.Instance.Get<SentanceData>();
        var words = LocalDB.Instance.Get<WordsData>();
        datas = alphabets.Select(x => new AlphabetData(x,
            sentance.Get(x),
            words.Get(x),
            audios.Get(x)))
            .ToDictionary(x => x.Alphabet, x => x);
    }
    public AudioClip GetClipCorrectEffect() => LocalDB.Instance.GetCorrectClip();
    public AlphabetData GetResources() => datas[currentAlphabet];
    public AlphabetData GetResources(eAlphabet alphabet) => datas[alphabet];
    public AlphabetSpriteData.AlphabetSpritePair GetAlphbetSprite(eAlphabetStyle style) => LocalDB.Instance.Get<AlphabetSpriteData>().Get(style);
    public Sprite[] GetAlphbetSprite(eAlphabetStyle style, eAlphabetType type) => LocalDB.Instance.Get<AlphabetSpriteData>().Get(style,type);
    public Sprite GetAlphbetSprite(eAlphabetStyle style, eAlphabetType type, eAlphabet alphabet) => LocalDB.Instance.Get<AlphabetSpriteData>().Get(style, type, alphabet);
    public eAlphabet[] alphabets => Enum.GetNames(typeof(eAlphabet)).Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x)).ToArray();
    public WordsData.WordSources FindWord(eAlphabet alphabet, string value) => datas[alphabet].Words.ToList().Find(x => x.value == value);
}

public class AlphabetData
{
    public eAlphabet Alphabet { get; private set; }
    public SentanceData.SentancesSource[] Sentances { get; private set; }
    public WordsData.WordSources[] Words { get; private set; }

    public AlphabetAudioData.AlphabetAudioSource AudioData { get; private set; }
    public AlphabetData(eAlphabet alphabet, SentanceData.SentancesSource[] sentances, WordsData.WordSources[] words, AlphabetAudioData.AlphabetAudioSource audioData)
    {
        Alphabet = alphabet;
        Sentances = sentances;
        Words = words;
        AudioData = audioData;
    }
    public Sprite Get(eAlphabetStyle style, eAlphabetType type) => GameManager.Instance.GetAlphbetSprite(style, type, Alphabet);
}
