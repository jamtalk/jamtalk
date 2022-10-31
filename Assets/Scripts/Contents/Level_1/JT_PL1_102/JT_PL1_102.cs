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

    [Header("Guide")]
    public Egg guideEgg;
    public Image guideAlphaImage;

    private eAlphabet[] targets;

    bool isGuidnce = false;

    protected override IEnumerator ShowGuidnceRoutine()
    {
        yield return base.ShowGuidnceRoutine();

        guideFinger.gameObject.SetActive(true);
        guideAlphaImage.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Card, eAlphabetType.Upper, GameManager.Instance.currentAlphabet);
        guideAlphaImage.SetNativeSize();
        guideAlphaImage.preserveAspect = true;
        guideEgg.onBroken += OnBorken;
        guideEgg.Init();

        guideFinger.DoClick(() =>
        {
            OnClickEgg(guideEgg);
            guideFinger.DoClick(() =>
            {
                OnClickEgg(guideEgg);
                guideFinger.DoClick(() =>
                {
                    guideFinger.gameObject.SetActive(false);
                    OnClickEgg(guideEgg);
                });
            });
        });

    }

    protected override void Awake()
    {
        base.Awake();
        currentIndex = 0;
        egg.onBroken += OnBorken;
        buttonEgg.onClick.AddListener(() => OnClickEgg(egg));
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
            guidePopup.gameObject.SetActive(false);

            if (currentIndex < targets.Length)
                Init(targets[currentIndex]);
            else
                ShowResult();
        });
    }
    private void OnClickEgg(Egg egg)
    {

        audioPlayer.Play(GameManager.Instance.schema.GetAlphabetAudio(targets[currentIndex]).phanics);

        currentClickCount += 1;
        if (CheckOver())
        {
            egg.Break();
            currentClickCount = 0;
        }
        else if ((float)currentClickCount / (float)ClickCount >= .5f && !egg.isCrakced)
            egg.SetCrack();
        else
            egg.Shake();
    }
}
