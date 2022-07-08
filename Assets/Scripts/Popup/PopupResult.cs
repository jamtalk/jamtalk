using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                break;
            case eGameResult.Greate:
                imageResult.sprite = spriteGreate;
                break;
            case eGameResult.Fail:
                imageResult.sprite = spriteFail[Random.Range(0, spriteFail.Length)];
                break;
        }
        source.Play();
    }
    private void OnClickNext()
    {
        if (GameManager.Instance.currentAlphabet + 1 < eAlphabet.Z)
            GJGameLibrary.GJSceneLoader.Instance.LoadScene(eSceneName.Test);
        else
        {
            GameManager.Instance.currentAlphabet += 2;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void OnClickPre()
    {
        GJGameLibrary.GJSceneLoader.Instance.LoadScene(eSceneName.Test);
    }
}
