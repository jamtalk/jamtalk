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
    public AudioSource source => GetComponent<AudioSource>();
    private string deviceName;
    public Action<bool, string> onSTTResult;

    public AudioClip clip
    {
        get => source.clip;
        private set => source.clip = value;
    }

    public void Record()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("마이크가 없습니다");
        }
        else
        {
            deviceName = Microphone.devices[0];
            Debug.LogFormat("녹음 시작 : {0}",deviceName);
            source.clip = Microphone.Start(deviceName, false, 10, 44100);//서버 송신용 8000
        }
    }
    public void Stop()
    {
        Debug.Log("종료");
        source.Stop();
        Microphone.End(deviceName);
    }

    public void SendSTT()
    {
        if (Microphone.IsRecording(deviceName))
            Stop();

        RequestManager.Instance.RequestGoogleSTT(this, (response) =>
        {
            //Debug.Log(response.GetLog());
            var isSTTResult = !string.IsNullOrEmpty(response);
            onSTTResult?.Invoke(isSTTResult, response);
        });
    }

    public void RecordOrSendSTT()
    {
        if (Microphone.IsRecording(deviceName))
            SendSTT();
        else
            Record();
    }
    public void Play()
    {
        Debug.Log("실행");
        source.Play();
    }
}
