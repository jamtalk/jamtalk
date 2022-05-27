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
    private class AlphabetAudioSource
    {
        public AudioClip clip;
        public AudioClip phanics;
        public AudioClip act1;
        public AudioClip act2;

        public AlphabetAudioSource(AudioClip clip, AudioClip phanics, AudioClip act1, AudioClip act2)
        {
            this.clip = clip;
            this.phanics = phanics;
            this.act1 = act1;
            this.act2 = act2;
        }
    }

    [SerializeField]
    private SerializableDictionaryBase<eAlphabet, AlphabetAudioSource> data;

    [Header("Orizinal Data")]
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip[] phanicses;
    [SerializeField]
    private AudioClip[] acts1;
    [SerializeField]
    private AudioClip[] acts2;

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
                LocalDB.Find(clips, clip),
                LocalDB.Find(phanicses, phanics),
                LocalDB.Find(acts1, act1),
                LocalDB.Find(acts2, act2)
                ));
        }

        this.data.CopyFrom(tmp);
    }
}
