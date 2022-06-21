using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class VoiceRecorder : MonoBehaviour
{
    public event Action<bool, string> onSTT;
    public bool isRecording { get; private set; } = false;
    public AudioSource source => GetComponent<AudioSource>();
    private string deviceName;
    public AudioClip clip
    {
        get => source.clip;
        private set => source.clip = value;
    }
#if UNITY_IOS
    public string correct;
#endif
    public void Record()
    {
#if UNITY_IOS
        Stop();
#else
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("마이크가 없습니다");
        }
        else
        {
            Debug.Log("녹음 시작");
            deviceName = Microphone.devices[0];
            source.clip = Microphone.Start(deviceName, false, 5, 8000);
            //source.Play();
        }
#endif
    }
    public void Stop()
    {
#if UNITY_IOS
        onSTT?.Invoke(true, correct);
#else
        Debug.Log("종료");
        source.Stop();
        Microphone.End(deviceName);
        var param = new STTParam(clip);
        RequestManager.Instance.RequestSTT(param, (response) =>
        {
            var result = response.GetResult<STTResult>();
            onSTT?.Invoke(result.IsSuccessed, result.result);
        });
#endif
    }
    public void Play()
    {
        Debug.Log("실행");
        source.Play();
    }
}
