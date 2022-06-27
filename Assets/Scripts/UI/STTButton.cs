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
    public bool isRecording { get; private set; } = false;
    private void Awake()
    {
        recorder.onSTT += OnSTTEnded;
        button.onClick.AddListener(() =>
        {
            if (isRecording)
                recorder.Stop();
            else
                recorder.Record();
            isRecording = !isRecording;
            onRecord?.Invoke(isRecording);
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
