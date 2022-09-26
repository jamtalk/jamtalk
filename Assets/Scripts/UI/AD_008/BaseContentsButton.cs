using UnityEngine;
using UnityEngine.UI;

public abstract class BaseContentsButton : MonoBehaviour
{
    public Button button;
    protected abstract void LoadScene();
    protected virtual void Awake()
    {
        button.onClick.AddListener(LoadScene);
    }
}
