using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupButton<T> : MonoBehaviour where T:BasePopup
{
    [SerializeField]
    private GameObject popup;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            PopUp();
        });
    }
    protected virtual T PopUp()
    {
        return PopupManager.Instance.Popup<T>(popup);
    }
}
