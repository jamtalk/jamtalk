using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Thrower113 : BaseThrower<Item113>
{
    public Image imageProduct;
    public Text textValue;

    protected override float upperTime => .125f;

    protected override float lowerTime => moveTime / 2f;

    protected override float inertTime => upperTime + moveTime / 2f;

    protected override void SetItem(Item113 item)
    {
        imageProduct.sprite = item.product.sprite;
        textValue.text = item.textValue.text;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }
}
