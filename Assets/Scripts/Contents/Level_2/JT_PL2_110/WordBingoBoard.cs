using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordBingoBoard : BaseBingoBoard<AlphabetWordsData, Text, WordBingoButton>
{
    public override bool CheckOn(int index) => buttons[index].isOn;
}
