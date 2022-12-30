using GJGameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class PopupResult : BasePopup
{
    [Header("UI")]
    public Image imageResult;
    public Button buttonPre;
    public Button buttonNext;
    public Button buttonConfirm;
    public AudioSource source;
    [Header("Sprites")]
    public Sprite spritePerfect;
    public Sprite spriteGreate;

    public Sprite[] spriteSuccess;
    public Sprite[] spriteFail;
    public eGameResult result { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        buttonPre.onClick.AddListener(OnClickPre);
        buttonNext.onClick.AddListener(OnClickNext);
        buttonConfirm.onClick.AddListener(() => GJSceneLoader.Instance.LoadScene(eSceneName.AD_003));
        buttonPre.gameObject.SetActive(GameManager.Instance.currentContents > eContents.JT_PL1_102);
    }

    public void Init(Action onClickConfirm, Action onClickNext = null, Action onClickPre = null)
    {
        buttonConfirm.onClick.AddListener(() => onClickConfirm?.Invoke());

        buttonNext.gameObject.SetActive(onClickNext != null);
        buttonNext.onClick.AddListener(() => onClickNext?.Invoke());

        buttonPre.gameObject.SetActive(onClickPre != null);
        buttonPre.onClick.AddListener(() => onClickPre?.Invoke());
    }
    public void SetResult(eGameResult result)
    {
        this.result = result;

        var resultDatas = ResourceSchema.GetCorrectClip();
        var successSprit = spriteSuccess.Where(x => x.name == resultDatas.Item2.ToString()).First();

        switch (result)
        {
            case eGameResult.Fail:
                imageResult.sprite = spriteFail[Random.Range(0, spriteFail.Length)];
                break;
            default:
                imageResult.sprite = successSprit;
                break;
        }

        source.clip = resultDatas.Item1;
        source.Play();
    }
    private void OnClickNext()
    {
        if (GameManager.Instance.currentAlphabet + 1 < eAlphabet.Z)
            GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);
        else
        {
            GameManager.Instance.currentAlphabet += 2;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void OnClickPre()
    {
        GJSceneLoader.Instance.LoadScene(GJSceneLoader.Instance.currentScene +1);
        GJGameLibrary.GJSceneLoader.Instance.LoadScene(eSceneName.AD_003);
    }
}
