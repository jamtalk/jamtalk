using UnityEngine;

public class DoubleClickButton4_104 : DoubleClickButton
{
    public GameObject incorrectMark;
    public int inCorrectCnt = 0;

    public DigraphsWordsData data;

    public void Init(DigraphsWordsData data)
    {
        this.data = data;
        isOn = false;
        incorrectMark.SetActive(false);
        inCorrectCnt = 0;
        button.interactable = true;
    }
}