using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GJGameLibrary.DesignPattern;
//using System.Runtime.InteropServices;

public class AndroidPluginManager : MonoSingleton<AndroidPluginManager>
{
    private const string package = "com.gamejange.unityplugin";
    private AndroidJavaClass pluginClass;
    private AndroidJavaObject pluginInstance;

    public Action onStartCallBack;
    public Action onDoneCallback;
    public Action<string> onSpeakRangeCallback;
    [Range(0.5f, 2)]
    public float pitch = 1f; //[0.5 - 2] Default 1
    [Range(0.5f, 2)]
    public float rate = 1f; //[min - max] android:[0.5 - 2] iOS:[0 - 1]
    public override void Initialize()
    {
        pluginClass = new AndroidJavaClass(string.Format("{0}.{1}", package, "UnityPlugin"));
        pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("Instance",gameObject.name);
        SettingTTS(1f, .8f);
        base.Initialize();
    }
    public void SettingTTS(float _pitch, float _rate)
    {
        pitch = _pitch;
        rate = _rate;
#if UNITY_EDITOR
#elif UNITY_IPHONE
        //_TAG_SettingSpeak("kr", pitch, rate / 2);
#elif UNITY_ANDROID
        pluginInstance.Call("OnSettingSpeak", pitch, rate);
#endif
    }
    public void PlayTTS(string _message, Action onCompleted = null)
    {
        onDoneCallback = onCompleted;
#if UNITY_EDITOR
        Debug.Log("TTS : "+_message);
        onDoneCallback?.Invoke();
#elif UNITY_IPHONE
        //_TAG_StartSpeak(_message);
#elif UNITY_ANDROID
        pluginInstance.Call("OnStartSpeak", _message);
#endif
    }
    public void StopTTS()
    {
#if UNITY_EDITOR
        Debug.Log("TTS Stoped");
#elif UNITY_IPHONE
        //_TAG_StopSpeak();
#elif UNITY_ANDROID
        pluginInstance.Call("OnStopSpeak");
#endif
    }

    public void onSpeechRange(string _message)
    {
        if (onSpeakRangeCallback != null && _message != null)
        {
            onSpeakRangeCallback(_message);
        }
        Debug.Log("onSpeechRange : " + _message);
    }
    public void onStart(string _message)
    {
        if (onStartCallBack != null)
            onStartCallBack();
        Debug.Log("TTS 시작 : " + _message);
    }
    public void onDone(string _message)
    {
        onDoneCallback?.Invoke();
        Debug.Log("TTS 끝남 : " + _message);
    }
    public void onError(string _message)
    {
        Debug.LogError("TTS 에러발생 : " + _message);
    }
    public void onMessage(string _message)
    {
        Debug.Log("메세지 : " + _message);
    }
    /** Denotes the language is available for the language by the locale, but not the country and variant. */
    public const int LANG_AVAILABLE = 0;
    /** Denotes the language data is missing. */
    public const int LANG_MISSING_DATA = -1;
    /** Denotes the language is not supported. */
    public const int LANG_NOT_SUPPORTED = -2;
    public void onSettingResult(string _params)
    {
        int _error = int.Parse(_params);
        string _message = "";
        if (_error == LANG_MISSING_DATA || _error == LANG_NOT_SUPPORTED)
        {
            _message = "This Language is not supported";
        }
        else
        {
            _message = "This Language valid";
        }
        Debug.Log(_message);
    }
    public void OnInitializing(string param)
    {
        Debug.Log("TTS Initializing : " + param);
    }
    public void Toast(string message)
    {

#if UNITY_EDITOR
        Debug.Log("Toast : " + message);
#elif UNITY_IPHONE
        //_TAG_StopSpeak();
#elif UNITY_ANDROID
        pluginInstance.Call("Toast",message);
#endif
    }

//#if UNITY_IPHONE
//        [DllImport("__Internal")]
//        private static extern void _TAG_StartSpeak(string _message);

//        [DllImport("__Internal")]
//        private static extern void _TAG_SettingSpeak(float _pitch, float _rate);

//        [DllImport("__Internal")]
//        private static extern void _TAG_StopSpeak();
//#endif
}

