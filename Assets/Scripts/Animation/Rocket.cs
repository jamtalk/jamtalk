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
    private float horizontalStartPoint_ToLeft => Screen.width / 2f;
    private float horizontalMiddlePoint_ToLeft => rt.sizeDelta.x / -2f;
    private float horizontalEndPoint_ToLeft => Screen.width / -2f - rt.sizeDelta.x;
    private float horizontalStartPoint_ToRight => (Screen.width / -2f) - rt.sizeDelta.x;
    private float horizontalMiddlePoint_ToRight => rt.sizeDelta.x / -2f;
    private float horizontalEndPoint_ToRight => Screen.width / 2f;
    private float verticalStartPoint => Screen.height/-2f + rt.sizeDelta.y / -2f;
    private float verticalMiddlePoint => rt.sizeDelta.y / 2f;
    private float verticalEndPoint => Screen.height / 2f + rt.sizeDelta.y;
    [Range(0,2)]
    public int tempNum = 0;


    Tween tween;
    private RectTransform defaultRt => GetComponent<RectTransform>();

    public void ResetPosition()
    {
        if(tween != null)
        {
            tween.Kill();
            tween = null;
        }
        transform.position = defaultRt.position;
    }
    private void Awake()
    {
        
    }
    public void Init()
    {
        switch (direction)
        {
            case eRocketDirection.Vertical:
                rt.pivot = new Vector2(0f, 1f);
                rt.anchorMin = new Vector2(0f, .5f);
                rt.anchorMax = new Vector2(0f, .5f);
                break;
            case eRocketDirection.Horizontal_ToLeft:
                rt.pivot = new Vector2(0f, 1f);
                rt.anchorMin = new Vector2(0f, .5f);
                rt.anchorMax = new Vector2(0f, .5f);
                break;
            case eRocketDirection.Horizontal_ToRight:
                rt.pivot = new Vector2(1f, 0f); 
                rt.anchorMin = new Vector2(.5f, 0f);
                rt.anchorMax = new Vector2(.5f, 0f);
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
            case eRocketDirection.Horizontal_ToLeft:
                pos.x = horizontalStartPoint_ToLeft;
                break;
            case eRocketDirection.Horizontal_ToRight:
                pos.x = horizontalStartPoint_ToRight;
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
            case eRocketDirection.Horizontal_ToLeft:
                pos.y = horizontalMiddlePoint_ToLeft;
                break;
            case eRocketDirection.Horizontal_ToRight:
                pos.x = horizontalMiddlePoint_ToRight;
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
            case eRocketDirection.Horizontal_ToLeft:
                pos.y = horizontalEndPoint_ToLeft;
                break;
            case eRocketDirection.Horizontal_ToRight:
                pos.x = horizontalEndPoint_ToRight;
                break;
        }
        rt.anchoredPosition = pos;
    }

    public void Call(TweenCallback onArrival = null)
    {
        isMoving = true;
        SetStartPosition();
        mask.SetActive(false);
        tween = null;
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
            case eRocketDirection.Horizontal_ToLeft:
                tween = rt.DOAnchorPosX(horizontalMiddlePoint_ToLeft, duration);
                tween.onUpdate += () =>
                {
                    if (rt.anchoredPosition.x < horizontalMiddlePoint_ToLeft / 1.5f && isMoving)
                    {
                        isMoving = false;
                        audioPlayer.Play(duration / 2f,clipStop);
                    }
                };
                break;
            case eRocketDirection.Horizontal_ToRight:
                tween = rt.DOAnchorPosX(horizontalMiddlePoint_ToRight, duration);
                tween.onUpdate += () =>
                {
                    if (rt.anchoredPosition.x < horizontalMiddlePoint_ToRight / 1.5f && isMoving)
                    {
                        isMoving = false;
                        audioPlayer.Play(duration / 2f, clipStop);
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

        tween = null;
        float duration = 2f;
        switch (direction)
        {
            case eRocketDirection.Vertical:
                tween = rt.DOAnchorPosY(verticalEndPoint, duration);
                break;
            case eRocketDirection.Horizontal_ToLeft:
                tween = rt.DOAnchorPosX(horizontalEndPoint_ToLeft, duration);
                break;
            case eRocketDirection.Horizontal_ToRight:
                tween = rt.DOAnchorPosX(horizontalEndPoint_ToRight, duration);
                break;

        }

        tween.onComplete += onLeave;
        tween.SetEase(Ease.InCubic);
        audioPlayer.Play(clipMove);
        tween.Play();

    }
    protected abstract void SetValue(TValue value);
}
