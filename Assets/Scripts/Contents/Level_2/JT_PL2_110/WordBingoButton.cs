using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordBingoButton : BaseBingoButton<AlphabetWordsData, Text>
{
    protected override void SetViewer()
    {
        viewer.text = value.key;
    }
}
