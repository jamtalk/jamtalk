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
    protected override int GetTotalScore() => 1;
    public Image imageAlphabet;
    public Button buttonEgg;
    public Egg egg;
    public AudioSinglePlayer audioPlayer;
    public AlphabetAudioData.AlphabetAudioSource audioData;
    protected override void Awake()
    {
        Debug.Log("컨텐츠 로드 완료");
        base.Awake();
        Debug.Log("베이스");
        audioData = GameManager.Instance.GetResources().AudioData;
        Debug.LogFormat("오디오 {0}", audioData!=null);
        imageAlphabet.sprite = null;
        imageAlphabet.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Card,eAlphabetType.Upper,GameManager.Instance.currentAlphabet);
        Debug.LogFormat("이미지 {0}", imageAlphabet.sprite != null);
        imageAlphabet.SetNativeSize();
        imageAlphabet.preserveAspect = true;
        egg.onBroken += OnBorken;
        buttonEgg.onClick.AddListener(OnClickEgg);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("클릭");
        }
    }
    private void OnBorken()
    {
        audioPlayer.Play(audioData.act2,ShowResult);
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
