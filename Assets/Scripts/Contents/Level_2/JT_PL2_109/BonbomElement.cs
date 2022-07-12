using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonbomElement : UIMover
{
    public Sprite[] sprites;

    private void OnEnable()
    {
        gameObject.GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
        gameObject.GetComponent<Image>().preserveAspect = true;
    }
}
