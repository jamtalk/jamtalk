using UnityEngine.UI;

public class TextRocket : Rocket<Text, string>
{
    public Text text;
    protected override void SetValue(string value)
    {
        valueUI.text = value;
    }
}
