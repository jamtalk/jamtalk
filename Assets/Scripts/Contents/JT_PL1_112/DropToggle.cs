using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DropToggle : MonoBehaviour
{
    public Image image;
    public AudioSinglePlayer audioPlayer;
    public eAlphabet alphabet { get; private set; }
    [SerializeField]
    private bool _isOn = false;
    public event Action onDroped; 
    public bool isOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            var color = Color.white;
            color.a = isOn ? 1f : .5f;
            image.color = color;
            if (value)
            {
                audioPlayer.Play();
                onDroped?.Invoke();
            }
        }
    }
    public void Init(eAlphabet alphabet, bool isOn = false)
    {
        this.alphabet = alphabet;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Dino,eAlphbetType.Upper,alphabet);
        image.preserveAspect = true;
        this.isOn = isOn;
        this.alphabet = alphabet;
    }
}
