using System;
using UnityEngine;

[RequireComponent(typeof(TurningCard))]
public class Card_108 : MonoBehaviour
{
    public TurningCard turnner => GetComponent<TurningCard>();
    public ImageButton imageButton;
    [HideInInspector]
    public string currentValue;
    public Action<string> onClick;
    public void Init(string value)
    {
        imageButton.SetSprite(GameManager.Instance.GetSpriteWord(value));
        currentValue = value;
        turnner.Init(1, () =>
        {
            if (!turnner.IsFornt)
                onClick?.Invoke(value);
        },alwaysBackDisable:true);
    }
}
