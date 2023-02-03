using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleElement : MonoBehaviour
{
    public Toggle pushToggle;
    public Image toggleBg;
    private RectTransform toggleRt => pushToggle.GetComponent<RectTransform>();
    public RectTransform leftRt;
    public RectTransform rightRt;
    public Sprite toggleOnSprite;
    public Sprite toggleOffSprite;

    private void Awake()
    {
        pushToggle.onValueChanged.AddListener(SetToggle);
    }

    public void SetToggle(bool value)
    {
        var toggleSprite = value ? toggleOnSprite : toggleOffSprite;
        var targetRt = value ? leftRt : rightRt;

        pushToggle.transform.DOMove(targetRt.position, 1f);
        toggleBg.sprite = toggleSprite;
    }
}
