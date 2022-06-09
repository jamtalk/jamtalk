using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Sentance Data.asset", menuName = "LocalDB/Element/Sentance Data")]
public class SentanceData : LocalDBElement
{
    [Serializable]
    public class SentancesSource
    {
        public eAlphabet key;
        public string value;
        public string[] words;
        public SentancesSource(eAlphabet key, string value)
        {
            this.key = key;
            this.value = value;
            words = value.Split(' ');
        }
    }

    [SerializeField]
    private SentancesSource[] data;
    public SentancesSource[] Get(eAlphabet alphabet) => data.Where(x=>x.key == alphabet).ToArray();
    public SentancesSource[] Get() => data;

    public override void Load(List<Hashtable> data)
    {
        var tmp = new List<SentancesSource>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];

            var key = (eAlphabet)Enum.Parse(typeof(eAlphabet), datas["key"].ToString());
            var value = datas["value"].ToString();
            tmp.Add(new SentancesSource(key, value));
        }
        this.data = tmp.ToArray();
    }
}
