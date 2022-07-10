using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotElement502 : SlotMachineElement<DigraphsSource>
{
    public Text text;
    public Image image;
    public override void Init(DigraphsSource data)
    {
        var digraphs = data.type.ToString().ToLower();
        var value = data.value.Replace(digraphs, "<color=\"red\">" + digraphs + "</color>");

        text.text = value;
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }
}
