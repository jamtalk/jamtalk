using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card120 : MonoBehaviour
{
    public TurningCard card;
    public ImageButton imageButton;
    public event Action<WordsData.WordSources> onClick;
    public WordsData.WordSources data;
    public void Init(WordsData.WordSources data)
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
