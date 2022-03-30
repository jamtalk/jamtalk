using System;
using UnityEngine;

[RequireComponent(typeof(TurningCard))]
public class Card_108 : MonoBehaviour
{
    public TurningCard turnner => GetComponent<TurningCard>();
    public ImageButton imageButton;
    [HideInInspector]
    public string currentValue;
    [HideInInspector]
    public Action<string> onClick;
    [HideInInspector]
    public Func<string,bool> checkVaild;
    private void Awake()
    {
        turnner.onClick += () =>
        {
            CheckVaild();
            onClick?.Invoke(currentValue);
        };
    }
    private void CheckVaild()
    {
        var vaild = checkVaild.Invoke(currentValue);
        if (vaild)
        {
            turnner.Turnning();
        }
    }
    public void Init(string value)
    {
        imageButton.SetSprite(GameManager.Instance.GetSpriteWord(value));
        currentValue = value;
        turnner.Init(1, () =>
        {
            if (!turnner.IsFornt)
                onClick?.Invoke(value);
        }, true, true);
    }
}
