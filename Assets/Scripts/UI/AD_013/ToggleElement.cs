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


    public void Awake()
    {
        Init();
        pushToggle.onValueChanged.AddListener(SetToggle);
    }

    public void Init() => StartCoroutine(WaitRoutine());

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForEndOfFrame();
        SetToggle(UserDataManager.Instance.CurrentUser.isPush);
    }

    public void SetToggle(bool value)
    {
        var toggleSprite = value ? toggleOnSprite : toggleOffSprite;
        var targetRt = value ? leftRt : rightRt;

        toggleTarget.transform.DOMove(targetRt.position, 1f);
        toggleBg.sprite = toggleSprite;
    }
}
