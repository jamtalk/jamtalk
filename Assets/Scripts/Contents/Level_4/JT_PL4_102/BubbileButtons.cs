using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbileButtons : ImageButton
{
    public DigraphsWordsData data;
    public RectTransform rt => GetComponent<RectTransform>();

    public void Init(DigraphsWordsData data)
    {
        this.data = data;
        sprite = data.sprite;
        button.interactable = true;
    }
}
