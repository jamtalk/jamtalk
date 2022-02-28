using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.IO;

[CreateAssetMenu(fileName = "AssetImporter.asset", menuName = "Assets Importer")]
public class AssetImporter : ScriptableObject
{
    [Header("Auido")]
    [SerializeField]
    private AudioClip[] clipAlphabet;
    [SerializeField]
    private AudioClip[] clipPhanics;
    [SerializeField]
    private AudioClip[] clipAlphabetAct1;
    [SerializeField]
    private AudioClip[] clipAlphabetAct2;
    [SerializeField]
    private AudioClip[] clipAct3;
    [SerializeField]
    private AudioClip[] clipWords;
    [SerializeField]
    private AudioClip clipCorrect;

    [Header("Sprites")]
    [SerializeField]
    private Sprite[] spriteAlphabetCard;
    [SerializeField]
    private Sprite[] spriteAlphabetBrownUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetBrownLower;
    [SerializeField]
    private Sprite[] spriteAlphabetNeonYellowUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetNeonYellowLower;
    [SerializeField]
    private Sprite[] spriteAlphabetNeonRedUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetNeonRedLower;
    [SerializeField]
    private Sprite[] spriteAlphabetFullColorUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetFullColorLower;
    [SerializeField]
    private Sprite[] spriteAlphabetDinoUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetFullColorCardUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetFullColorCardLower;
    [SerializeField]
    private Sprite[] spriteAlphabetBingoRedUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetBingoRedLower;
    [SerializeField]
    private Sprite[] spriteAlphabetBingoBlueUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetBingoBlueLower;
    [SerializeField]
    private Sprite[] spriteAlphabetYellowUpper;
    [SerializeField]
    private Sprite[] spriteAlphabetYellowLower;
    [SerializeField]
    private Sprite[] spriteAlphabetWhite;
    [SerializeField]
    private Sprite[] spriteWords;

    [SerializeField]
    private SerializableDictionaryBase<string, string> clipWordMetadata = new SerializableDictionaryBase<string, string>();
    [SerializeField]
    private SerializableDictionaryBase<string, string> clipAct3Metadata = new SerializableDictionaryBase<string, string>();
    //Audio Getter
    public AudioClip GetClipAlphabet(eAlphabet alphabet) => clipAlphabet[(int)alphabet];
    public AudioClip GetClipPhanics(eAlphabet alphabet) => clipPhanics[(int)alphabet];
    public AudioClip GetClipAct1(eAlphabet alphabet) => clipAlphabetAct1[(int)alphabet];
    public AudioClip GetClipAct2(eAlphabet alphabet) => clipAlphabetAct2[(int)alphabet];
    public AudioClip GetClipAct3(string word)
    {
        var key = int.Parse(clipAct3Metadata[word].Split('-').Last());
        return clipAct3.ToList().Find(x => int.Parse(x.name.Split('-').Last()) == key);
    }
    public AudioClip GetClipWord(string word) => clipWords.Where(x => x.name == clipWordMetadata[word]).First();
    public AudioClip GetClipCorrectEffect() => clipCorrect;

    //SpriteGetter
    public Sprite[] GetSpriteAlphabet(eAlphbetStyle contents, eAlphbetType type)
    {
        switch (contents)
        {
            case eAlphbetStyle.Card:
                return spriteAlphabetCard;
            case eAlphbetStyle.Brown:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetBrownUpper;
                else
                    return spriteAlphabetBrownLower;
            case eAlphbetStyle.NeonYellow:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetNeonYellowUpper;
                else
                    return spriteAlphabetNeonYellowLower;
            case eAlphbetStyle.NeonRed:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetNeonRedUpper;
                else
                    return spriteAlphabetNeonRedLower;
            case eAlphbetStyle.FullColor:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetFullColorUpper;
                else
                    return spriteAlphabetFullColorLower;
            case eAlphbetStyle.Dino:
                return spriteAlphabetDinoUpper;
            case eAlphbetStyle.FullColorCard:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetFullColorCardUpper;
                else
                    return spriteAlphabetFullColorCardLower;
            case eAlphbetStyle.BingoRed:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetBingoRedUpper;
                else
                    return spriteAlphabetBingoRedLower;
            case eAlphbetStyle.BingoBlue:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetBingoBlueUpper;
                else
                    return spriteAlphabetBingoBlueLower;
            case eAlphbetStyle.Yellow:
                if (type == eAlphbetType.Upper)
                    return spriteAlphabetYellowUpper;
                else
                    return spriteAlphabetYellowLower;
            case eAlphbetStyle.White:
                return spriteAlphabetWhite;
            default:
                return null;
        }
    }
    public Sprite GetSpriteAlphabet(eAlphbetStyle style, eAlphbetType type, eAlphabet alphabet) => GetSpriteAlphabet(style, type)[(int)alphabet];
    public Sprite[] GetSpriteWord(eAlphabet alphbet) => spriteWords.Where(x => x.name.First().ToString().ToUpper() == alphbet.ToString().ToUpper()).ToArray();
    public Sprite[] GetSpriteWord() => spriteWords;
    public Sprite GetSpriteWord(string word) => spriteWords.ToList().Find(x => x.name == word);

    //StringGetter
    public string[] GetWords() => clipWordMetadata.Keys.ToArray().Where(x=>spriteWords.Select(y=>y.name).Contains(x)).ToArray();

    private void Awake()
    {
        //clipWordMetadata.Clear();
        //clipAct3Metadata.Clear();
        //var words = Resources.Load<TextAsset>("Words").text.Split('\n')
        //    .Where(x => !string.IsNullOrEmpty(x))
        //    .Select(x => x.Split(','))
        //    .Select(x=>x.ToArray());
        //foreach(var data in words)
        //{
        //    clipWordMetadata.Add(data[0], data[1].Replace(" ", ""));
        //    clipAct3Metadata.Add(data[0], data[2].Replace(" ", ""));
        //}
    }
    //private void OnEnable()
    //{
    //    var words = Resources.Load<TextAsset>("Words").text.Split('\n')
    //        .Where(x => !string.IsNullOrEmpty(x))
    //        .Select(x => x.Split(','))
    //        .Select(x => x.ToArray());
    //    clipWordMetadata.Clear();
    //    clipAct3Metadata.Clear();
    //    foreach (var data in words)
    //    {
    //        Debug.Log(string.Join(",", data));
    //        clipWordMetadata.Add(data[0], data[1].Replace(" ", ""));
    //        clipAct3Metadata.Add(data[0], data[2].Replace(" ", ""));
    //    }
    //}
}
