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
        public string actValue;
        public void PlayClip() => AndroidPluginManager.Instance.PlayTTS(value);
        public void PlayAct() => AndroidPluginManager.Instance.PlayTTS(actValue);

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

        public VowelSource(eVowelType type, eAlphabet alphabet, string value, Sprite sprite, string act)
        {
            this.type = type;
            this.alphabet = alphabet;
            this.value = value;
            this.sprite = sprite;
            this.actValue = act;
        }

        public bool IsNull => string.IsNullOrEmpty(value) ||
            string.IsNullOrEmpty(actValue) ||
            sprite == null;

    }
    [SerializeField]
    private VowelSource[] data;
    public VowelSource[] Get() => data;
    public VowelSource[] Get(eAlphabet alphabet) => data.Where(x => x.alphabet == alphabet).ToArray();
    public VowelSource Get(eAlphabet alphabet, string word) => Get(alphabet).ToList().Find(x => x.value == word);
    [Header("Orizinal Data")]
    [SerializeField]
    private Sprite[] sprites;

    public override void Load(List<Hashtable> data)
    {
        int current = sprites.Length;
        sprites = sprites.Distinct().ToArray();
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
                LocalDB.Find(sprites, value),
                actValue
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
