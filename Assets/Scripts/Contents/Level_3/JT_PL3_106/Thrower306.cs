using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thrower306 : BaseThrower<Image>
{
    protected override void SetItem(Image item)
    {
        rt.sizeDelta = item.GetComponent<RectTransform>().rect.size;
        rt.position = item.GetComponent<RectTransform>().position;
    }

    protected override void SetTime(RectTransform target)
    {
        upperSize = .5f;
        upperTime = .3f;
        moveTime = Vector3.Distance(transform.position, target.position) / 3000f;
        lowerTime = moveTime / 2f;
        inertTime = upperTime + moveTime / 2;
    }
}
