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
}
public class BookConversationData : ResourceElement
{
    [JsonIgnore]
    public eBookType type => (eBookType)Enum.Parse(typeof(eBookType), key);
    public string name;
    public int number;
    public int page;
    public int priority;
    public string clip;
    [JsonIgnore]
    public AudioClip Clip => Addressables.LoadAssetAsync<AudioClip>(clip).WaitForCompletion();
    public string speaker;
    public string value;
}
public class BookSentanceData : BaseSentanceData<eBookType>
{
    public string name;
    public int number;
    public int page;
    public int priority;
    [JsonIgnore]
    public AudioClip Clip => Addressables.LoadAssetAsync<AudioClip>(clip).WaitForCompletion();
    public string value_kr;
    public string image;
    [JsonIgnore]
    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(image).WaitForCompletion();
}
public class BookWordData : ResourceElement
{
    public eBookType type;
    public int bookNumber;
    [JsonIgnore]
    public Sprite sprite => Addressables.LoadAssetAsync<Sprite>(key).WaitForCompletion();
}

