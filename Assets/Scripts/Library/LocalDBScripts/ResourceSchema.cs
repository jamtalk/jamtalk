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
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "ResourceSchema.asset", menuName = "Create DB/ResourceSchema")]
public class ResourceSchema : ScriptableObject
{
    [SerializeField]
    private TextAsset orizinal;
    [SerializeField]
    private TextAsset bookJson;
    public AudioClip correctSound;
    private ResourceData _data = null;
    public ResourceData data//=> JObject.Parse(orizinal.text).ToObject<ResourceData>();
    {
        get
        {
            if (_data == null)
            {
                _data = JObject.Parse(orizinal.text).ToObject<ResourceData>();
                Debug.Log("데이터 신규 로딩");
            }

            return _data;
        }
    }
    public Dictionary<eBookType, Dictionary<int, int[]>> _bookPageData;
    public Dictionary<eBookType, Dictionary<int, int[]>> bookPageData
    {
        get
        {
            if (_bookPageData == null)
            {
                _bookPageData = JArray.Parse(Resources.Load<TextAsset>("BookData/BookMetaData").text).ToObject<BookPageData[]>()
                    .ToDictionary(x => x.type, x => x.numbers.ToDictionary(y => y.number, y => y.pages));
            }
            return _bookPageData;
        }
    }
    //public BookMetaData[] bookData
    //{
    //    get
    //    {
    //        //if (_bookData == null)
    //        //{
    //            var _bookData = JArray.Parse(bookJson.text).ToObject<BookMetaData[]>();
    //            for (int i = 0; i < _bookData.Length; i++)
    //                _bookData[i].SetBook();

    //            return _bookData;
    //        //}
    //        //GC.Collect();
    //        //return _bookData;
    //    }
    //}
    public AlphabetAudioData GetAlphabetAudio(eAlphabet alphabet) => data.alphabetAudio.ToList().Find(x => x.Alphabet == alphabet);
    public VowelAudioData GetVowelAudio(eAlphabet vowel) => data.vowelAudio.ToList().Find(x => x.Vowel == vowel);
    public DigraphsAudioData GetDigrpahsAudio(string digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs);
    public DigraphsAudioData GetDigrpahsAudio(eDigraphs digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs.ToString());
    public DigraphsAudioData GetDigrpahsAudio(ePairDigraphs digraphs) => data.digraphsAudio.ToList().Find(x => x.key == digraphs.ToString());
    //public int[] GetBookNumbers(eBookType type) => bookData.Where(x => x.type == type).Select(x => x.bookNumber).Distinct().OrderBy(x => x).ToArray();
    //public int[] GetBookPages(eBookType type, int number) => bookData.Where(x => x.type == type).Where(x => x.bookNumber == number).Select(x => x.bookNumber).Distinct().OrderBy(x => x).ToArray();
    //public BookMetaData[] GetBookData(eBookType type, int bookNumber) => bookData.Where(x => x.type == type).Where(x => x.bookNumber == bookNumber).OrderBy(x => x.page).ToArray();
    //public BookMetaData GetBookData(eBookType type, int bookNumber, int page) => bookData.Where(x => x.type == type).Where(x => x.bookNumber == bookNumber).Where(x => x.page == page).First();

    public int[] GetBookNumbers(eBookType type) => bookPageData[type].Keys.ToArray();

    public int[] GetBookPages(eBookType type, int number) => bookPageData[type][number];

    private Dictionary<eBookType, Dictionary<int, BookMetaData[]>> bookDic = new Dictionary<eBookType, Dictionary<int, BookMetaData[]>>();
    public BookMetaData[] GetBookData(eBookType type, int bookNumber)
    {
        if (!bookDic.ContainsKey(type))
            bookDic.Add(type, new Dictionary<int, BookMetaData[]>());

        if (!bookDic[type].ContainsKey(bookNumber))
        {
            Debug.LogFormat("{0}책 {1}권 데이터 읽어오기\n{2}", type, bookNumber, Resources.Load<TextAsset>(string.Format("BookData/{0}/{1}", type, bookNumber)));
            var data = JsonConvert.DeserializeObject<BookMetaData[]>(Resources.Load<TextAsset>(string.Format("BookData/{0}/{1}", type, bookNumber)).text);
            for (int i = 0; i < data.Length; i++)
                data[i].SetBook();
            bookDic[type].Add(bookNumber, data);
        }
        return bookDic[type][bookNumber];
    }
    public BookMetaData GetBookData(eBookType type, int bookNumber, int page) => GetBookData(type, bookNumber).Where(x => x.page == page).First();
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
            var word = this.data.vowelWords
                .Select(x => (ResourceWordsElement)x)
                .Where(x=>x.key.ToLower() == value.ToLower())
                .Union(this.data.digraphsWords.Select(x => (ResourceWordsElement)x).Where(x => x.key.ToLower() == value.ToLower()))
                .Union(this.data.alphabetWords.Select(x => (ResourceWordsElement)x).Where(x => x.key.ToLower() == value.ToLower()))
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
        Debug.Log(type);
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
    private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    public string act;
    public string clip;
    protected abstract eAtlasType atalsType { get; }
    public Sprite sprite
    {
        get
        {
            if (string.IsNullOrEmpty(key))
                return null;

            if (!sprites.ContainsKey(key))
                sprites.Add(key, SpriteAsync.WaitForCompletion());
            return sprites[key];
        }
    }
    public AsyncOperationHandle<Sprite> SpriteAsync => Addressables.LoadAssetAsync<Sprite>(key);
    public static void ClearSprites()
    {
        sprites.Clear();
        GC.Collect();
    }
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
    public string songURL;
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

public class BookPageData
{
    public class BookPageNumber
    {
        public int number;
        public int[] pages;
    }
    public eBookType type;
    public BookPageNumber[] numbers;
}
public class BookURLData
{
    public eBookType key;
    public int number;
    public string title;
    public string animationURL;
    public string songURL;
    public string resourceRootURL;
    public string GetLog()
    {
        return string.Join("\n", GetType().GetFields()
            .Select(x => string.Format("{0} : {1}", x.Name, x.GetValue(this))));
    }
}
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

    public BookURLData GetURLData()
    {
        var path = string.Format("BookData/URL/{0}/{1}", type, bookNumber);
        return JsonConvert.DeserializeObject<BookURLData>(Resources.Load<TextAsset>(path).text);
        //Debug.LogFormat("{0} 책 {1}권 <b>{2}</b>의URL 찾기", type, bookNumber, title_en);
        //Debug.LogFormat(type+"책 URL 데이터 : {0}권\n{1}", GameManager.Instance.schema.data.bookData
        //    .Where(x => x.key == type).Count(),
        //    string.Join("\n", GameManager.Instance.schema.data.bookData
        //    .Where(x => x.key == type).Select(x => x.number + "권")));
        //Debug.LogFormat("{0}권 찾기 결과 : {1}권 찾음", bookNumber,
        //    GameManager.Instance.schema.data.bookData
        //    .Where(x => x.key == type)
        //    .Where(x => x.number == bookNumber).Count());
        //return GameManager.Instance.schema.data.bookData
        //    .Where(x => x.key == type)
        //    .Where(x => x.number == bookNumber)
        //    .First();
    }

    public Sprite GetSprite() => GetSpriteAsync().WaitForCompletion();
    public AsyncOperationHandle<Sprite> GetSpriteAsync() => Addressables.LoadAssetAsync<Sprite>(string.Format("Sentance/{0}/{1}/{2}", type.ToString(), bookNumber, page));
    public Sprite GetSprite(BookWordData data) => GetSpriteAsync(data).WaitForCompletion();
    public AsyncOperationHandle<Sprite> GetSpriteAsync(BookWordData data) => Addressables.LoadAssetAsync<Sprite>(string.Format("Words/{0}/{1}/{2}.{3}", type.ToString(), bookNumber, data.value, data.extension));
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
    [JsonIgnore]
    public BookMetaData currentBook { get; private set; }
    public void SetBook(BookMetaData book) => currentBook = book;
    [JsonIgnore]
    public Sprite sprite => currentBook.GetSprite();
    public AsyncOperationHandle<Sprite> spriteAsync => currentBook.GetSpriteAsync();
}
public class BookSentanceData : BaseSentanceData
{
    public string kr;
    public string en
    {
        get => value;
        set => this.value = value;
    }
    public int priority;
    public string clip_en
    {
        get => clip;
        set => clip = value;
    }
    public string clip_kr;
    [JsonIgnore]
    public BookMetaData currentBook { get; private set; }
    public void SetBook(BookMetaData book) => currentBook = book;
    [JsonIgnore]
    public Sprite sprite => currentBook.GetSprite();
    public AsyncOperationHandle<Sprite> spriteAsync => currentBook.GetSpriteAsync();
}
public class BookWordData
{
    public string value;
    public string extension;
    public string clip;
    [JsonIgnore]
    public BookMetaData currentBook { get; private set; }
    public void SetBook(BookMetaData book) => currentBook = book;
    [JsonIgnore]
    public Sprite sprite => currentBook.GetSprite(this);
    [JsonIgnore]
    public AsyncOperationHandle<Sprite> spriteAsync => currentBook.GetSpriteAsync(this);
}
public class BookCommentData
{
    public eBookLevel level;
    public eContents contents;
    public float correctRate;
    public string comment;
}
public class BookLevelData
{
    public eBookType type;
    public int bookNumber;
    public eBookLevel level;
}
#endregion

[Serializable]
public class SiteWordData : ResourceElement
{
    public string clip;
}

[Serializable]
public class CommentData
{
    public eContents id;
    public string[] comments;
    public string GetComment(int failCount)
    {
        if (failCount == 0 || comments.Length == 1)
            return comments[0];
        else if (failCount < 4)
            return comments[1];
        else
            return comments[2];
    }
}

[Serializable]
public class ResourceData
{
    private AlphabetWordsData[] _alphabetWords = null;
    private AlphabetAudioData[] _alphabetAudio = null;
    private VowelWordsData[] _vowelWords = null;
    private VowelAudioData[] _vowelAudio = null;
    private DigraphsWordsData[] _digraphsWords = null;
    private DigraphsAudioData[] _digraphsAudio = null;
    private AlphabetSentanceData[] _alphabetSentaces = null;
    private DigraphsSentanceData[] _digraphsSentances = null;
    private SiteWordData[] _siteWords = null;
    private CommentData[] _commentData = null;
    private BookCommentData[] _bookComments = null;
    private BookLevelData[] _bookLevels = null;

    public AlphabetWordsData[] alphabetWords
    {
        get
        {
            if(_alphabetWords == null)
                _alphabetWords = JArray.Parse(Resources.Load<TextAsset>("Data/alphabetWords").text).Select(x => x.ToObject<AlphabetWordsData>()).ToArray();

            return _alphabetWords;
        }
    }
    
    public AlphabetAudioData[] alphabetAudio
    {
        get
        {
            if(_alphabetAudio == null)
                _alphabetAudio = JArray.Parse(Resources.Load<TextAsset>("Data/alphabetAudio").text).Select(x => x.ToObject<AlphabetAudioData>()).ToArray();

            return _alphabetAudio;
        }
    }
    public VowelWordsData[] vowelWords
    {
        get
        {
            if(_vowelWords == null)
                _vowelWords = JArray.Parse(Resources.Load<TextAsset>("Data/vowelWords").text).Select(x => x.ToObject<VowelWordsData>()).ToArray();

            return vowelWords;
        }
    }
    public VowelAudioData[] vowelAudio
    {
        get
        {
            if(_vowelAudio == null)
                _vowelAudio = JArray.Parse(Resources.Load<TextAsset>("Data/vowelAudio").text).Select(x => x.ToObject<VowelAudioData>()).ToArray();

            return _vowelAudio;
        }
    }
    public DigraphsWordsData[] digraphsWords
    {
        get
        {
            if(_digraphsWords == null)
                _digraphsWords = JArray.Parse(Resources.Load<TextAsset>("Data/digraphsWords").text).Select(x => x.ToObject<DigraphsWordsData>()).ToArray();

            return _digraphsWords;
        }
    }
    public DigraphsAudioData[] digraphsAudio
    {
        get
        {
            if(_digraphsAudio == null)
                _digraphsAudio = JArray.Parse(Resources.Load<TextAsset>("Data/digraphsAudio").text).Select(x => x.ToObject<DigraphsAudioData>()).ToArray();

            return _digraphsAudio;
        }
    }
    public AlphabetSentanceData[] alphabetSentaces
    {
        get
        {
            if(_alphabetSentaces == null)
                _alphabetSentaces = JArray.Parse(Resources.Load<TextAsset>("Data/alphabetSentaces").text).Select(x => x.ToObject<AlphabetSentanceData>()).ToArray(); ;

            return _alphabetSentaces;
        }
    }
    public DigraphsSentanceData[] digraphsSentances
    {
        get
        {
            if(_digraphsSentances == null)
                _digraphsSentances = JArray.Parse(Resources.Load<TextAsset>("Data/digraphsSentances").text).Select(x => x.ToObject<DigraphsSentanceData>()).ToArray();

            return _digraphsSentances;
        }
    }
    public SiteWordData[] siteWords
    {
        get
        {
            if(_siteWords == null)
                _siteWords = JArray.Parse(Resources.Load<TextAsset>("Data/siteWords").text).Select(x => x.ToObject<SiteWordData>()).ToArray();

            return _siteWords;
        }
    }
    public CommentData[] commentData
    {
        get
        {
            if(_commentData == null)
                _commentData = JArray.Parse(Resources.Load<TextAsset>("Data/commentData").text).Select(x => x.ToObject<CommentData>()).ToArray();

            return _commentData;
        }
    }
    public BookCommentData[] bookComments
    {
        get
        {
            if(_bookComments == null)
                _bookComments = JArray.Parse(Resources.Load<TextAsset>("Data/bookComments").text).Select(x => x.ToObject<BookCommentData>()).ToArray();

            return _bookComments;
        }
    }
    public BookLevelData[] bookLevels
    {
        get
        {
            if(_bookLevels == null)
                _bookLevels = JArray.Parse(Resources.Load<TextAsset>("Data/bookLevels").text).Select(x => x.ToObject<BookLevelData>()).ToArray();
            return _bookLevels;
        }
    }
    public string[] inCorrectClips;
    public string[] correctPerfectClip;
    public string[] correctGreatClip;
    public string[] correctWonderfulClip;
    public string[] correctExcellentClip;
    public string[] correctGoodjobClip;
    public string[] correctAmazingClip;
    public string[] correctNiceClip;
}

