using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Vowel Audio Data.asset", menuName = "LocalDB/Element/Vowel Audio Data")]
public class VowelAudioData : LocalDBElement
{
    [Serializable]
    public class VowelAudioSource
    {
        public eAlphabet alphabet;
        public string clip;
        public string phanics_long;
        public string phanics_short;

        public VowelAudioSource(eAlphabet alphabet, string clip, string phanics_long, string phanics_short)
        {
            this.alphabet = alphabet;
            this.clip = clip;
            this.phanics_long = phanics_long;
            this.phanics_short = phanics_short;
        }
        public string GetPhanics(eVowelType type)
        {
            if (type == eVowelType.Short)
                return phanics_short;
            else
                return phanics_long;
        }
    }
    [SerializeField]
    private SerializableDictionaryBase<eAlphabet, VowelAudioSource> data;
    public VowelAudioSource Get(eAlphabet alphabet) => data[alphabet];

    public override void Load(List<Hashtable> data)
    {
        var tmp = new Dictionary<eAlphabet, VowelAudioSource>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];

            var key = (eAlphabet)Enum.Parse(typeof(eAlphabet), datas["key"].ToString());
            var clip = datas["clip"].ToString();
            var phanics_long = datas["phanics_long"].ToString();
            var phanics_short = datas["phanics_short"].ToString();
            tmp.Add(key, new VowelAudioSource(
                key,
                clip,
                phanics_long,
                phanics_short
                ));
        }

        this.data.CopyFrom(tmp);
    }
}
