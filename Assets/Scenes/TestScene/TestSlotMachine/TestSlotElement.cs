using UnityEngine.UI;

public class TestSlotElement : SlotMachineElement<int>
{
    public Text text;
    public override void Init(int data)
    {
        text.text = data.ToString();
        gameObject.name = data.ToString();
    }
}
