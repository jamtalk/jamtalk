using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotElement502 : SlotMachineElement<string>
{
    public Text text;
    public override void Init(string data) => text.text = data;
}
