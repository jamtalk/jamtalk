﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WordsData.asset", menuName = "LocalDB/Element/Word Data")]
public class WordsData : LocalDBElement
{
    [Serializable]
    private class WordSources
    {
        public eAlphabet alphabet;
        public string value;
        public Sprite sprite;
        public AudioClip clip;
        public AudioClip act;
        public bool IsNull => string.IsNullOrEmpty(value) ||
            sprite == null ||
            clip == null ||
            act == null;

        public WordSources(eAlphabet alphabet, string word, Sprite image, AudioClip clip, AudioClip act)
        {
            this.alphabet = alphabet;
            this.value = word;
            this.sprite = image;
            this.clip = clip;
            this.act = act;
        }
    }
    [SerializeField]
    private WordSources[] data;

    [Header("Orizinal Data")]
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private AudioClip[] acts;

    public override void Load(List<Hashtable> data)
    {
        var tmp = new List<WordSources>();
        for (int i = 0; i < data.Count; i++)
        {
            var datas = data[i];
            var clip = datas["clip"].ToString();
            var act = datas["act"].ToString();
            var value = datas["key"].ToString();
            var alphabet = (eAlphabet)Enum.Parse(typeof(eAlphabet), datas["alhpabet"].ToString());
            tmp.Add(new WordSources(
                alphabet,
                value,
                LocalDB.Find(sprites, value),
                LocalDB.Find(clips, clip),
                LocalDB.Find(acts, act)
                ));
        }
        this.data = tmp.ToArray();

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
               else if (x.act == null)
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