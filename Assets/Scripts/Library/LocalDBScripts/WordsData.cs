using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "WordsData.asset", menuName = "LocalDB/Element/Word Data")]
public class WordsData : LocalDBElement<WordSource>
{
    public WordSource[] Get(eAlphabet alphabet) => data.Where(x => x.alphabet == alphabet).ToArray();
    public WordSource Get(eAlphabet alphabet, string word) => Get(alphabet).ToList().Find(x => x.value == word);
    [Header("Orizinal Data")]
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip[] acts;

    public override void Load(List<Hashtable> data)
    {
        var tmp = new List<WordSource>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];
            var clip = datas["clip"].ToString();
            var act = datas["act"].ToString();
            var value = datas["key"].ToString();
            var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), datas["alphabet"].ToString());
            tmp.Add(new WordSource(
                alphabet,
                value,
                LocalDB.Find(clips, clip),
                LocalDB.Find(acts, act)
                ));
        }
        this.data = tmp.Where(x=>!x.IsNull).ToArray();
    }
}

[Serializable]
public class WordSource : DataSource
{
    protected override eAtlasType atlas => eAtlasType.Words;
    public eAlphabet alphabet;
    public AudioClip clip;
    public AudioClip act3;
    public AlphabetAudioData.AlphabetAudioSource audio => GameManager.Instance.GetResources(alphabet).AudioData;
    public override bool IsNull => base.IsNull ||
        clip == null ||
        act3 == null;

    public WordSource(eAlphabet alphabet, string word, AudioClip clip, AudioClip act) : base(word)
    {
        this.alphabet = alphabet;
        this.clip = clip;
        this.act3 = act;
    }

    public override bool Equals(object obj)
    {
        return obj is WordSource sources &&
               alphabet == sources.alphabet &&
               value == sources.value;
    }

    public override int GetHashCode()
    {
        var hashCode = -1117737164;
        hashCode = hashCode * -1521134295 + alphabet.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(value);
        return hashCode;
    }
}