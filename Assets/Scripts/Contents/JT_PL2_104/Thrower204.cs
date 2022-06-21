using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Thrower204 : BaseThrower<BubbleElement>
{
    public Image imageProduct;
    public Text textValue;

    protected override void SetTime(RectTransform target)
    {
        upperTime = 0.5f;
        moveTime = Vector3.Distance(transform.position, target.position) / 10f;
        lowerTime = 0.5f;
        inertTime = 0.5f;
    }
    protected override void SetItem(BubbleElement item)
    {
        textValue.text = item.textValue.text;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }
}
