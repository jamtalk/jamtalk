﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "VowelData.asset", menuName = "LocalDB/Element/Vowel Data")]
public class VowelData : LocalDBElement
{
    [SerializeField]
    private VowelSource[] data;
    public VowelSource[] Get() => data;
    public VowelSource[] Get(eAlphabet alphabet) => data.Where(x => x.alphabet == alphabet).ToArray();
    public VowelSource Get(eAlphabet alphabet, string word) => Get(alphabet).ToList().Find(x => x.value == word);

    public override void Load(List<Hashtable> data)
    {
        var tmp = new List<VowelSource>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];
            var value = datas["key"].ToString();
            var type = (eVowelType)Enum.Parse(typeof(eVowelType), datas["type"].ToString());
            var actValue = datas["actValue"].ToString();
            var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), datas["alphabet"].ToString());
            tmp.Add(new VowelSource(
                type,
                alphabet,
                value,
                actValue
                ));
        }
        this.data = tmp
            .Where(x => !x.IsNull)
            .ToArray();
    }
}

[Serializable]
public class VowelSource : DataSource
{
    protected override eAtlasType atlas => eAtlasType.Vowels;
    public eVowelType type;
    public eAlphabet alphabet;
    public string actValue;
    public void PlayClip() => AndroidPluginManager.Instance.PlayTTS(value);
    public void PlayAct() => AndroidPluginManager.Instance.PlayTTS(actValue);
    public override bool IsNull => base.IsNull || string.IsNullOrEmpty(actValue);

    public override bool Equals(object obj)
    {
        return obj is VowelSource source &&
               type == source.type &&
               alphabet == source.alphabet &&
               value == source.value;
    }

    public override int GetHashCode()
    {
        var hashCode = -251028527;
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        hashCode = hashCode * -1521134295 + alphabet.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(value);
        return hashCode;
    }

    public VowelSource(eVowelType type, eAlphabet alphabet, string value, string act) : base(value)
    {
        this.type = type;
        this.alphabet = alphabet;
        this.actValue = act;
    }


}