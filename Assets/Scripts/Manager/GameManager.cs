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
    private const string AlhpabetSpritePath = "{0}/{1}/{2}";
    public eAlphabet currentAlphabet { get; set; } = eAlphabet.A;
    public eContents currentContents { get; set; } = eContents.JT_PL1_102;
    public eDigraphs currentDigrpahs { get; set; } = eDigraphs.AI;
    public eVowelType currentVowel { get; set; } = eVowelType.Short;
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
            Debug.LogFormat("스키마 로딩 시작 ({0}MB)", bytes);
            while (!schemaLoader.IsDone)
            {
                yield return null;
                var bytesSatus = schemaLoader.GetDownloadStatus();
                Debug.LogFormat("로딩중 ({0}/{1})", bytesSatus.DownloadedBytes, bytesSatus.TotalBytes);
            }
            Debug.LogFormat("스키마 로딩 결과\n상태 : {0}\n오류 : {1}\n결과 : {2}", schemaLoader.Status, schemaLoader.OperationException, schemaLoader.Result);
            _schema = schemaLoader.Result;
        }

        callback?.Invoke();
    }
    public AudioClip GetClipCorrectEffect() => schema.correctSound;
    public AlphabetData GetResources(eAlphabet alphabet) => new AlphabetData(alphabet);
    public AlphabetData GetResources() => GetResources(currentAlphabet);
    public DigraphsWordsData[] GetDigraphs(eDigraphs type) => schema.data.digraphsWords.Where(x => x.digraphs == type.ToString()).ToArray();
    public DigraphsWordsData[] GetDigraphs() => GetDigraphs(currentDigrpahs);
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

}

public class AlphabetData
{
    public eAlphabet Alphabet { get; private set; }
    public bool IsVowel => Vowels == null || Vowels.Length == 0;
    public AlphabetSentanceData[] Sentances => GameManager.Instance.schema.data.alphabetSentaces.Where(x => x.Alphabet == Alphabet).ToArray();
    public AlphabetWordsData[] Words => GameManager.Instance.schema.data.alphabetWords.Where(x => x.Alphabet == Alphabet).ToArray();
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
