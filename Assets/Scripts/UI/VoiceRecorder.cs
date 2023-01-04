using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System.Collections;
using sw = System.Diagnostics;

[RequireComponent(typeof(AudioSource))]
public class VoiceRecorder : MonoBehaviour
{
    public AudioSource source => GetComponent<AudioSource>();
    public string deviceName;
    public Action<bool, string> onSTTResult;
    public Action<bool> onDecibelResult;
    public Action onStopRecord;
    public bool isLetter = false;

    private Coroutine decibelRoutine;
    private Coroutine recordRoutine;

    public AudioClip clip
    {
        get => source.clip;
        private set => source.clip = value;
    }

    public void Record(bool isSTT = true)
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

            recordRoutine = StartCoroutine(RecordStopRoutine());
            
            if(!isSTT)
                decibelRoutine = StartCoroutine(DecibelRoutine());
        }
    }
    public void Stop()
    {
        Debug.Log("종료");
        onStopRecord?.Invoke();
        source.Stop();
        Microphone.End(deviceName);

        if(decibelRoutine != null)
        {
            StopCoroutine(decibelRoutine);
            decibelRoutine = null;
        }

        if (recordRoutine != null)
        {
            StopCoroutine(recordRoutine);
            recordRoutine = null;
        }
    }

    public void RecordOrSendSTT(bool isSTT = true)
    {
        if (Microphone.IsRecording(deviceName))
        {
            if (isSTT)
                SendSTT();
            else
                DecibelMeasurement();
        }
        else
            Record(isSTT);
    }

    public void Play()
    {
        Debug.Log("실행");
        source.Play();
    }

    public void SendSTT()
    {
        if (Microphone.IsRecording(deviceName))
            Stop();

        if (decibelRoutine != null)
        {
            StopCoroutine(decibelRoutine);
            decibelRoutine = null;
        }

        Debug.Log(GetAveragedVolume());

        RequestManager.Instance.RequestGoogleSTT(this, (response) =>
        {
            //Debug.Log(response.GetLog());
            var isSTTResult = !string.IsNullOrEmpty(response);
            onSTTResult?.Invoke(isSTTResult, response);
        });
    }   

    private IEnumerator RecordStopRoutine(float recordTime = 5f)
    {
        sw.Stopwatch sw = new sw.Stopwatch();
        sw.Start();

        while (sw.ElapsedMilliseconds * 0.001 < recordTime)
            yield return null;

        sw.Stop();
        sw.Reset();

        Stop();
    }

    private void DecibelMeasurement()
    {
        onDecibelResult?.Invoke(true);
    }

    private IEnumerator DecibelRoutine()
    {
        while (!(Microphone.GetPosition(null) > 0)) { }

        source.Play();
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (GetAveragedVolume() > 70)
            {
                Debug.Log(GetAveragedVolume());

                yield return new WaitForSecondsRealtime(1f);

                DecibelMeasurement();
                Stop();
                break;
            }
        }
    }

    private float GetAveragedVolume()
    {
        float[] data = new float[256];
        float temp = 0;
        source.GetOutputData(data, 0);

        foreach (var item in data)
            temp += Mathf.Abs(item);

        return (temp / 256) * 10000;
    }
}
