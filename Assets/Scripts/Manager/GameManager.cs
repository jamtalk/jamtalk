using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJGameLibrary.DesignPattern;
using System;
using System.Linq;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class GameManager : MonoSingleton<GameManager>
{
    public delegate void PlayTTS();
    public eAlphabet currentAlphabet { get; set; }
    public eContents currentContents { get; set; }
    public eDigraphs currentDigrpahs { get; set; } = eDigraphs.CH;
    public eVowelType currentVowel { get; set; }

    private Dictionary<eAlphabet, AlphabetData> datas;
    private Dictionary<eDigraphs, DigraphsSource[]> digraphs;

    public override void Initialize()
    {
        base.Initialize();
        currentAlphabet = eAlphabet.A;
        currentContents = eContents.JT_PL1_102;
        var audios = LocalDB.Instance.Get<AlphabetAudioData>();
        var sprites = LocalDB.Instance.Get<AlphabetSpriteData>();
        var sentance = LocalDB.Instance.Get<SentanceData>();
        var words = LocalDB.Instance.Get<WordsData>();
        var vowels = LocalDB.Instance.Get<VowelData>();
        datas = alphabets.Select(x => new AlphabetData(x,
            sentance.Get(x),
            words.Get(x),
            vowels.Get(x),
            audios.Get(x)))
            .ToDictionary(x => x.Alphabet, x => x);
        Debug.Log(string.Format("Datas : {0}°³\n{1}", datas.Count, string.Join("\n",datas.Select(x => x.Key))));

        var digraphsData = LocalDB.Instance.Get<DigraphsData>().Get();
        digraphs = digrpahs
            .ToDictionary(x => x, x => digraphsData.Where(y => y.type == x).ToArray());
    }
    public AudioClip GetClipCorrectEffect() => LocalDB.Instance.GetCorrectClip();
    public AlphabetData GetResources() => datas[currentAlphabet];
    public AlphabetData GetResources(eAlphabet alphabet) => datas[alphabet];
    public Dictionary<eAlphabet, PlayTTS> GetVowelClips(eVowelType type)
    {
        var dic = new Dictionary<eAlphabet, PlayTTS>();
        for(int i = 0;i< vowels.Length; i++)
        {
            var vowel = vowels[i];
            dic.Add(vowel,
                GetResources(vowel).Vowels
                .Where(x => x.type == type)
                .Where(x => x.alphabet == vowel)
                .First().PlayAct);
        }
        return dic;
    }
    public DigraphsSource[] GetDigraphs(eDigraphs type) => digraphs[type];
    public DigraphsSource[] GetDigraphs() => GetDigraphs(currentDigrpahs);
    public DigraphsSource[] GetDigraphs(int level) => digraphs.SelectMany(x => x.Value).Where(x => x.TargetLevel == level).ToArray();
    public AlphabetSpriteData.AlphabetSpritePair GetAlphbetSprite(eAlphabetStyle style) => LocalDB.Instance.Get<AlphabetSpriteData>().Get(style);
    public Sprite[] GetAlphbetSprite(eAlphabetStyle style, eAlphabetType type) => LocalDB.Instance.Get<AlphabetSpriteData>().Get(style,type);
    public Sprite GetAlphbetSprite(eAlphabetStyle style, eAlphabetType type, eAlphabet alphabet) => LocalDB.Instance.Get<AlphabetSpriteData>().Get(style, type, alphabet);
    public eAlphabet[] alphabets => Enum.GetNames(typeof(eAlphabet)).Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x)).ToArray();
    public eAlphabet[] vowels => new eAlphabet[] { eAlphabet.A, eAlphabet.E, eAlphabet.I, eAlphabet.O, eAlphabet.U };
    public eDigraphs[] digrpahs => Enum.GetNames(typeof(eDigraphs))
        .Select(x => (eDigraphs)Enum.Parse(typeof(eDigraphs), x))
        .ToArray();
    public WordSource FindWord(eAlphabet alphabet, string value) => datas[alphabet].Words.ToList().Find(x => x.value == value);
    public Vector3 GetMousePosition(float z = 0)
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = z;
        return pos;
    }
}

public class AlphabetData
{
    public eAlphabet Alphabet { get; private set; }
    public bool IsVowel => Vowels == null || Vowels.Length == 0;
    public SentanceData.SentancesSource[] Sentances { get; private set; }
    public WordSource[] Words { get; private set; }
    public VowelSource[] Vowels { get; private set; }

    public AlphabetAudioData.AlphabetAudioSource AudioData { get; private set; }
    public AlphabetData(eAlphabet alphabet, SentanceData.SentancesSource[] sentances, WordSource[] words, VowelSource[] vowels, AlphabetAudioData.AlphabetAudioSource audioData)
    {
        Alphabet = alphabet;
        Sentances = sentances;
        Words = words;
        Vowels = vowels;
        AudioData = audioData;
    }
    public Sprite Get(eAlphabetStyle style, eAlphabetType type) => GameManager.Instance.GetAlphbetSprite(style, type, Alphabet);
}
