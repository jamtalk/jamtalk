using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thrower505 : BaseThrower<DoubleClick505>
{
    public Text text;

    protected override float upperTime => .8f;

    protected override float lowerTime => .8f;

    protected override float inertTime => .8f;
    protected override float upperSize => 1f;

    protected override void SetItem(DoubleClick505 item)
    {
        text.text = item.text.text;
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }
}
