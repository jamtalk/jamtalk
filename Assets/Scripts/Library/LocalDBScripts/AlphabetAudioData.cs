using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Alphabet Audio Data.asset", menuName = "LocalDB/Element/Alphabet Audio Data")]
public class AlphabetAudioData : LocalDBElement
{
    [Serializable]
    public class AlphabetAudioSource
    {
        public eAlphabet alphabet;
        public string clip;
        public string phanics;
        public string act1;
        public string act2;

        public AlphabetAudioSource(eAlphabet alphabet, string clip, string phanics, string act1, string act2)
        {
            this.alphabet = alphabet;
            this.clip = clip;
            this.phanics = phanics;
            this.act1 = act1;
            this.act2 = act2;
        }
    }

    [SerializeField]
    private SerializableDictionaryBase<eAlphabet, AlphabetAudioSource> data;
    public AlphabetAudioSource Get(eAlphabet alphabet) => data[alphabet];

    public override void Load(List<Hashtable> data)
    {
        var tmp = new Dictionary<eAlphabet, AlphabetAudioSource>();
        for(int i = 0; i < data.Count; i++)
        {
            var datas = data[i];

            var key = (eAlphabet)Enum.Parse(typeof(eAlphabet),datas["key"].ToString());
            var clip = datas["clip"].ToString();
            var phanics = datas["phanics"].ToString();
            var act1 = datas["act1"].ToString();
            var act2 = datas["act2"].ToString();
            tmp.Add(key, new AlphabetAudioSource(
                key,
                clip,
                phanics,
                act1,
                act2
                ));
        }

        this.data.CopyFrom(tmp);
    }
}
