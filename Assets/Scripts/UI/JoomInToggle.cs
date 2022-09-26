using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class JoomInToggle : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(value =>
        {
            var scale = value ? 1f : .8f;
            GetComponent<RectTransform>().DOScale(scale, .3f);
        });
    }
}
