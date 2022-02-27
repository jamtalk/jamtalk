using UnityEngine.UI;

public class TextRocket : Rocket<Text, string>
{
    protected override void SetValue(string value)
    {
        valueUI.text = value;
    }
}
