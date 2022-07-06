using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigraphsBingoButton : BaseBingoButton<DigraphsSource, Text>
{
    protected override void SetViewer()
    {
        viewer.text = value.value;
    }
}
