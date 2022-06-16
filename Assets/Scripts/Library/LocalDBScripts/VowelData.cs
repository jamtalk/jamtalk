using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "VowelData.asset", menuName = "LocalDB/Element/Vowel Data")]
public class VowelData : LocalDBElement
{
    [Serializable]
    public class VowelSource
    {
        public eVowelType type;
        public eAlphabet alphabet;
        public string value;
        public Sprite sprite;
        public AudioClip clip;
        public AudioClip act3;

        public VowelSource(eVowelType type, eAlphabet alphabet, string value, Sprite sprite, AudioClip clip, AudioClip act3)
        {
            this.type = type;
            this.alphabet = alphabet;
            this.value = value;
            this.sprite = sprite;
            this.clip = clip;
            this.act3 = act3;
        }

        public bool IsNull => string.IsNullOrEmpty(value) ||
            sprite == null ||
            clip == null ||
            act3 == null;

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
    }
    [SerializeField]
    private VowelSource[] data;
    public VowelSource[] Get() => data;
    public VowelSource[] Get(eAlphabet alphabet) => data.Where(x => x.alphabet == alphabet).ToArray();
    public VowelSource Get(eAlphabet alphabet, string word) => Get(alphabet).ToList().Find(x => x.value == word);
    [Header("Orizinal Data")]
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip[] acts;

    public override void Load(List<Hashtable> data)
    {
        int current = sprites.Length;
        sprites = sprites.Distinct().ToArray();
        var tmp = new List<VowelSource>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];
            var clip = datas["clip"].ToString();
            var act = datas["act"].ToString();
            var value = datas["key"].ToString();
            var type = (eVowelType)Enum.Parse(typeof(eVowelType), datas["type"].ToString());
            var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), datas["alphabet"].ToString());
            tmp.Add(new VowelSource(
                type,
                alphabet,
                value,
                LocalDB.Find(sprites, value),
                LocalDB.Find(clips, clip),
                LocalDB.Find(acts, act)
                ));
        }
        this.data = tmp
            //.Where(x => !x.IsNull)
            .ToArray();

        var spritesNames = sprites.Select(x => x.name);
        var dataNames = this.data.Select(x => x.value);
        var missingAudiosAll = "음성 전체 누락 단어목록\n" + string.Join("\n", spritesNames.Where(x => !dataNames.Contains(x)));
        var duplicated = "중복 이미지 목록\n" + string.Join("\n", sprites.Where(x => sprites.Where(y => y.name == x.name).Count() > 1).Select(x => x.name));
        var missings = "누락 항목\n" + string.Join("\n", tmp.Where(x => x.IsNull).Select(x =>
        {
            string missingItem;
            if (x.sprite == null)
                missingItem = "Sprite";
            else if (x.clip == null)
                missingItem = "Audio";
            else if (x.act3 == null)
                missingItem = "Act";
            else
                missingItem = "UnKnwon";

            return string.Format("{0} Missing : {1}", x.value, missingItem);
        }));
        Debug.Log(duplicated);
        Debug.Log(missingAudiosAll);
        Debug.Log(missings);
    }
    #region Methods
    #endregion
}
