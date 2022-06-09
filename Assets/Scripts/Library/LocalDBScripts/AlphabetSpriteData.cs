using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Alphabet Sprite Data.asset", menuName = "LocalDB/Element/Alphabet Sprite Data")]
public class AlphabetSpriteData : LocalDBElement
{
    [Serializable]
    public class AlphabetSpritePair
    {
        [SerializeField]
        private Sprite[] upper;
        [SerializeField]
        private Sprite[] lower;
        public Sprite Get(eAlphabetType type, eAlphabet alhpabet) => Get(type)[(int)alhpabet];
        public Sprite[] Get(eAlphabetType type)
        {
            Sprite[] target;
            if (type == eAlphabetType.Lower)
                target = lower;
            else
                target = upper;

            if (target == null)
            {
                if (upper == null)
                    return upper;
                else
                    return lower;
            }
            return target;
        }
    }

    public override bool Loadable => false;
    [SerializeField]
    private SerializableDictionaryBase<eAlphabetStyle, AlphabetSpritePair> sprites;
    public AlphabetSpritePair Get(eAlphabetStyle style) => sprites[style];
    public Sprite[] Get(eAlphabetStyle style, eAlphabetType type) => Get(style).Get(type);
    public Sprite Get(eAlphabetStyle style, eAlphabetType type, eAlphabet alphabet) => Get(style).Get(type, alphabet);

    public override void Load(List<Hashtable> data) { }
}
