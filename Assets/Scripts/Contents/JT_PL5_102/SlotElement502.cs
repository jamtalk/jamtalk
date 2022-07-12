using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotElement502 : SlotMachineElement<DigraphsWordsData>
{
    public Text text;
    public Image image;
    public override void Init(DigraphsWordsData data)
    {
        var digraphs = data.Digraphs.ToString().ToLower();
        var value = data.key.Replace(digraphs, "<color=\"red\">" + digraphs + "</color>");

        text.text = value;
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }
}
