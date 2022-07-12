using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thrower505 : BaseThrower<DoubleClick505>
{
    public Text text;

    protected override void SetItem(DoubleClick505 item)
    {
        text.text = item.text.text;
        upperSize = 1f;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }

    protected override void SetTime(RectTransform target)
    {
        upperTime = 0.8f;
        moveTime = 1f;
        lowerTime = 0.8f;
        inertTime = 0.8f;
    }
}
