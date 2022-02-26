using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJGameLibrary.DesignPattern;

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
    public AudioClip GetClipAlphbet()=> assets.GetClipAlphabet(currentAlphabet);
    public AudioClip GetClipPhanics()=> assets.GetClipPhanics(currentAlphabet);
    public AudioClip GetClipAct1()=> assets.GetClipAct1(currentAlphabet);
    public AudioClip GetClipAct2() =>assets.GetClipAct2(currentAlphabet);
    public AudioClip GetClipAct3(string word) => assets.GetClipAct3(word);
    public AudioClip GetClipWord(string word) => assets.GetClipWord(word);
    public AudioClip GetClipCorrectEffect() => assets.GetClipCorrectEffect();
    public Sprite GetAlphbetSprite(eAlphbetStyle style, eAlphbetType type) => assets.GetSpriteAlphabet(style, type, currentAlphabet);
    public Sprite[] GetAlphbetSprites(eAlphbetStyle style, eAlphbetType type) => assets.GetSpriteAlphabet(style, type);
    public Sprite[] GetSpriteWord(eAlphabet alphabet) => assets.GetSpriteWord(alphabet);
    public Sprite[] GetSpriteWord() => assets.GetSpriteWord(currentAlphabet);
    public Sprite GetSpriteWord(string word) => assets.GetSpriteWord(word);

    public string[] GetWords() => assets.GetWords();
}
