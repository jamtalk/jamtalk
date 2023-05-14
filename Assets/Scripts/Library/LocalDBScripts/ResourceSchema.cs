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
    [SerializeField]
    private TextAsset bookJson;
    public AudioClip correctSound;
    [SerializeField]
    private ResourceData _data = null;
    public ResourceData data
    {
        get
        {
            if (_data == null)
                _data = JObject.Parse(orizinal.text).ToObject<ResourceData>();
            return _data;
        }
    }
    private BookMetaData[] _bookData = null;
    public BookMetaData[] bookData
    {
        get
        {
            if (_bookData == null)
            {
                _bookData = JArray.Parse(bookJson.text).ToObject<BookMetaData[]>();
                for (int i = 0; i < _bookData.Length; i++)
                    _bookData[i].SetBook();
            }
            return _bookData;
        }
    }
    public AlphabetAudioData GetAlphabetAudio(eAlphabet alphabet) => data.alphabetAudio.ToList().Find(x => x.Alphabet == alphabet);
    public VowelAudioData GetVowelAudio(eAlphabet vowel) => data.vowelAudio.ToList().Find(x => x.Vowel == vowel);
    public DigraphsAudioData GetDigrpahsAudio(string digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs);
    public DigraphsAudioData GetDigrpahsAudio(eDigraphs digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs.ToString());
    public DigraphsAudioData GetDigrpahsAudio(ePairDigraphs digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs.ToString());
    public BookMetaData[] GetBookData(eBookType type, int bookNumber) => bookData.Where(x => x.type == type).Where(x => x.bookNumber == bookNumber).OrderBy(x=>x.page).ToArray();
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

#region BookData (Legacy)

//public class BookDataElement : ResourceElement
//{
//    [JsonIgnore]
//    public eBookType type => (eBookType)Enum.Parse(typeof(eBookType), key);
//}
//public class BookTitleData
//{
//    public string key;
//    //public string type;
//    [JsonIgnore]
//    public eBookType type;
//    public int bookNumber;
//    public int page;
//    public string value_EN;
//    public string value_KR;
//    public BookSentanceData[] book;
//    public BookConversationData[] conversationData;
//    public BookWordData[] words =>
//        GameManager.Instance.schema.data.bookWords
//        .Where(x => x.type == type)
//        .Where(x => x.bookNumber == bookNumber)
//        .OrderBy(x => x)
//        .ToArray();
//}
//public class BookConversationData : BookDataElement
//{
//    public string bookName;
//    public int number;
//    public int page;
//    public int priority;
//    public string clip;
//    [JsonIgnore]
//    public AudioClip Clip => Addressables.LoadAssetAsync<AudioClip>(clip).WaitForCompletion();
//    public string speaker;
//    public string value;
//}
//public class BookSentanceData : BaseSentanceData<eBookType>
//{
//    public string name;
//    public int number;
//    public int page;
//    public int priority;
//    [JsonIgnore]
//    public AudioClip Clip => Addressables.LoadAssetAsync<AudioClip>(clip).WaitForCompletion();
//    public string value_kr;
//    public string image;
//    [JsonIgnore]
//    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(image).WaitForCompletion();
//}
//public class BookWordData : BookDataElement
//{
//    public string bookName;
//    public int bookNumber;
//    public string word;
//    [JsonIgnore]
//    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(word).WaitForCompletion();
//}

//public class BookConversationData
//{
//    public int priority;
//    public string speaker;
//    public string value;
//}
//public class BookSentanceData : BaseSentanceData
//{
//    public string kr;
//    public string en;
//    public int priority;
//    public string clip_kr;
//    public string clip_en;
//    public string image;
//    [JsonIgnore]
//    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(image).WaitForCompletion();


//    //public BookMetaData(string kr, string en, int priority)
//    //{
//    //    this.kr = kr;
//    //    this.en = en;
//    //    this.priority = priority;
//    //}

//    public void PlayClip_KR(AudioSource source)
//    {
//        //TODO. 음원파일 받았을 경우 source로 플레이
//        if (string.IsNullOrEmpty(clip_kr))
//            AndroidPluginManager.Instance.PlayTTS(kr);
//    }
//    public void PlayClip_EN(AudioSource source)
//    {
//        //TODO. 음원파일 받았을 경우 source로 플레이
//        if (string.IsNullOrEmpty(clip_en))
//            AndroidPluginManager.Instance.PlayTTS(en);
//    }
//}
#endregion

#region BookData
public class BookMetaData
{
    public string key;
    public eBookType type;
    public int bookNumber;
    public int page;
    public string title_en;
    public string title_kr;
    public string titleClip_en;
    public string titleClip_kr;
    public BookSentanceData[] sentances;
    public BookConversationData[] conversations;
    public BookWordData[] words;

    public Sprite GetSprite() => Addressables.LoadAssetAsync<Sprite>(string.Format("Sentance/{0}/{1}/{2}",type.ToString(),bookNumber,page)).WaitForCompletion();
    public Sprite GetSprite(BookWordData data) => Addressables.LoadAssetAsync<Sprite>(string.Format("Words/{0}/{1}/{2}", type.ToString(), bookNumber, data.value)).WaitForCompletion();
    public void SetBook()
    {
        for (int i = 0; i < sentances.Length; i++)
            sentances[i].SetBook(this);
        for (int i = 0; i < conversations.Length; i++)
            conversations[i].SetBook(this);
        for (int i = 0; i < words.Length; i++)
            words[i].SetBook(this);
    }
}
public class BookConversationData
{
    public int priority;
    public string speaker;
    public string value;
    public string clip;
    public BookMetaData currentBook { get; private set; }
    public void SetBook(BookMetaData book) => currentBook = book;
    public Sprite sprite => currentBook.GetSprite();
}
public class BookSentanceData : BaseSentanceData
{
    public new string value => en;
    public new string clip => clip_en;
    public string kr;
    public string en;
    public int priority;
    public string clip_en;
    public string clip_kr;
    public BookMetaData currentBook { get; private set; }
    public void SetBook(BookMetaData book) => currentBook = book;
    public Sprite sprite => currentBook.GetSprite();
}
public class BookWordData
{
    public string value;
    public string extension;
    public string clip;
    public BookMetaData currentBook { get; private set; }
    public void SetBook(BookMetaData book) => currentBook = book;
    public Sprite sprite => currentBook.GetSprite(this);
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
    public string[] inCorrectClips;
    public string[] correctPerfectClip;
    public string[] correctGreatClip;
    public string[] correctWonderfulClip;
    public string[] correctExcellentClip;
    public string[] correctGoodjobClip;
    public string[] correctAmazingClip;
    public string[] correctNiceClip;
}

