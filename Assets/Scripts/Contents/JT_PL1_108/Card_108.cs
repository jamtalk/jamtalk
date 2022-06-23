using System;
using UnityEngine;

[RequireComponent(typeof(TurningCard))]
public class Card_108 : MonoBehaviour
{
    public TurningCard turnner => GetComponent<TurningCard>();
    public ImageButton imageButton;
    [HideInInspector]
    public WordSource data;
    [HideInInspector]
    public Action<WordSource> onClick;
    [HideInInspector]
    public Func<WordSource, bool> checkVaild;
    private void Awake()
    {
        turnner.onClick += () =>
        {
            CheckVaild();
            onClick?.Invoke(data);
        };
    }
    private void CheckVaild()
    {
        var vaild = checkVaild.Invoke(data);
        if (vaild)
        {
            turnner.Turnning();
        }
    }
    public void Init(WordSource data)
    {
        this.data = data;
        imageButton.sprite = data.sprite;
        turnner.Init(1, () =>
        {
            if (!turnner.IsFornt)
                onClick?.Invoke(data);
        }, true, true);
    }
}
