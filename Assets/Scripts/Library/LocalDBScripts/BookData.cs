using GJGameLibrary.DesignPattern;
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
public class BookData : Singleton<BookData>
{
    private BookConversationData[] conversations;
    private BookSentanceData[] sentances;
    public Dictionary<eBookType, BookConversationData[]> ConverSationData;
    public Dictionary<eBookType, BookSentanceData[]> SentanceData;
    public bool Initialized { get; private set; } = false;
    public override void Initialize()
    {
        if (!Initialized)
        {
            conversations = JObject.Parse(Resources.Load<TextAsset>("").text).ToObject<BookConversationData[]>();
            ConverSationData = GetMultipleDictionary(conversations);

            sentances = JObject.Parse(Resources.Load<TextAsset>("").text).ToObject<BookSentanceData[]>();
            SentanceData = GetMultipleDictionary(sentances);

            Initialized = true;
            base.Initialize();
        }
        else
            return;
    }

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
    public eBookType type => (eBookType)Enum.Parse(typeof(eBookType), key);
    public string name;
    public string number;
    public int page;
    public int priority;
    public string clip;
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

