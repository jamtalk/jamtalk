using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card120 : MonoBehaviour
{
    public TurningCard card;
    public ImageButton imageButton;
    public event Action onClick;
    public void Init(Sprite sprite)
    {
        imageButton.SetSprite(sprite);
        card.Init(alwaysBackDisable: true,callback:()=> {
            if (!card.IsFornt)
                onClick?.Invoke();
        });
        card.SetFront();
    }
}
