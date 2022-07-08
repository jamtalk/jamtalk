using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlot : BaseSlotMachine<int,TestSlotElement>
{
    protected override float duration => 5f;
    private void Awake()
    {
        var values = new List<int>();
        for (int i = 0; i < 10; i++)
            values.Add(i);

        Sloting(values.ToArray(), () => Debug.Log("³¡"));
    }
}
