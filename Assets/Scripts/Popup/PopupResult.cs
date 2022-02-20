using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupResult : BasePopup
{
    [Header("UI")]
    public Image imageResult;
    public Button buttonPre;
    public Button[] buttonNext;
    public AudioSource source;
    [Header("Sprites")]
    public Sprite spritePerfect;
    public Sprite spriteGreate;
    public Sprite[] spriteFail;
    public eGameResult result { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        buttonPre.onClick.AddListener(OnClickPre);
        for (int i = 0; i < buttonNext.Length; i++)
            buttonNext[i].onClick.AddListener(OnClickNext);
    }
    public void SetResult(eGameResult result)
    {
        this.result = result;
        switch (result)
        {
            case eGameResult.Perfect:
                imageResult.sprite = spritePerfect;
                source.Play();
                break;
            case eGameResult.Greate:
                imageResult.sprite = spriteGreate;
                break;
            case eGameResult.Fail:
                imageResult.sprite = spriteFail[Random.Range(0, spriteFail.Length)];
                break;
        }
    }
    private void OnClickNext()
    {
        if (result == eGameResult.Perfect)
            SetResult(eGameResult.Greate);
        else
            SetResult(eGameResult.Perfect);
    }
    private void OnClickPre()
    {

        SetResult(eGameResult.Fail);
    }
}
