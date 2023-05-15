using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AlphabetToggle110 : BaseDragableToggle<eAlphabet>
{
    private eAlphabetType GetAlphabetType() => transform.GetSiblingIndex() == 0 ? eAlphabetType.Upper : eAlphabetType.Lower;
    protected override Sprite GetBackground()=> GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.NeonFulcolor, GetAlphabetType(), value);
    protected override Sprite GetSprite()=> GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, GetAlphabetType(), value);
}