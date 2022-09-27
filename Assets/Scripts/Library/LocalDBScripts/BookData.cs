using GJGameLibrary.DesignPattern;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
public enum eBookType
{
    LD
}
public class BookData
{
    private static BookData instance = null;
    [JsonIgnore]
    public static BookData Instance
    {
        get
        {
            if(instance == null)
            {
                var text = Resources.Load<TextAsset>("BookData").text;
                var json = JObject.Parse(text);

                instance = new BookData();

                instance.conversations = JArray.FromObject(json.SelectToken("conversations")).Select(x => x.ToObject<BookConversationData>()).ToArray();
                instance.sentances = JArray.FromObject(json.SelectToken("sentances")).Select(x => x.ToObject<BookSentanceData>()).ToArray();
                instance.bookWords = JArray.FromObject(json.SelectToken("bookWords")).Select(x => x.ToObject<BookWordData>()).ToArray();
            }
            return instance;
        }
    }
    public BookConversationData[] conversations { get; private set; }
    public BookSentanceData[] sentances { get; private set; }
    public BookWordData[] bookWords { get; private set; }

    [JsonIgnore]
    public Dictionary<eBookType, BookConversationData[]> ConverSationData => GetMultipleDictionary(conversations);

    [JsonIgnore]
    public Dictionary<eBookType, BookSentanceData[]> SentanceData => GetMultipleDictionary(sentances);

    private Dictionary<eBookType, TValue[]> GetMultipleDictionary<TValue>(TValue[] values) where TValue:BookDataElement
    {
        return values
            .Select(x => x.type)
            .Distinct()
            .ToDictionary(x => x, x => values.Where(y => y.type == x).ToArray());
    }
}

public abstract class BookDataElement
{
    public string key;
    [JsonIgnore]
    public eBookType type => (eBookType)Enum.Parse(typeof(eBookType), key);
    public string name;
    public string number;
    public int page;
    public int priority;
    public string clip;
    [JsonIgnore]
    public AudioClip Clip => Addressables.LoadAssetAsync<AudioClip>(clip).WaitForCompletion();
}
public class BookConversationData : BookDataElement
{
    public string speaker;
    public string value;
}
public class BookSentanceData : BookDataElement
{
    public string value_kr;
    public string value_en;
}
public class BookWordData
{
    public eBookType type;
    public int bookNumber;
    public string value;
    [JsonIgnore]
    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(value).WaitForCompletion();
}

