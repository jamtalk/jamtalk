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
    [Header("Orizinal Data")]
    [SerializeField]
    private Sprite[] sprites;
    public override void Load(List<Hashtable> data)
    {
        int current = sprites.Length;
        sprites = sprites.Distinct().ToArray();
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
                LocalDB.Find(sprites, value),
                actValue,
                level
                ));
        }
        this.data = tmp
            .Where(x => !x.IsNull)
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
}

[Serializable]
public class DigraphsSource : DataSource
{
    public eDigraphs type;
    public string actValue;
    public override bool IsNull => base.IsNull || string.IsNullOrEmpty(actValue);

    public DigraphsSource(eDigraphs type, string value, Sprite sprite, string actValue, int targetLevel) : base(value,sprite)
    {
        this.type = type;
        this.actValue = actValue;
        TargetLevel = targetLevel;
    }

    public int TargetLevel { get; private set; }
    public void PlayClip() => AndroidPluginManager.Instance.PlayTTS(value);
    public void PlayAct() => AndroidPluginManager.Instance.PlayTTS(actValue);

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