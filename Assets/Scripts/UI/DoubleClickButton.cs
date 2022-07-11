using UnityEngine;
using UnityEngine.UI;

public class DoubleClickButton : ImageButton
{
    [SerializeField]
    private Sprite spriteNormal;
    [SerializeField]
    private Sprite spriteSelected;
    public Button.ButtonClickedEvent onClick;
    public Button.ButtonClickedEvent onClickFirst;
    private bool _ison;
    public bool isOn
    {
        get => _ison;
        set
        {
            if (value)
                button.image.sprite = spriteSelected;
            else
                button.image.sprite = spriteNormal;
            _ison = value;
        }
    }
    protected virtual void Awake()
    {
        _ison = false;
        button.onClick.AddListener(() =>
        {
            if (isOn)
                onClick?.Invoke();
            else
            {
                isOn = true;
                onClickFirst?.Invoke();
            }
        });
    }
}
