using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RotaryHeart.Lib.SerializableDictionary;
[CreateAssetMenu(fileName = "AlphabetPath.asset", menuName = "Alphabet Path")]
public class AlphabetPaths : ScriptableObject
{
    [SerializeField]
    private SerializableDictionaryBase<eAlphabet,SerializableDictionaryBase<int, AlphabetPath>> upper;
    [SerializeField]
    private SerializableDictionaryBase<eAlphabet, SerializableDictionaryBase<int, AlphabetPath>> lower;
    //public void Add(eAlphabet alphabet, eAlphbetType type, AlphabetPath[] path)
    //{
    //    if(type == eAlphbetType.Upper)
    //    {
    //        if (!upper.ContainsKey(alphabet))
    //            upper.Add(alphabet, new SerializableDictionaryBase<int, AlphabetPath>());

    //        for(int i = 0;i < path.Length; i++)
    //        {
    //            upper[alphabet].Add(path[i].stroke, path[i]);
    //        }
    //    }
    //    else
    //    {
    //        if (!lower.ContainsKey(alphabet))
    //            lower.Add(alphabet, new SerializableDictionaryBase<int, AlphabetPath>());

    //        for (int i = 0; i < path.Length; i++)
    //        {
    //            lower[alphabet].Add(path[i].stroke, path[i]);
    //        }
    //    }
    //}
    public AlphabetPath[] GetPath(eAlphabet alphabet, eAlphabetType type)
    {
        if (type == eAlphabetType.Upper)
            return upper[alphabet].Values.ToArray();
        else
            return lower[alphabet].Values.ToArray();
    }
}

[System.Serializable]
public class AlphabetPath 
{
    public eAlphabet alphabet;
    public eAlphabetType type;
    public int stroke;
    public Vector2[] path;

    public AlphabetPath(eAlphabet alphabet, eAlphabetType type, int stroke, Vector2[] path)
    {
        this.alphabet = alphabet;
        this.type = type;
        this.stroke = stroke;
        this.path = path;
    }
}