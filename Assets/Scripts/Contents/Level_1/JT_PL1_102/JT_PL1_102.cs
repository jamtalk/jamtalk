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

    private eAlphabet[] targets;

    bool isGuidnce = false;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        guideFinger.gameObject.SetActive(true);
        var eggGuide = egg;
        eggGuide.Init();

        guideFinger.DoClick(() =>
        {
            OnClickEgg();
            guideFinger.DoClick(() =>
            {
                OnClickEgg();
                guideFinger.DoClick(() =>
                {
                    OnClickEgg();
                    guidePopup.gameObject.SetActive(false);
                });
            });
        });

    }

    protected override void Awake()
    {
        base.Awake();
        //ShowGuidnce();
        currentIndex = 0;
        egg.onBroken += OnBorken;
        buttonEgg.onClick.AddListener(OnClickEgg);
        targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        Init(targets[currentIndex]);
    }
    protected void Init(eAlphabet value)
    {
        egg.Init();
        currentClickCount = 0;
        var data = GameManager.Instance.GetResources(value);
        imageAlphabet.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Card, eAlphabetType.Upper, value);
        imageAlphabet.SetNativeSize();
        imageAlphabet.preserveAspect = true;
    }
    private void OnBorken()
    {
        audioPlayer.Play(GameManager.Instance.schema.GetAlphabetAudio(targets[currentIndex]).act2,()=>
        {
            if (isGuidnce)
                currentIndex += 1;
            else
                isGuidnce = true;

            if (currentIndex < targets.Length)
                Init(targets[currentIndex]);
            else
                ShowResult();
        });
    }
    private void OnClickEgg()
    {
        if (!CheckOver())
            audioPlayer.Play(GameManager.Instance.schema.GetAlphabetAudio(targets[currentIndex]).phanics);
        else
            audioPlayer.Play(GameManager.Instance.schema.GetAlphabetAudio(targets[currentIndex]).phanics);

        currentClickCount += 1;
        if (CheckOver())
            egg.Break();
        else if ((float)currentClickCount / (float)ClickCount >= .5f && !egg.isCrakced)
            egg.SetCrack();
        else
            egg.Shake();
    }
}
