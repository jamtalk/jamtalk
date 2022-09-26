using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ReversIntractableToggle : MonoBehaviour
{
    private void Awake()
    {
        var toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(value =>
        {
            toggle.interactable = !value;
        });
    }
}
