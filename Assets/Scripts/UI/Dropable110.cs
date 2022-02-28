using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class Dropable110 : MonoBehaviour
{
    public event Action onDroped;
    public AlphabetToggle110 toggle { get; private set; }
    public eAlphabet value { get; private set; }
    public void Drop() => onDroped?.Invoke();
    public void Init(eAlphabet value, AlphabetToggle110 toggle)
    {
        this.value = value;
        this.toggle = toggle;
    }
}
