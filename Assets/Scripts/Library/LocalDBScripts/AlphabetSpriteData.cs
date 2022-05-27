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
    private class AlphabetSpritePair
    {
        [SerializeField]
        private Sprite[] upper;
        [SerializeField]
        private Sprite[] lower;
        public Sprite Get(eAlphabet alhpabet, eAlphbetType type) => Get(type)[(int)alhpabet];
        public Sprite[] Get(eAlphbetType type)
        {
            Sprite[] target;
            if (type == eAlphbetType.Lower)
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
    private void Awake()
    {
        sprites = new SerializableDictionaryBase<eAlphabetStyle, AlphabetSpritePair>();

        sprites.CopyFrom(
            Enum.GetNames(typeof(eAlphabetStyle))
            .Select(x => (eAlphabetStyle)Enum.Parse(typeof(eAlphabetStyle), x))
            .OrderBy(x => (int)x)
            .ToArray().ToDictionary(x => x, x => new AlphabetSpritePair())
            );

    }
    public override void Load(List<Hashtable> data) { }
}
