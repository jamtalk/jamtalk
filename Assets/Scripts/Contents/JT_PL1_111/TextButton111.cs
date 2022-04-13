using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton111 : TextButton
{
    public Image image;
    public override void Init(string value)
    {
        base.Init(value);
        image.sprite = GameManager.Instance.GetSpriteWord(value);
        image.preserveAspect = true;
    }
}
