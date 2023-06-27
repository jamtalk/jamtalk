using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.UI;

public class JT_PL1_102 : BaseContents<AlphabetContentsSetting>
{
    protected override eContents contents => eContents.JT_PL1_102;
    public int ClickCount => 3;
    public int currentClickCount = 0;
    protected override bool CheckOver() => currentClickCount == ClickCount;
    private int _index;
    private int currentIndex
    {
        get => _index;
        set
        {
            _index = value;
            if (value < targets.Length)
            {
                act = GameManager.Instance.schema.GetAlphabetAudio(targets[currentIndex]).act2;
                phanics = GameManager.Instance.schema.GetAlphabetAudio(targets[currentIndex]).phanics;
            }
        }
    }
    private string act;
    private string phanics;
    protected override int GetTotalScore() => 1;
    public Image imageAlphabet;
    public Button buttonEgg;
    public Egg egg;

    private eAlphabet[] targets;

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
                    guideFinger.gameObject.SetActive(false);
                    OnClickEgg();
                });
            });
        });
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        //ShowGuidnce();
        targets = new eAlphabet[] { GameManager.Instance.currentAlphabet, GameManager.Instance.currentAlphabet + 1 };
        currentIndex = 0;
        egg.onBroken += OnBorken;
        egg.onbreaking += () => imageAlphabet.gameObject.SetActive(true);
        buttonEgg.onClick.AddListener(OnClickEgg);
        Init(targets[currentIndex]);
    }
    protected void Init(eAlphabet value)
    {
        egg.Init();
        currentClickCount = 0;
        var data = GameManager.Instance.GetResources(value);
        imageAlphabet.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Card, eAlphabetType.Upper, value);

        imageAlphabet.gameObject.SetActive(false);
        //imageAlphabet.SetNativeSize();
        imageAlphabet.preserveAspect = true;
    }
    private void OnBorken()
    {
        imageAlphabet.gameObject.SetActive(true);
        audioPlayer.Play(act,()=>
        {
            guidePopup.gameObject.SetActive(false);

            if (!isGuide)
                currentIndex += 1;
            else
                isGuide = false;

            if (currentIndex < targets.Length)
                Init(targets[currentIndex]);
            else
                ShowResult();
        });
    }
    private void OnClickEgg()
    {
        audioPlayer.Play(phanics);

        currentClickCount += 1;
        if (CheckOver())
            egg.Break();
        else if ((float)currentClickCount / (float)ClickCount >= .5f && !egg.isCrakced)
            egg.SetCrack();
        else
            egg.Shake();
    }

    protected override void EndGuidnce()
    {
        egg.Init();
        currentClickCount = 0;
        base.EndGuidnce();
    }
}
