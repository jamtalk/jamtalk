using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rocket<TUI,TValue> : MonoBehaviour
{
    public eRocketDirection direction;
    public GameObject mask;
    public TUI valueUI;

    public AudioSinglePlayer audioPlayer;
    public AudioClip clipMove;
    public AudioClip clipStop;
    public AudioClip clipArrival;

    private bool isMoving=false;

    public RectTransform rt => GetComponent<RectTransform>();
    private float horizontalStartPoint => Screen.width / 2f;
    private float horizontalMiddlePoint => rt.sizeDelta.x / -2f;
    private float horizontalEndPoint => Screen.width / -2f - rt.sizeDelta.x;
    private float verticalStartPoint => Screen.height/-2f + rt.sizeDelta.y / -2f;
    private float verticalMiddlePoint => rt.sizeDelta.y / 2f;
    private float verticalEndPoint => Screen.height / 2f + rt.sizeDelta.y;
    public void Init()
    {
        switch (direction)
        {
            case eRocketDirection.Vertical:
                rt.pivot = new Vector2(0f, 1f);
                rt.anchorMin = new Vector2(0f, .5f);
                rt.anchorMax = new Vector2(0f, .5f);
                break;
            case eRocketDirection.Horizontal:
                rt.pivot = new Vector2(0f, 1f);
                rt.anchorMin = new Vector2(0f, .5f);
                rt.anchorMax = new Vector2(0f, .5f);
                break;
        }
        SetStartPosition();
    }
    public void SetStartPosition()
    {
        var pos = rt.anchoredPosition;
        switch (direction)
        {
            case eRocketDirection.Vertical:
                pos.y = verticalStartPoint;
                break;
            case eRocketDirection.Horizontal:
                pos.x = horizontalStartPoint;
                break;
        }
        rt.anchoredPosition = pos;
    }
    public void SetMiddlePosition()
    {
        var pos = rt.anchoredPosition;
        switch (direction)
        {
            case eRocketDirection.Vertical:
                pos.x = verticalMiddlePoint;
                break;
            case eRocketDirection.Horizontal:
                pos.y = horizontalMiddlePoint;
                break;
        }
        rt.anchoredPosition = pos;
    }
    public void SetEndPositition()
    {
        var pos = rt.anchoredPosition;
        switch (direction)
        {
            case eRocketDirection.Vertical:
                pos.x = verticalEndPoint;
                break;
            case eRocketDirection.Horizontal:
                pos.y = horizontalEndPoint;
                break;
        }
        rt.anchoredPosition = pos;
    }

    public void Call(TweenCallback onArrival = null)
    {
        isMoving = true;
        SetStartPosition();
        mask.SetActive(false);
        Tween tween = null;
        float duration = 2f;
        switch (direction)
        {
            case eRocketDirection.Vertical:
                tween = rt.DOAnchorPosY(verticalMiddlePoint, duration);
                tween.onUpdate += () =>
                {
                    if (rt.anchoredPosition.y > verticalMiddlePoint / 2f && isMoving)
                    {
                        isMoving = false;
                        audioPlayer.Play(duration / 2f, clipStop);
                    }
                };
                break;
            case eRocketDirection.Horizontal:
                tween = rt.DOAnchorPosX(horizontalMiddlePoint, duration);
                tween.onUpdate += () =>
                {
                    if (rt.anchoredPosition.x < horizontalMiddlePoint / 1.5f && isMoving)
                    {
                        isMoving = false;
                        audioPlayer.Play(duration / 2f,clipStop);
                    }
                };
                break;
        }
        tween.onComplete += () => audioPlayer.Play(clipArrival);
        tween.onComplete += onArrival;
        tween.SetEase(Ease.OutCubic);
        audioPlayer.Play(clipMove);
        tween.Play();
    }
    public void Away(TValue value, TweenCallback onLeave=null)
    {
        SetValue(value);
        mask.gameObject.SetActive(true);

        Tween tween = null;
        float duration = 2f;
        switch (direction)
        {
            case eRocketDirection.Vertical:
                tween = rt.DOAnchorPosY(verticalEndPoint, duration);
                break;
            case eRocketDirection.Horizontal:
                tween = rt.DOAnchorPosX(horizontalEndPoint, duration);
                break;
        }

        tween.onComplete += onLeave;
        tween.SetEase(Ease.InCubic);
        audioPlayer.Play(clipMove);
        tween.Play();

    }
    protected abstract void SetValue(TValue value);
}
