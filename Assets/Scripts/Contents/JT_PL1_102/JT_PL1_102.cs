using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;

public class JT_PL1_102 : BaseContents
{
    protected override eContents contents => eContents.JT_PL1_102;
    public int ClickCount => 3;
    public int currentClickCount = 0;
    protected override bool CheckOver() => currentClickCount == ClickCount;
    private int currentIndex;
    protected override int GetTotalScore() => 1;
    public Image imageAlphabet;
    public Button buttonEgg;
    public Egg egg;
    public AlphabetAudioData.AlphabetAudioSource audioData;
    private eAlphabet[] targets;
    protected override void Awake()
    {
        base.Awake();
        currentIndex = 0;
        targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet+1 };
        Init(targets[currentIndex]);
        egg.onBroken += OnBorken;
        buttonEgg.onClick.AddListener(OnClickEgg);
    }
    protected void Init(eAlphabet value)
    {
        egg.Init();
        currentClickCount = 0;
        var data = GameManager.Instance.GetResources(value);
        audioData = data.AudioData;
        imageAlphabet.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Card, eAlphabetType.Upper, value);
        imageAlphabet.SetNativeSize();
        imageAlphabet.preserveAspect = true;
    }
    private void OnBorken()
    {
        audioPlayer.Play(audioData.act2,()=>
        {
            currentIndex += 1;
            if (currentIndex < targets.Length)
                Init(targets[currentIndex]);
            else
                ShowResult();
        });
    }
    private void OnClickEgg()
    {
        if (!CheckOver())
            audioPlayer.Play(audioData.phanics);
        else
            audioPlayer.Play(audioData.phanics);

        currentClickCount += 1;
        if (CheckOver())
            egg.Break();
        else if ((float)currentClickCount / (float)ClickCount >= .5f && !egg.isCrakced)
            egg.SetCrack();
        else
            egg.Shake();
    }
}
