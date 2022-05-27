using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetToggle110 : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Dropable110 drop;
    public Dragable110 drag => throwElement.GetComponent<Dragable110>();

    public RectTransform throwElement;
    public AudioSinglePlayer audioPlayer;
    public AudioClip clipOn;
    public bool isOn
    {
        get => toggle.isOn;
        set
        {
            toggle.isOn = value;
            audioPlayer.Play(clipOn);
            onOn?.Invoke();
        }
    }
    public event Action onOn;
    public eAlphabet value;
    private void Awake()
    {
        drop.onDroped += () => isOn = true;
    }
    public void Init(eAlphabet alphabet)
    {
        var index = parent.GetSiblingIndex();
        var type = index == 0 ? eAlphbetType.Upper : eAlphbetType.Lower;
        var sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.FullColor, type, alphabet);
        value = alphabet;

        for(int i = 0;i < images.Length; i++)
        {
            images[i].sprite = sprite;
            images[i].preserveAspect = true;
        }
        background.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.NeonFulcolor, type, alphabet);
        drag.Init(alphabet, this);
        drop.Init(alphabet, this);
    }
}
