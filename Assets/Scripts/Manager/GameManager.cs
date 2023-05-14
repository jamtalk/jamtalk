using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GJGameLibrary;
using GJGameLibrary.DesignPattern;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoSingleton<GameManager>
{
    private const string AlhpabetSpritePath = "{0}/{1}/{2}";
    public eAlphabet currentAlphabet { get; set; } = eAlphabet.A;
    public eContents currentContents { get; set; } = eContents.JT_PL1_102;
    public eDigraphs currentDigrpahs { get; set; } = eDigraphs.AI;
    public eVowelType currentVowel { get; set; } = eVowelType.Short;
    public eBookType currentBook { get; set; } = eBookType.LD;
    public int currentBookNumber { get; set; } = 1;
    public int currentPage { get; set; } = 0;
    private ResourceSchema _schema = null;
    public ResourceSchema schema
    {
        get
        {
            if (_schema == null)
                _schema = Addressables.LoadAssetAsync<ResourceSchema>("ResourceSchema").WaitForCompletion();

            return _schema;
        }
        private set { _schema = value; }
    }
    public override void Initialize()
    {
        base.Initialize();
        currentAlphabet = eAlphabet.A;
        currentContents = eContents.JT_PL1_102;
    }

    public void LoadAddressables(Action<float> callbackProgress, Action onCompleted)
    {
        StartCoroutine(LoadResources("", callbackProgress, onCompleted));
    }
    private IEnumerator LoadResources(string label, Action<float> progress, Action callback)
    {
        var op = Addressables.LoadResourceLocationsAsync(label);
        while (!op.IsDone)
        {
            Debug.Log("로딩 대기중");
            yield return null;
        }
        var locations = op.Result;
        var cached = Addressables.GetDownloadSizeAsync(locations);
        Debug.LogFormat("로딩완료\n{0}\n{1}", string.Join("\n", locations.Select(x => string.Format("{0} {1} {2}", x.ResourceType, x.ProviderId, x.InternalId))), cached.Result / 1024f / 1024f);
        while (!cached.IsDone) { yield return null; }
        if (cached.Result > 0)
        {
            var mb = cached.Result / 1024f / 1024f;
            mb = Mathf.Round(mb * 100f) / 100f;
            Debug.LogFormat("추가 다운로드해야할 사이즈 : {0}", mb);
            bool isChecked = false;
            while (!isChecked) { yield return null; }

            var download = Addressables.DownloadDependenciesAsync(locations);
            while (!download.IsDone)
            {
                var status = download.GetDownloadStatus();
                float current = status.DownloadedBytes;
                float total = status.TotalBytes;
                float per = current == 0 || total == 0 ? 0 : current / total * 100f;
                progress?.Invoke(per / 100f);
                yield return null;
            }
        }
        Debug.Log("완료");
        callback?.Invoke();
    }
    public void Initialize(Action callback)
    {
        StartCoroutine(Initializing(callback));
    }
    private IEnumerator Initializing(Action callback=null)
    {
        if (schema != null)
        {
            var schemaLoader = Addressables.LoadAssetAsync<ResourceSchema>("ResourceSchema");
            var bytes = schemaLoader.GetDownloadStatus().TotalBytes;
            while (!schemaLoader.IsDone)
            {
                yield return null;
                var bytesSatus = schemaLoader.GetDownloadStatus();
            }
            _schema = schemaLoader.Result;
        }

        callback?.Invoke();
    }
    public AudioClip GetClipCorrectEffect() => schema.correctSound;
    public AlphabetData GetResources(eAlphabet alphabet) => new AlphabetData(alphabet);
    public AlphabetData GetResources() => GetResources(currentAlphabet);
    public Dictionary<string,string> GetClips()
    {
        return schema.data.digraphsWords.Select(x => new KeyValuePair<string, string>(x.clip, x.key))
            .Union(schema.data.alphabetWords.Select(x => new KeyValuePair<string, string>(x.clip, x.key)))
            .Union(schema.data.vowelWords.Select(x => new KeyValuePair<string, string>(x.clip, x.key)))
            .ToDictionary(x => x.Key, x => x.Value);
    }
    public DigraphsWordsData[] GetDigraphs(eDigraphs type) => schema.data.digraphsWords.Where(x => x.digraphs == type.ToString()).ToArray();
    public DigraphsWordsData[] GetDigraphs() => GetDigraphs(currentDigrpahs);
    public DigraphsSentanceData[] GetDigraphsSentance(eDigraphs type) => schema.data.digraphsSentances.Where(x => x.Key == type).ToArray();
    public DigraphsSentanceData[] GetDigraphsSentance() => schema.data.digraphsSentances.Where(x => x.Key == currentDigrpahs).ToArray();
    public BookMetaData[] GetBookMetaData(eBookType type, int bookNumber) => schema.GetBookData(type, bookNumber);
    public BookMetaData[] GetCurrentBook() => GetBookMetaData(currentBook, currentBookNumber);
    public BookMetaData[] GetIncorrectBooks() => schema.bookData.Where(x => x.type != currentBook).ToArray();
    public BookWordData[] GetBookWords(eBookType type, int bookNumber) => schema.GetBookData(type, bookNumber).SelectMany(x => x.words).Distinct().ToArray();
    public BookWordData[] GetCurrentBookWords() => GetBookWords(currentBook, currentBookNumber);
    public BookWordData[] GetIncorrectBookWords() => schema.bookData.Where(x => x.type != currentBook && x.bookNumber != currentBookNumber).SelectMany(x => x.words).ToArray();
    public Sprite GetAlphbetSprite(eAlphabetStyle style, eAlphabetType type, eAlphabet alphabet) => Addressables.LoadAssetAsync<Sprite>(string.Format(AlhpabetSpritePath, style, type, alphabet)).WaitForCompletion();
    public eAlphabet[] alphabets => Enum.GetNames(typeof(eAlphabet)).Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x)).ToArray();
    public eAlphabet[] vowels => new eAlphabet[] { eAlphabet.A, eAlphabet.E, eAlphabet.I, eAlphabet.O, eAlphabet.U };
    public eDigraphs[] digrpahs => Enum.GetNames(typeof(eDigraphs))
        .Select(x => (eDigraphs)Enum.Parse(typeof(eDigraphs), x))
        .ToArray();
    public Vector3 GetMousePosition(float z = 0)
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = z;
        return pos;
    }

    public void SignInSNS(eProvider provider, Action<string,string,eProvider,string> callback)
    {
        if (provider == eProvider.kakao)
            new KakaoSigner().SignInSNS(callback);
        else if (provider == eProvider.naver)
            new NaverSigner().SignInSNS(callback);
        else if (provider == eProvider.facebook)
            new FacebookSigner().SignInSNS(callback);
    }

    public void SignIn(string id, string pw, eProvider provider, string uid, Action onCompleted = null)
    {
        if (string.IsNullOrEmpty(id))
            AndroidPluginManager.Instance.Toast("아이디를 입력하세요.");
        else if (string.IsNullOrEmpty(pw))
            AndroidPluginManager.Instance.Toast("비밀번호를 입력하세요.");
        else
        {
            var param = new SignInParam(id, pw, provider, uid);
            RequestManager.Instance.Request(param, (res) =>
            {
                var result = res.GetResult<ActRequestResult>();
                if (result.code != eErrorCode.Success)
                {
                    AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
                }
                else
                {
                    if (provider == eProvider.none)
                    { 
                        var isEmail = id.Contains("email:");
                        if (!isEmail) id = "email:" + id;
                    }
                    UserDataManager.Instance.LoadUserData(id, () =>
                    {
                        GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);;
                    });
                }
                onCompleted?.Invoke();
            });
        }
    }

    public void SignOut()
    {
        eProvider provider = UserDataManager.Instance.UserProvider;

        if (provider == eProvider.kakao)
            new KakaoSigner().SignOutSNS();
        else if (provider == eProvider.naver)
            new NaverSigner().SignOutSNS();
        else if (provider == eProvider.facebook)
            new FacebookSigner().SignOutSNS();

        if (PlayerPrefs.HasKey("PW")) PlayerPrefs.DeleteKey("PW");
        PlayerPrefs.Save();

        GJSceneLoader.Instance.LoadScene(eSceneName.AD_002);
    }
}
public class AlphabetData
{
    public eAlphabet Alphabet { get; private set; }
    public bool IsVowel => Vowels == null || Vowels.Length == 0;
    public AlphabetSentanceData[] AlphabetSentances => GameManager.Instance.schema.data.alphabetSentaces.Where(x => x.Key == Alphabet).ToArray();
    public AlphabetWordsData[] Words => GameManager.Instance.schema.data.alphabetWords.Where(x => x.Key == Alphabet).ToArray();
    public VowelWordsData[] Vowels => GameManager.Instance.schema.data.vowelWords.Where(x => x.Vowel == Alphabet).ToArray();

    public AlphabetAudioData AudioData => GameManager.Instance.schema.GetAlphabetAudio(Alphabet);
    public VowelAudioData VowelAudioData => GameManager.Instance.schema.GetVowelAudio(Alphabet);
    public AlphabetData(eAlphabet alphabet)
    {
        Alphabet = alphabet;
    }
    public Sprite Get(eAlphabetStyle style, eAlphabetType type) => GameManager.Instance.GetAlphbetSprite(style, type, Alphabet);
}


//[Serializable]
//public class DigraphsWordsData
//{
//    public eDigraphs type;
//    public bool IsPair() => IsPair(type);
//    public ePairDigraphs GetPair() => GetPair(type);
//    public static bool IsPair(eDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(ePairDigraphs))
//            .Select(x => (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        return pairs.Contains(num);
//    }
//    public static bool IsPair(ePairDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(eDigraphs))
//            .Select(x => (eDigraphs)Enum.Parse(typeof(eDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        return pairs.Contains(num);
//    }
//    public static ePairDigraphs GetPair(eDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(ePairDigraphs))
//            .Select(x => (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        if (pairs.Contains(num))
//            return (ePairDigraphs)num;
//        else
//            return 0;
//    }
//    public static eDigraphs GetPair(ePairDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(eDigraphs))
//            .Select(x => (eDigraphs)Enum.Parse(typeof(eDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        if (pairs.Contains(num))
//            return (eDigraphs)num;
//        else
//            return 0;
//    }
//    private DigraphsAudioData digrpahsAudio => GameManager.Instance.Schema.GetDigrpahsAudio(type);
//    private DigraphsAudioData pairDigraphsAudio => GameManager.Instance.Schema.GetDigrpahsAudio(GetPair());
//    public string act;
//    public string clip;

//    public DigraphsWordsData(eDigraphs type, string value, string act, string clip, int targetLevel) : base(value)
//    {
//        this.type = type;
//        this.act = act;
//        this.clip = clip;
//        TargetLevel = targetLevel;
//    }

//    public int TargetLevel { get; private set; }

//    public override bool Equals(object obj)
//    {
//        return obj is DigraphsWordsData source &&
//               type == source.type &&
//               value == source.value;
//    }

//    public override int GetHashCode()
//    {
//        var hashCode = 1148455455;
//        hashCode = hashCode * -1521134295 + type.GetHashCode();
//        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(value);
//        return hashCode;
//    } 
//}
