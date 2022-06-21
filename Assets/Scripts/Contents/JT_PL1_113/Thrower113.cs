using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Thrower113 : BaseThrower<Item113>
{
    public Image imageProduct;
    public Text textValue;

    protected override void SetItem(Item113 item)
    {
        imageProduct.sprite = item.product.sprite;
        textValue.text = item.textValue.text;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }

    protected override void SetTime(RectTransform target)
    {
        upperTime = .125f;
        moveTime = Vector3.Distance(transform.position, target.position) / 3000f;
        lowerTime = moveTime / 2f;
        inertTime = upperTime + moveTime / 2f;
    }
}
