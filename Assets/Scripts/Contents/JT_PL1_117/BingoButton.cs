using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class BingoButton : MonoBehaviour
{
    public JT_PL1_117 controller;
    public EventSystem eventSystem;
    public Button button => GetComponent<Button>();
    public eAlphabet value { get; private set; }
    public eAlphabetStyle style;
    public Image imageAlphabet;
    public Image imageStamp;
    public event Action<eAlphabet> onClick;
    public AudioSinglePlayer audioPlayer;
    public AudioClip stampingClip;
    public bool isOn { get; private set; }
    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void Init(eAlphabet value, Sprite Stamp)
    {
        this.value = value;
        isOn = false;
        button.interactable = true;
        imageAlphabet.sprite = GameManager.Instance.GetAlphbetSprite(style, eAlphbetType.Upper, value);
        imageStamp.gameObject.SetActive(false);
        imageAlphabet.gameObject.SetActive(true);
        imageStamp.sprite = Stamp;
        imageStamp.preserveAspect = true;
        imageAlphabet.preserveAspect = true;
    }
    private void OnClick()
    {
        Debug.LogFormat("{0} : {1}",name,controller == null);
        if (controller.currentQuestion == value)
        {
            button.interactable = false;
            Stamping(() => onClick?.Invoke(value));
        }
        else
            onClick?.Invoke(value);
    }
    private void Stamping(TweenCallback onStamped)
    {
        audioPlayer.Play(1.5f, stampingClip);
        eventSystem.enabled = false;
        isOn = true;
        imageStamp.transform.localScale = Vector3.one * 1.5f;
        imageStamp.gameObject.SetActive(true);
        var tween = imageStamp.transform.DOScale(1, 1f);
        tween.SetEase(Ease.OutCubic);
        tween.onComplete += onStamped;
        tween.onComplete += () =>
        {
            eventSystem.enabled = true;
            imageAlphabet.gameObject.SetActive(false);
        };
        tween.Play();
    }
}
