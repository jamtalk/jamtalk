using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public delegate void delegateSTTresult(string result);
public delegate void delegateMSG(string msg);
public class STTManager : MonoBehaviour
{
    private static STTManager instance;
    private static GameObject content;
    public static STTManager Instance
    {
        get
        {
            if (instance == null)
            {
                content = new GameObject();
                content.name = "STTManager"; // 오브젝트 이름 틀리면 안됩니다.
                DontDestroyOnLoad(content);
                instance = content.AddComponent<STTManager>();
            }
            return instance;
        }
    }
    // 안드로이드와 델리게이트 선언입니다.

    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;

    public event Action onStarted;
    public event Action onEnded;
    public event Action<string> onError;
    public event Action<string> onResult;
    private string[] errorCodes = {
        "",

    };
    //안드로이드 연결은 Awake()에서 해줍니다.
#if UNITY_ANDROID && !UNITY_EDITOR
    void Awake()
    {
        //AJC = new AndroidJavaClass("com.gamejange.sttlibrary.STTLibrary");
        //AJO = AJC.GetStatic<AndroidJavaObject>("currentActivity");
        //일단 아까 plugin의 context를 설정해주기 위해
        //유니티 자체의 UnityPlayerActivity를 가져옵시다.
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

        }

        //클래스를 불러와줍니다.
        //패키지명 + 클래스명입니다.
        using (javaClass = new AndroidJavaClass("com.gamejange.sttlibrary.STTLibrary"))
        {
            if (javaClass != null)
            {
                //아까 싱글톤으로 사용하자고 만들었던 static instance를 불러와줍니다.
                javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("Instance");
                //Context를 설정해줍니다.
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(SetContext));
            }
        }
    }
private void SetContext()
    {
        javaClassInstance.Call("SetContext", activityContext);
    }
#endif
    //안드로이드에 있는 함수를 호출합니다. string lang은 "en-US" 넣어주면 됩니다.
    public void StartSTT(string lang)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        javaClassInstance.Call("StartSpeechReco", lang);
#else
        onEnded?.Invoke();
#endif
    }

    // 안드로이드에서 호출할 유니티 메소드 두개입니다. 에러메세지 받을 것과 음성인식 결과 받을 것입니다.
    public void msgUnity(string msg)
    {
        Debug.LogFormat("STT 메세지 도착 : {0}", msg);
        switch (msg)
        {
            case "START":
                Debug.Log("시작");
                onStarted?.Invoke();
                break;
            case "END":
                Debug.Log("끝");
                onEnded?.Invoke();
                break;
            default:
                Debug.LogFormat("에러 : {0}", msg);
                onEnded?.Invoke();
                onError?.Invoke(msg);
                break;
        }
    }

    public void sttUnity(string result)
    {
        Debug.LogFormat("결과 도착 : {0}", result);
        onResult?.Invoke(result);
    }

}
public class STTResult
{
    public bool Successed;
    public string value;

    public STTResult(bool successed, string value)
    {
        Successed = successed;
        this.value = value;
    }
}
