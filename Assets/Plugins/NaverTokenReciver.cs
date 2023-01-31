using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//Scripted by WonWoo
public class NaverTokenReciver :MonoBehaviour
{

    private Action<bool, NaverAuthInfo> onSigned;


    private static string OAUTH_CLIENT_ID = "Noa5boot3VMLazFeyHLP";//네이버 어플리케이션 등록시 발급받는 아이디
    private static string OAUTH_CLIENT_SECRET = "CNnEY3bxe9";//네이버 어플리케이션 등록시 발급받는 코드(보안 주의)
    private static string OAUTH_CLIENT_NAME = "Jamtalk English";//접속시 뜰 이름
    private static NaverTokenReciver instance = null;
    public static NaverTokenReciver Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<NaverTokenReciver>();
                instance.name = "NaverLoginPlugin";//게임 오브젝트 이름을 가지고 message를 받으므로 이름 절대 바꾸면 안됨!
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private AndroidJavaObject javaObj;
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject activity;
    private AndroidJavaObject app;

    /// <summary>
    /// 처음 시작시 설정해야하는 코드(반드시)
    /// </summary>
    /// <param name="OAUTH_CLIENT_ID">네이버 어플리케이션 등록시 발급받는 아이디</param>
    /// <param name="OAUTH_CLIENT_SECRET">네이버 어플리케이션 등록시 발급받는 코드(보안 주의)</param>
    /// <param name="OAUTH_CLIENT_NAME">접속시 뜰 이름</param>
    public void Awake()
    {
#if !UNITYEDITOR && UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            var pluginClass = new AndroidJavaClass("com.example.plugin.NaverLoginTest");

            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            app = activity.Call<AndroidJavaObject>("getApplicationContext");
            javaObj = pluginClass.CallStatic<AndroidJavaObject>("GetInstance");
            pluginClass.CallStatic("SetInfo", OAUTH_CLIENT_ID, OAUTH_CLIENT_SECRET, OAUTH_CLIENT_NAME);
    
            javaObj.Call("initData", app);
        }
#endif  
    }

    /// <summary>
    /// unity message를 통해 callback됨
    /// </summary>
    public void NaverLoginCallBack(string message)
    {

        ReciveTokenCallback callBack=null;
        Debug.Log("callBack!   "+message);
        try
        {
            callBack = JsonUtility.FromJson<ReciveTokenCallback>(message);

            switch (callBack.message)
            {
                case Message.FAIL:
                    Debug.LogError("로그인 실패!");
                    onSigned?.Invoke(false, null);
                    break;
                case Message.SUCCESS:
                    Debug.Log("로그인 성공!"+ callBack.token);
                    StartCoroutine(GetUserData(callBack.token));
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogFormat("Parsing Error!\n{0}", e);
        }
    }
    public void AccessLogin(Action<bool, NaverAuthInfo> callback)
    {
        this.onSigned = callback;
        if (Application.platform == RuntimePlatform.Android)
        {
            javaObj.Call("Click", activity);
        }
    }
    private IEnumerator GetUserData(string token)
    {
        var url = "https://openapi.naver.com/v1/nid/me";
        var form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("X-Naver-Client-Id", OAUTH_CLIENT_ID);
            www.SetRequestHeader("X-Naver-Client-Secret", OAUTH_CLIENT_SECRET);
            www.SetRequestHeader("Authorization", "Bearer " + token);
            yield return www.SendWebRequest();
            Debug.LogFormat("네이버 로그인 결과 도착\n{0}", www.downloadHandler.text);
            if (www.result == UnityWebRequest.Result.Success)
            {
                var result = JsonUtility.FromJson<NaverAuthInfo>(www.downloadHandler.text);
                onSigned?.Invoke(true, result);
            }
            else
            {
                onSigned?.Invoke(false, null);
            }
        }
    }

    private class ReciveTokenCallback
    {
        public Message message; //0 = 실패, 1 = 성공
        public string token;
    }
    private enum Message
    {
        FAIL,SUCCESS
    }
    public class NaverAuthInfo
    {
        public class NaverAuth
        {
            public string email;
            public string nickname;
            public string profile_image;
            public string age;
            public string gender;
            public string id;
            public string name;
            public string birthday;
            public string birthyear;
            public string mobile;
        }
        public string resultcode;
        public string message;
        public NaverAuth response;
    }
}
