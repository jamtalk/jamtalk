using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleElement : MonoBehaviour
{
    public Toggle pushToggle;
    public GameObject toggleTarget;
    public Image toggleBg;
    public RectTransform leftRt;
    public RectTransform rightRt;
    public Sprite toggleOnSprite;
    public Sprite toggleOffSprite;
    private float time=>.25f;


    public void Awake()
    {
        Init();
        pushToggle.onValueChanged.AddListener((value)=>SetToggle(value,time));
    }

    public void Init() => StartCoroutine(WaitRoutine());

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForEndOfFrame();
        SetToggle(UserDataManager.Instance.CurrentUser.isPush,0);
    }

    public void SetToggle(bool value, float time)
    {
        var toggleSprite = value ? toggleOnSprite : toggleOffSprite;
        var targetRt = value ? leftRt : rightRt;

        toggleTarget.transform.DOMove(targetRt.position, time);
        toggleBg.sprite = toggleSprite;
    }
}
