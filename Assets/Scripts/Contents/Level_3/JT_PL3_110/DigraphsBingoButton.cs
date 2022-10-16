using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigraphsBingoButton : BaseBingoButton<DigraphsWordsData, Text>
{
    protected override void SetViewer()
    {
        viewer.text = value.key;
        strValue = value.key;
    }
}
