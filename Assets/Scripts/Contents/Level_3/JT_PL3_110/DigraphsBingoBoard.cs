using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigraphsBingoBoard : BaseBingoBoard<DigraphsWordsData, Text, DigraphsBingoButton>
{
    public override bool CheckOn(int index) => buttons[index].isOn;
}
