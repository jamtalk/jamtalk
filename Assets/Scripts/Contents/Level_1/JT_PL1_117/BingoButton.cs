using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BingoButton : BaseBingoButton<AlphabetData,Image>
{
    [SerializeField]
    private eAlphabetStyle style;
    protected override void SetViewer()
    {
        viewer.sprite = GameManager.Instance.GetAlphbetSprite(style, eAlphabetType.Upper, value.Alphabet);
        viewer.preserveAspect = true;
    }
}
