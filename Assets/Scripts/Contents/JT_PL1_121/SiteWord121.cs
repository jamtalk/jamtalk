using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiteWord121 : MonoBehaviour
{
    public GameObject mask;
    public bool isCorrect;
    public Image baseImage;
    public Image throwingImage;
    public event Action<string> onCorrect;
    private Drageable121 dragEelemnt => throwingImage.GetComponent<Drageable121>();
    public RectTransform throwingElement => throwingImage.GetComponent<RectTransform>();
    public void Init(Sprite sprite)
    {
        isCorrect = false;
        if (sprite != null)
            name = sprite.name;
        else
            name = "NULL";

        baseImage.sprite = sprite;
        baseImage.preserveAspect = true;
        throwingImage.sprite = sprite;
        throwingImage.preserveAspect = true;
        dragEelemnt.Init(GetInstanceID());
        dragEelemnt.onCorrect += ()=>
        {
            isCorrect = true;
            onCorrect?.Invoke(sprite.name);
        };
    }
}
