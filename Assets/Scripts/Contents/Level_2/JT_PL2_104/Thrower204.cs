using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Thrower204 : BaseThrower<BubbleElement>
{
    public Image imageProduct;
    public Text textValue;

    protected override float upperTime => .5f;

    protected override float lowerTime => .5f;

    protected override float inertTime => .5f;
    protected override void SetItem(BubbleElement item)
    {
        textValue.text = item.textValue.text;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }
}
