using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "DigraphsData.asset", menuName = "LocalDB/Element/Digraphs Data")]
public class DigraphsData : LocalDBElement<DigraphsSource>
{
    public DigraphsSource[] Get(eDigraphs type) => data.Where(x => x.type == type).ToArray();
    public DigraphsSource Get(eDigraphs type, string word) => Get(type).ToList().Find(x => x.value == word);
    public override void Load(List<Hashtable> data)
    {
        var tmp = new List<DigraphsSource>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];
            var value = datas["value"].ToString();
            var type = (eDigraphs)Enum.Parse(typeof(eDigraphs), datas["type"].ToString().ToUpper());
            var actValue = datas["actValue"].ToString();
            var level = int.Parse(datas["level"].ToString());
            tmp.Add(new DigraphsSource(
                type,
                value,
                actValue,
                level
                ));
        }
        this.data = tmp
            .Where(x => !x.IsNull)
            .ToArray();
    }
}

[Serializable]
public class DigraphsSource : DataSource
{
    protected override eAtlasType atlas => eAtlasType.Digraphs;
    public eDigraphs type;
    public bool IsPair() => IsPair(type);
    public ePairDigraphs GetPair() => GetPair(type);
    public static bool IsPair(eDigraphs digraphs)
    {
        var num = (int)digraphs;
        var pairs = Enum.GetNames(typeof(ePairDigraphs))
            .Select(x => (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), x))
            .Select(x => (int)x)
            .ToArray();
        return pairs.Contains(num);
    }
    public static ePairDigraphs GetPair(eDigraphs digraphs)
    {
        var num = (int)digraphs;
        var pairs = Enum.GetNames(typeof(ePairDigraphs))
            .Select(x => (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), x))
            .Select(x => (int)x)
            .ToArray();
        if (pairs.Contains(num))
            return (ePairDigraphs)num;
        else
            return 0;
    }
    public string actValue;
    public override bool IsNull => base.IsNull || string.IsNullOrEmpty(actValue);

    public DigraphsSource(eDigraphs type, string value, string actValue, int targetLevel) : base(value)
    {
        this.type = type;
        this.actValue = actValue;
        TargetLevel = targetLevel;
    }

    public int TargetLevel { get; private set; }

    public void PlayClip(Action onDone = null) => AndroidPluginManager.Instance.PlayTTS(value,onDone);
    public void PlayAct(Action onDone = null) => AndroidPluginManager.Instance.PlayTTS(actValue,onDone);

    public override bool Equals(object obj)
    {
        return obj is DigraphsSource source &&
               type == source.type &&
               value == source.value;
    }

    public override int GetHashCode()
    {
        var hashCode = 1148455455;
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(value);
        return hashCode;
    }


}