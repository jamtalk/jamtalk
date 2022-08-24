using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thrower306 : BaseThrower<Image>
{
    protected override float upperTime => .5f;

    protected override float lowerTime => moveTime / 2f;

    protected override float inertTime => upperTime + moveTime / 2f;
    protected override float upperSize => .5f;

    protected override void SetItem(Image item)
    {
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }
}
