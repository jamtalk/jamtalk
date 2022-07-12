using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card120 : MonoBehaviour
{
    public TurningCard card;
    public ImageButton imageButton;
    public event Action<AlphabetWordsData> onClick;
    public AlphabetWordsData data;
    public void Init(AlphabetWordsData data)
    {
        this.data = data;
        imageButton.sprite = data.sprite;
        card.Init(alwaysBackDisable: true,callback:()=> {
            if (!card.IsFornt)
                onClick?.Invoke(data);
        });
        card.SetFront();
    }
}
