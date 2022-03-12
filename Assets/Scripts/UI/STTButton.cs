using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class STTButton : MonoBehaviour
{
    public GameObject onRecoding;
    public EventSystem eventSystem;
    public Button button;
    private void Awake()
    {
        STTManager.Instance.onEnded += OnSTTEnded;

        button.onClick.AddListener(() =>
        {
            onRecoding.SetActive(true);
            button.interactable = false;
            eventSystem.enabled = false;
            STTManager.Instance.StartSTT("en-US");
        });
    }
    private void OnDisable()
    {
        STTManager.Instance.onEnded -= OnSTTEnded;
    }

    private void OnSTTEnded()
    {
        onRecoding.SetActive(false);
        button.interactable = true;
        eventSystem.enabled = true;
    }
}
