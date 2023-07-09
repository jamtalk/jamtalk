using System.Collections;
using UnityEngine;
using GJGameLibrary.DesignPattern;
using UnityEngine.Networking;
using System.Linq;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using GJGameLibrary.Util;
using System.Threading.Tasks;
using GJGameLibrary;
using System.Text;

public partial class RequestManager : MonoSingleton<RequestManager>
{
    public delegate void OnResponse(ResponseData data);
    private const string ROOT = @"https://metafamilylibrary.com/shop/api/";
    private const string ROOTSTT= @"https://api.maum.ai/stt/cnnSttSimple";

    /// <summary>
    /// Google STT 계정 정보
    /// </summary>
    private const string GoogleSTTAPI = @"https://speech.googleapis.com/v1/speech:recognize";
    private const string GoogleSTTKEY = @"AIzaSyCDogfeweKC8GhDo0LVfPrkqp7 - aOA0QrA";

    public void Request<T>(T param, OnResponse onResponse) where T:ActParam
    {
        StartCoroutine(SendRequest(ROOT+string.Format("?act={0}",param.act),param, onResponse));
    }
    public void RequestSTT<T>(T param, OnResponse onResponse) where T : IParam
    {
        StartCoroutine(SendRequest(ROOTSTT,param, onResponse));
    }

    public void RequestGoogleSTT(VoiceRecorder param, Action<string> onResponse) 
    {
        if (Microphone.IsRecording(Microphone.devices[0]))
            param.Stop();

        var value = new GoogleSTTProtocal(param.clip).audio.content;
        var languageCode = "en";

        StartCoroutine(GoogleSpeechToText(value, languageCode, value =>
        {
            onResponse?.Invoke(value);
            if(!string.IsNullOrEmpty(value))
                Debug.Log(value);
        }));
    }

    IEnumerator GoogleSpeechToText(string base64DecodedContent, string languageCode, Action<string> callbackContent)
    {
        // Create json object
        STTRequestBody stt = new STTRequestBody()
        {
            config = new Config() { languageCode = languageCode },
            audio = new Audio() { content = base64DecodedContent }
        };

        var requestBody = JsonConvert.SerializeObject(stt);
        //string requestBody = JsonUtility.ToJson(stt);
        Debug.Log("[SpeechToText] request body json: " + requestBody);

        string requestUri = $"https://speech.googleapis.com/v1p1beta1/speech:recognize?key={GoogleSTTKEY}";
        //Debug.Log("[SpeechToText] request uri: " + requestUri);

        using (UnityWebRequest uwr = UnityWebRequest.Post(requestUri, ""))
        {
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestBody));
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                string responseBody = uwr.downloadHandler.text;
                var isRecognition = responseBody.Contains("result");
                Debug.LogFormat("{0} [SpeechToText] response body json: {1}" ,isRecognition ,responseBody);

                STTResponseBody sttResponse = JsonUtility.FromJson<STTResponseBody>(responseBody);

                if (isRecognition)
                    callbackContent?.Invoke(sttResponse.results[0].alternatives[0].transcript);
                else
                    callbackContent?.Invoke(string.Empty);
            }
        }
    }

    private IEnumerator SendRequest(string url, IParam param, OnResponse onResponse)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, param.GetForm()))
        {
            Action loading = null;
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != eSceneName.AD_001.ToString())
                loading = PopupManager.Instance.ShowLoading();
            //www.SetRequestHeader("Content-Type", "multipart/form-data; boundary=<calculated when request is sent>");
            yield return www.SendWebRequest();
            loading?.Invoke();
            object data = null;
            bool success = true;
            Debug.Log(url);
            Debug.LogFormat("{0} request result : {1}\nURL : {2}\n------------------\nData\n{3}\n------------------\nResult\n{4}"
                , url.Split('/').Last().Split('=').Last()
                , success, url, param.ToString(), JObject.Parse(www.downloadHandler.text));
            try
            {
                data = RemoveEmptyChildren(JObject.Parse(www.downloadHandler.text));
                //Debug.LogFormat("{0}\n\n{1}", JObject.Parse(www.downloadHandler.text), data);
                success = true;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("URL : {0}\nParsingError! Exception : {1}\n-------------\nResult :{2}", url, e, www.downloadHandler.text);
                success = false;
            }
            finally
            {
                var response = new ResponseData(success, data);
                if (success)
                {
                    onResponse?.Invoke(response);
                }
            }
        }
    }

    public JToken RemoveEmptyChildren(JToken token)
    {
        if (token.Type == JTokenType.Object)
        {
            JObject copy = new JObject();
            foreach (JProperty prop in token.Children<JProperty>())
            {
                JToken child = prop.Value;
                if (child.HasValues)
                {
                    child = RemoveEmptyChildren(child);
                }
                if (!IsEmpty(child))
                {
                    if(!string.IsNullOrEmpty(prop.Value.ToString()) && prop.Value != null)
                        copy.Add(prop.Name, child);
                }
            }
            return copy;
        }
        else if (token.Type == JTokenType.Array)
        {
            JArray copy = new JArray();
            foreach (JToken item in token.Children())
            {
                JToken child = item;
                if (child.HasValues)
                {
                    child = RemoveEmptyChildren(child);
                }
                if (!IsEmpty(child))
                {
                    copy.Add(child);
                }
            }
            return copy;
        }
        return token;
    }
    public bool IsEmpty(JToken token)
    {
        return (token.Type == JTokenType.Null);
    }

}

public class ResponseData
{
    public object data { get; private set; }
    public bool HasResult => data != null;
    public bool RequestSuccess { get; private set; }
    public ResponseData(bool success, object data = null)
    {
        RequestSuccess = success;
        this.data = data;
    }
    public string GetLog()
    {
        return string.Format(
            "Request Success : {0}\n" +
            "HasResult : {1}\n" +
            "---Result---\n" +
            "{2}\n" +
            "---------",
            RequestSuccess,
            HasResult,
            HasResult ? data.ToString() : string.Empty);
    }
    public T GetResult<T>() where T : class => JsonConvert.DeserializeObject<T>(data.ToString());
}