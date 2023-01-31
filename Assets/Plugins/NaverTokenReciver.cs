using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//Scripted by WonWoo
public class NaverTokenReciver :MonoBehaviour
{

    private Action<bool, NaverAuthInfo> onSigned;


    private static string OAUTH_CLIENT_ID = "Noa5boot3VMLazFeyHLP";//���̹� ���ø����̼� ��Ͻ� �߱޹޴� ���̵�
    private static string OAUTH_CLIENT_SECRET = "CNnEY3bxe9";//���̹� ���ø����̼� ��Ͻ� �߱޹޴� �ڵ�(���� ����)
    private static string OAUTH_CLIENT_NAME = "Jamtalk English";//���ӽ� �� �̸�
    private static NaverTokenReciver instance = null;
    public static NaverTokenReciver Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<NaverTokenReciver>();
                instance.name = "NaverLoginPlugin";//���� ������Ʈ �̸��� ������ message�� �����Ƿ� �̸� ���� �ٲٸ� �ȵ�!
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
    /// ó�� ���۽� �����ؾ��ϴ� �ڵ�(�ݵ��)
    /// </summary>
    /// <param name="OAUTH_CLIENT_ID">���̹� ���ø����̼� ��Ͻ� �߱޹޴� ���̵�</param>
    /// <param name="OAUTH_CLIENT_SECRET">���̹� ���ø����̼� ��Ͻ� �߱޹޴� �ڵ�(���� ����)</param>
    /// <param name="OAUTH_CLIENT_NAME">���ӽ� �� �̸�</param>
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
    /// unity message�� ���� callback��
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
                    Debug.LogError("�α��� ����!");
                    onSigned?.Invoke(false, null);
                    break;
                case Message.SUCCESS:
                    Debug.Log("�α��� ����!"+ callBack.token);
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
            Debug.LogFormat("���̹� �α��� ��� ����\n{0}", www.downloadHandler.text);
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
        public Message message; //0 = ����, 1 = ����
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
