using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(VoiceRecorder))]
public class STTButton : MonoBehaviour
{
    public VoiceRecorder recorder => GetComponent<VoiceRecorder>();
    public GameObject onRecoding;
    public EventSystem eventSystem;
    public Button button;
    public event Action<string> onSTT;
    public event Action<bool> onRecord;
    private void Awake()
    {
        recorder.onSTT += OnSTTEnded;
        STTManager.Instance.onStarted += () => onRecord?.Invoke(true);
        STTManager.Instance.onEnded += () =>
        {
            onRecord?.Invoke(false);
            eventSystem.enabled = true;
        };
        STTManager.Instance.onError += (error) => AndroidPluginManager.Instance.Toast("오류 발생 : " + error);
        STTManager.Instance.onResult += (value) => onSTT(value);
        button.onClick.AddListener(() =>
        {
            eventSystem.enabled = false;
#if UNITY_EDITOR
            recorder.Record();
#elif UNITY_ANDROID
            STTManager.Instance.StartSTT("en-US");
#endif
        });
    }
    private void OnDisable()
    {
        recorder.onSTT -= OnSTTEnded;
    }

    private void OnSTTEnded(bool success, string result)
    {
        onRecoding.SetActive(false);
        button.interactable = true;
        eventSystem.enabled = true;
        if (success)
            onSTT?.Invoke(result);
        else
            onSTT?.Invoke(string.Empty);
    }
}
