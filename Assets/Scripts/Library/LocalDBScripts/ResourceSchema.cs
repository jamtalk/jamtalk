using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.U2D;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using GJGameLibrary.DesignPattern;
using System.Linq;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ResourceSchema.asset", menuName = "Create DB/ResourceSchema")]
public class ResourceSchema : ScriptableObject
{
    [SerializeField]
    private TextAsset orizinal;
    public AudioClip correctSound;
    [SerializeField]
    public ResourceData data => JObject.Parse(orizinal.text).ToObject<ResourceData>();
    public AlphabetAudioData GetAlphabetAudio(eAlphabet alphabet) => data.alphabetAudio.ToList().Find(x => x.Alphabet == alphabet);
    public VowelAudioData GetVowelAudio(eAlphabet vowel) => data.vowelAudio.ToList().Find(x => x.Vowel == vowel);
    public DigraphsAudioData GetDigrpahsAudio(string digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs);
    public DigraphsAudioData GetDigrpahsAudio(eDigraphs digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs.ToString());
    public DigraphsAudioData GetDigrpahsAudio(ePairDigraphs digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs.ToString());
    public YoutubeURL[] GetYoutubeURL(string type) => data.youtubeURL.Where(x => x.type == type).OrderBy(x => x.level).ToArray();
    public string GetSiteWordsClip(string value)
    {
        value = GJGameLibrary.GJStringFormatter.OnlyEnglish(value).ToLower();
        var data = this.data.siteWords.ToList().Find(x => x.key == value);
        if (data != null)
            return data.clip;
        else if (value == "i")
            return GetVowelAudio(eAlphabet.I).phanics_long;
        else if (value == "a")
            return GetVowelAudio(eAlphabet.A).phanics_short;
        else
        {
            var word = this.data.vowelWords.Select(x => (ResourceWordsElement)x)
                .Union(this.data.digraphsWords.Select(x => (ResourceWordsElement)x))
                .Union(this.data.alphabetWords.Select(x => (ResourceWordsElement)x))
                .ToList()
                .Find(x => x.key.ToLower() == value.ToLower());

            if (word != null)
            {
                return word.clip;
            }
            else
            {
                return string.Empty;
            }
        }
    }
    public static bool IsPair(string digrpahs)
    {
        var pair = Enum.GetNames(typeof(eDigraphs));
        return !pair.Contains(digrpahs);
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
    public static AudioClip GetInCorrectClip()
    {
        var value = GameManager.Instance.schema.data.inCorrectClips
            .OrderBy(x => Random.Range(0f, 1000f))
            .First();
        var clip = Addressables.LoadAssetAsync<AudioClip>(value).WaitForCompletion();
        return clip;
    }

    public static (AudioClip, eSuccessType) GetCorrectClip()
    {
        Array values = Enum.GetValues(typeof(eSuccessType));
        var type =  (eSuccessType)values.GetValue(new System.Random().Next(0, values.Length));

        string[] clips;
        switch (type)
        {
            case eSuccessType.Amazing:
                clips = GameManager.Instance.schema.data.correctAmazingClip;
                break;
            case eSuccessType.Excellent:
                clips = GameManager.Instance.schema.data.correctExcellentClip;
                break;
            case eSuccessType.Goodjob:
                clips = GameManager.Instance.schema.data.correctGoodjobClip;
                break;
            case eSuccessType.Great:
                clips = GameManager.Instance.schema.data.correctGreatClip;
                break;
            case eSuccessType.Nice:
                clips = GameManager.Instance.schema.data.correctNiceClip;
                break;
            case eSuccessType.Wonderful:
                clips = GameManager.Instance.schema.data.correctWonderfulClip;
                break;
            case eSuccessType.Perfect:
            default:
                clips = GameManager.Instance.schema.data.correctPerfectClip;
                break;
        }

        var value = clips
            .OrderBy(x => Random.Range(0f, 1000f))
            .First();
        var clip = Addressables.LoadAssetAsync<AudioClip>(value).WaitForCompletion();
        return (clip, type);
    }
}

#region Base
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
    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(key).WaitForCompletion();
}
#endregion

public class BaseSentanceData : ResourceElement
{
    public string value;
    public string clip;
    public string[] words => value.Split(' ').Where(x => !string.IsNullOrEmpty(value)).ToArray();
}
public class BaseSentanceData<TKey> : BaseSentanceData where TKey : Enum
{
    [JsonIgnore]
    public TKey Key => (TKey)Enum.Parse(typeof(TKey), key);
}

#region Alphabet Data
[Serializable]
public class AlphabetWordsData : ResourceWordsElement
{
    protected override eAtlasType atalsType => eAtlasType.Words;
    public string alphabet;
    public eAlphabet Key => (eAlphabet)Enum.Parse(typeof(eAlphabet), alphabet);
    public AlphabetAudioData audio => GameManager.Instance.schema.data.alphabetAudio.ToList().Find((Predicate<AlphabetAudioData>)(x => x.Alphabet == Key));
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
public class AlphabetSentanceData : BaseSentanceData<eAlphabet> { }
#endregion

#region VowelData
[Serializable]
public class VowelWordsData : ResourceWordsElement
{
    protected override eAtlasType atalsType => eAtlasType.Vowels;
    public string type;
    public string vowel;
    public eVowelType VowelType => (eVowelType)Enum.Parse(typeof(eVowelType), type);
    public eAlphabet Vowel => (eAlphabet)Enum.Parse(typeof(eAlphabet), vowel);
    public VowelAudioData audio => GameManager.Instance.schema.data.vowelAudio.ToList().Find(x => x.Vowel == Vowel);
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
#endregion

#region Digrpahs Data
[Serializable]
public class DigraphsWordsData : ResourceWordsElement
{
    public int level;
    public string digraphs;
    public eDigraphs Digraphs
    {
        get
        {
            if (ResourceSchema.IsPair(digraphs))
            {
                var _dig = (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), digraphs);
                var num = (int)_dig;
                return (eDigraphs)num;
            }
            else
            {
                return (eDigraphs)Enum.Parse(typeof(eDigraphs), digraphs);
            }
        }
    }
    public ePairDigraphs PairDigrpahs
    {
        get
        {
            if (ResourceSchema.IsPair(digraphs))
            {
                return (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), digraphs);
            }
            else
                return (ePairDigraphs)Enum.Parse(typeof(eDigraphs), digraphs);
        }
    }
    public bool IsPair => ResourceSchema.IsPair(digraphs);
    public DigraphsAudioData audio
    {
        get
        {
            var result = GameManager.Instance.schema.data.digraphsAudio.ToList().Find(x => x.key == Digraphs.ToString());
            if(result == null)
                result = GameManager.Instance.schema.data.digraphsAudio.ToList().Find(x => x.key == PairDigrpahs.ToString());

            return result;
        }
    }

    public string IncludedDigraphs
    {
        get
        {
            if (key.ToLower().Contains(Digraphs.ToString().ToLower()))
                return Digraphs.ToString().ToLower();
            else if (key.ToLower().Contains(PairDigrpahs.ToString().ToLower()))
                return PairDigrpahs.ToString().ToLower();
            else
                return null;
        }
    }
    protected override eAtlasType atalsType => eAtlasType.Digraphs;
}
[Serializable]
public class DigraphsAudioData : ResourceElement
{
    public string phanics;
}

public class DigraphsSentanceData : BaseSentanceData<eDigraphs>
{
    public eDigraphsType type;
}
#endregion

[Serializable]
public class SiteWordData : ResourceElement
{
    public string clip;
}

[Serializable]
public class ResourceData
{
    public AlphabetWordsData[] alphabetWords;
    public AlphabetAudioData[] alphabetAudio;
    public VowelWordsData[] vowelWords;
    public VowelAudioData[] vowelAudio;
    public DigraphsWordsData[] digraphsWords;
    public DigraphsAudioData[] digraphsAudio;
    public AlphabetSentanceData[] alphabetSentaces;
    public DigraphsSentanceData[] digraphsSentances;
    public SiteWordData[] siteWords;
    public YoutubeURL[] youtubeURL;
    public string[] inCorrectClips;
    public string[] correctPerfectClip;
    public string[] correctGreatClip;
    public string[] correctWonderfulClip;
    public string[] correctExcellentClip;
    public string[] correctGoodjobClip;
    public string[] correctAmazingClip;
    public string[] correctNiceClip;
}

public class YoutubeURL
{
    public string type;
    public int level;
    public string title;
    public string animationURL;
    public string songURL;
}

