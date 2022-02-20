using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;

public class JT_PL1_102 : SingleAnswerContents
{
    public AssetImporter asset;
    public SerializableDictionaryBase<eAlphabet, Sprite> sprites;
    public SerializableDictionaryBase<eAlphabet, AudioClip> clipPhanics;
    public SerializableDictionaryBase<eAlphabet, AudioClip> clipResult;
    protected override eContents contents => eContents.JT_PL1_102;
    public int ClickCount => 10;
    public int currentClickCount = 0;
    protected override bool CheckOver() => currentClickCount == ClickCount;
    
    public Image imageAlphabet;
    public Button buttonEgg;
    public Egg egg;
    public AudioSource audioPlayer;
    private void Awake()
    {
        imageAlphabet.sprite = sprites[GameManager.Instance.currentAlphabet];
        imageAlphabet.SetNativeSize();
        imageAlphabet.preserveAspect = true;
        egg.onBroken += ShowResult;
        buttonEgg.onClick.AddListener(OnClickEgg);
    }
    private void OnClickEgg()
    {
        if (!CheckOver())
            PlayAudio(clipPhanics[GameManager.Instance.currentAlphabet]);
        else
            PlayAudio(clipResult[GameManager.Instance.currentAlphabet]);

        currentClickCount += 1;
        if (CheckOver())
            egg.Break();
        else if ((float)currentClickCount / (float)ClickCount >= .5f && !egg.isCrakced)
            egg.SetCrack();
        else
            egg.Shake();
    }
    private void PlayAudio(AudioClip clip)
    {
        if (audioPlayer.isPlaying)
            audioPlayer.Stop();

        if (audioPlayer.clip != clip)
            audioPlayer.clip = clip;

        audioPlayer.Play();
    }
}
