using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class AlphabetButton : MonoBehaviour
{
    public Button button => GetComponent<Button>();
    public Image image;
    public event Action<eAlphabet> onClick;
    public eAlphabet value { get; private set; }
    public eAlphbetType type;
    private void Awake()
    {
        button.onClick.AddListener(() => onClick?.Invoke(value));
    }
    public void Init(eAlphabet alphabet, eAlphbetStyle style, eAlphbetType type)
    {
        value = alphabet;
        this.type = type;
        image.sprite = GameManager.Instance.GetAlphbetSprite(style, type, alphabet);
        image.preserveAspect = true;
    }
}
