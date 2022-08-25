using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine502 : BaseSlotMachine<string, SlotElement502>
{
    public override void Sloting(string[] datas, TweenCallback onDone)
    {
        var lastIndex = datas.Length - 1;
        datas[lastIndex] = string.Format("<color=\"red\">{0}</color>", datas[lastIndex]);
        base.Sloting(datas, onDone);
    }
}
