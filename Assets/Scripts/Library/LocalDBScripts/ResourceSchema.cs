using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.U2D;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using GJGameLibrary.ResourceData;
using GJGameLibrary.DesignPattern;
using System.Linq;

[CreateAssetMenu(fileName = "ResourceSchema.asset", menuName = "Create DB/ResourceSchema")]
public class ResourceSchema : ScriptableObject
{
    [SerializeField]
    public AudioClip correctSound;
    [SerializeField]
    public ResourceData data => JObject.Parse(Resources.Load<TextAsset>("Data").text).ToObject<ResourceData>(); 
    [SerializeField]
    private SerializableDictionaryBase<eAtlasType, SpriteAtlas> atlas;
    public Sprite GetSprite(eAtlasType type, string value) => atlas[type].GetSprite(value);

    public static bool IsPair(string digrpahs)
    {
        var pair = Enum.GetNames(typeof(ePairDigraphs));
        return pair.Contains(digrpahs);
    }
    public static eDigraphs GetPair(ePairDigraphs digraphs)
    {
        var num = (int)digraphs;
        return (eDigraphs)num;
    }
    public static ePairDigraphs GetPair(eDigraphs digraphs)
    {
        var num = (int)digraphs;
        return (ePairDigraphs)num;
    }
    public static ResourceSchema Instance => Resources.Load<ResourceSchema>("ResourceSchema");
}

namespace GJGameLibrary.ResourceData
{
    [Serializable]
    public abstract class ResourceElement
    {
        public string key;
    }
    public abstract class ResourceWordsElement : ResourceElement
    {
        public string act;
        public string clip;
        protected abstract eAtlasType atalsType { get; }
        public Sprite sprite => ResourceSchema.Instance.GetSprite(atalsType, key);
    }
    [Serializable]
    public class AlphabetWordsData : ResourceWordsElement
    {
        protected override eAtlasType atalsType => eAtlasType.Words;
        public string alphabet;
        public eAlphabet Alphabet => (eAlphabet)Enum.Parse(typeof(eAlphabet), alphabet);
        public AlphabetAudioData AudioData => ResourceSchema.Instance.data.alphabetAudio.ToList().Find(x => x.Alphabet == Alphabet);
    }
    [Serializable]
    public class AlphabetAudioData : ResourceElement
    {
        public string clip;
        public string act1;
        public string act2;
        public string phanics;
        public eAlphabet Alphabet => (eAlphabet)Enum.Parse(typeof(eAlphabet), key);
    }
    [Serializable]
    public class VowelWordsData : ResourceWordsElement
    {
        protected override eAtlasType atalsType => eAtlasType.Vowels;
        public string type;
        public string vowel;
        public eVowelType Type => (eVowelType)Enum.Parse(typeof(eVowelType), type);
        public eAlphabet Vowel => (eAlphabet)Enum.Parse(typeof(eAlphabet), vowel);
        public VowelAudioData AudioData => ResourceSchema.Instance.data.vowelAudio.ToList().Find(x => x.Vowel == Vowel);
    }
    [Serializable]
    public class VowelAudioData : ResourceElement
    {
        public string act_long;
        public string act_short;
        public string clip;
        public string phanics_long;
        public string phanics_short;
        public eAlphabet Vowel => (eAlphabet)Enum.Parse(typeof(eAlphabet), key);
    }
    [Serializable]
    public class DigraphsWordsData : ResourceWordsElement
    {
        public int level;
        public string digraphs;
        public bool IsPair => ResourceSchema.IsPair(digraphs);
        public DigrpahsAudioData AudioData => ResourceSchema.Instance.data.digraphsAudio.ToList().Find(x => x.key == digraphs);

        protected override eAtlasType atalsType => eAtlasType.Digraphs;
    }
    [Serializable]
    public class DigrpahsAudioData : ResourceElement
    {
        public string phanics;
    }
    [Serializable]
    public class ResourceData
    {
        public AlphabetWordsData[] alphabetWords;
        public AlphabetAudioData[] alphabetAudio;
        public VowelWordsData[] vowelWords;
        public VowelAudioData[] vowelAudio;
        public DigraphsWordsData[] digraphsWords;
        public DigrpahsAudioData[] digraphsAudio;
    }
}
