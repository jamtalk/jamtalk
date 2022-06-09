using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetImage105 : MonoBehaviour
{
    public Image imageLower;
    public Image imageUpper;
    private void Awake()
    {
        Init(eAlphabet.A);
    }
    public void Init(eAlphabet value)
    {
        imageLower.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Lower, value);
        imageUpper.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, eAlphabetType.Upper, value);
        imageLower.preserveAspect = true;
        imageUpper.preserveAspect = true;
    }
}
