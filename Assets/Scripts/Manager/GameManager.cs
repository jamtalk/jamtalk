using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJGameLibrary.DesignPattern;
using System;
using System.Linq;

public class GameManager : MonoSingleton<GameManager>
{
    public eAlphabet currentAlphabet { get; private set; }
    public eContents currentContents { get; private set; }
    private AssetImporter assets;
    public override void Initialize()
    {
        base.Initialize();
        currentAlphabet = eAlphabet.A;
        currentContents = eContents.JT_PL1_102;
        assets = Resources.Load<AssetImporter>("AssetImporter");
    }
    public AudioClip GetClipAlphbet(eAlphabet alphabet) => assets.GetClipAlphabet(alphabet);
    public AudioClip GetClipAlphbet()=> GetClipAlphbet(currentAlphabet);
    public AudioClip GetClipPhanics()=> assets.GetClipPhanics(currentAlphabet);
    public AudioClip GetClipAct1(eAlphabet alphabet) => assets.GetClipAct1(alphabet);
    public AudioClip GetClipAct2(eAlphabet alphabet) => assets.GetClipAct2(alphabet);
    public AudioClip GetClipAct1()=> GetClipAct1(currentAlphabet);
    public AudioClip GetClipAct2() => GetClipAct2(currentAlphabet);
    public AudioClip GetClipAct3(string word) => assets.GetClipAct3(word);
    public AudioClip GetClipWord(string word) => assets.GetClipWord(word);
    public AudioClip GetClipCorrectEffect() => assets.GetClipCorrectEffect();
    public Sprite GetAlphbetSprite(eAlphbetStyle style, eAlphbetType type, eAlphabet alphbet) => assets.GetSpriteAlphabet(style, type, alphbet);
    public Sprite GetAlphbetSprite(eAlphbetStyle style, eAlphbetType type) => GetAlphbetSprite(style, type, currentAlphabet);
    public Sprite[] GetAlphbetSprites(eAlphbetStyle style, eAlphbetType type) => assets.GetSpriteAlphabet(style, type);
    public Sprite[] GetSpriteWord(eAlphabet alphabet) => assets.GetSpriteWord(alphabet);
    public Sprite[] GetSpriteWord() => assets.GetSpriteWord(currentAlphabet);
    public Sprite GetSpriteWord(string word) => assets.GetSpriteWord(word);

    public string[] GetWords() => assets.GetWords();
    public string[] GetWords(eAlphabet alphabet) => GetWords().Where(x => x.First().ToString().ToUpper() == alphabet.ToString()).ToArray();
    public eAlphabet[] alphabets => Enum.GetNames(typeof(eAlphabet)).Select(x => (eAlphabet)Enum.Parse(typeof(eAlphabet), x)).ToArray();
}
