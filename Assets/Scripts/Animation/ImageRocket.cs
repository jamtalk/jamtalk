using UnityEngine;
using UnityEngine.UI;

public class ImageRocket : Rocket<Image, Sprite>
{
    protected override void SetValue(Sprite value)
    {
        valueUI.sprite = value;
        valueUI.preserveAspect = true;
    }
}
