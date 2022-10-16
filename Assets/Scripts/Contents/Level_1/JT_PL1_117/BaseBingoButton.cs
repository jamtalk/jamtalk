using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class BaseBingoButton<TValue, TViewer> : MonoBehaviour
    where TViewer : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button button => GetComponent<Button>();
    public TValue value { get; private set; }
    public TViewer viewer;
    public Image imageStamp;
    public Action<TValue> onClick;
    public AudioSinglePlayer audioPlayer;
    public AudioClip stampingClip;
    public bool isOn { get; private set; }
    protected Func<TValue, bool> isCorrect;
    public string strValue;
    protected virtual void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void Init(TValue value, Sprite Stamp, Func<TValue, bool> isCorrect)
    {
        this.isCorrect = isCorrect;
        this.value = value;
        isOn = false;
        button.interactable = true;
        //SetViewer
        //viewer.sprite = GameManager.Instance.GetAlphbetSprite(style, eAlphabetType.Upper, value);
        //viewer.preserveAspect = true;
        SetViewer();
        imageStamp.gameObject.SetActive(false);
        viewer.gameObject.SetActive(true);
        imageStamp.sprite = Stamp;
        imageStamp.preserveAspect = true;
    }
    protected abstract void SetViewer();
    private void OnClick()
    {
        if (isCorrect.Invoke(value))
        {
            button.interactable = false;
            Stamping(() => onClick?.Invoke(value));
        }
        else
            onClick?.Invoke(value);
    }
    public void GuideClick()
    {
        button.interactable = false;
        Stamping();
    }
    private void Stamping(TweenCallback onStamped = null)
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
            viewer.gameObject.SetActive(false);
        };
        tween.Play();
    }
}
