using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TurningCard : MonoBehaviour
{
    public AudioSinglePlayer audioPlayer;
    public RectTransform front;
    public RectTransform back;
    [Header("Buttons")]
    public Button buttonFront;
    public Button buttonBack;
    public event Action onClick;
    [Header("Clips")]
    public AudioClip clipFront;
    public AudioClip clipBack;
    private bool _isFront = true;
    private bool alwaysBackDisable;
    private bool alwaysFrontDisable;
    private Sequence seq;
    public event Action onTurned;
    public bool IsFornt
    {
        get => _isFront;
        set
        {
            Debug.Log(value);
            _isFront = value;
            if (value)
                SetFront();
            else
                SetBack();
        }
    }
    public void Init(float duration=1, TweenCallback callback = null, bool alwaysBackDisable = false, bool alwaysFrontDisable = false)
    {
        this.alwaysBackDisable = alwaysBackDisable;
        this.alwaysFrontDisable = alwaysFrontDisable;
        buttonFront.onClick.RemoveAllListeners();
        buttonBack.onClick.RemoveAllListeners();
        buttonBack.interactable = alwaysBackDisable;
        buttonFront.interactable = alwaysFrontDisable;
        buttonFront.onClick.AddListener(() =>
        {
            onClick?.Invoke();
            if (!alwaysFrontDisable)
                Turnning(duration, callback);
        });
        buttonBack.onClick.AddListener(() =>
        {
            onClick?.Invoke();
            if (!alwaysBackDisable)
                Turnning(duration, callback);
        });
    }
    public virtual void SetFront()
    {
        _isFront = true;
        buttonFront.interactable = !alwaysFrontDisable;
        buttonBack.interactable = !alwaysBackDisable;
        front.rotation = Quaternion.Euler(0, 0, 0);
        back.rotation = Quaternion.Euler(0, -90f, 0);
    }
    public virtual void SetBack()
    {
        _isFront = false;
        buttonFront.interactable = !alwaysFrontDisable;
        buttonBack.interactable = !alwaysBackDisable;
        front.rotation = Quaternion.Euler(0, -90f, 0);
        back.rotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual void Turnning(float duration=1f, TweenCallback onCompleted = null)
    {
        if (seq != null)
            seq.Kill();
        onCompleted += () => onTurned?.Invoke();
        if (_isFront)
            TurnningBack(duration,onCompleted);
        else
            TurnningFront(duration,onCompleted);
    }
    protected virtual void TurnningBack(float duration, TweenCallback onCompleted = null)
    {
        SetFront();
        _isFront = false;
        buttonFront.interactable = false;
        buttonBack.interactable = false;
        seq = DOTween.Sequence();

        var frontTween = front.DORotate(new Vector3(0, -90,0), duration / 2f);
        frontTween.SetEase(Ease.Linear);

        var backTween = back.DORotate(new Vector3(0, 0, 0), duration / 2f);
        backTween.SetEase(Ease.Linear);

        seq.Append(frontTween);
        seq.Append(backTween);
        seq.onKill += SetBack;
        seq.onComplete += onCompleted;
        seq.Play();

        audioPlayer.Play(clipBack);
    }
    private void TurnningFront(float duration, TweenCallback onCompleted = null)
    {
        SetBack();
        _isFront = true;
        buttonFront.interactable = false;
        buttonBack.interactable = false;
        seq = DOTween.Sequence();

        var frontTween = front.DORotate(new Vector3(0, 0, 0), duration / 2f);
        frontTween.SetEase(Ease.Linear);

        var backTween = back.DORotate(new Vector3(0, -90, 0), duration / 2f);
        backTween.SetEase(Ease.Linear);

        seq.Append(backTween);
        seq.Append(frontTween);
        seq.onKill += SetFront;
        seq.onComplete += onCompleted;
        seq.Play();

        audioPlayer.Play(clipFront);
    }
}
