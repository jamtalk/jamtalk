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
    private const string ACTAPI = @"http://app.jamtalk.live/shop/api/";
    private const string STTAPI= @"https://api.maum.ai/stt/cnnSttSimple";

    /// <summary>
    /// Google STT 계정 정보
    /// </summary>
    private const string GoogleSTTAPI = @"https://speech.googleapis.com/v1/speech:recognize";
    private const string GoogleSTTKEY = @"AIzaSyCDogfeweKC8GhDo0LVfPrkqp7 - aOA0QrA";

    public void RequestAct<T>(T param, OnResponse onResponse) where T:IParam
    {
        StartCoroutine(SendRequest(ACTAPI,param, onResponse));
    }
    public void RequestSTT<T>(T param, OnResponse onResponse) where T : IParam
    {
        StartCoroutine(SendRequest(STTAPI,param, onResponse));
    }

    public void RequestGoogleSTT(VoiceRecorder param, OnResponse onResponse) 
    {
        if (Microphone.IsRecording(Microphone.devices[0]))
            param.Stop();

        var value = new GoogleSTTProtocal(param.clip).audio.content;
        var languageCode = "en";

        StartCoroutine(GoogleSpeechToText(value, languageCode, value =>
        {
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
                Debug.Log("[SpeechToText] response body json: " + responseBody);

                STTResponseBody sttResponse = JsonUtility.FromJson<STTResponseBody>(responseBody);
                callbackContent?.Invoke(sttResponse.results[0].alternatives[0].transcript);
            }
        }
    }

    private IEnumerator SendRequest(string url, IParam param, OnResponse onResponse)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, param.GetForm()))
        {
            //www.SetRequestHeader("Content-Type", "multipart/form-data; boundary=<calculated when request is sent>");
            yield return www.SendWebRequest();
            object data = null;
            bool success = true;
            try
            {
                data = RemoveEmptyChildren(JObject.Parse(www.downloadHandler.text));
                success = true;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("ParsingError!\nException : {0}\n-------------\nResult :{1}", e, www.downloadHandler.text);
                success = false;
            }
            finally
            {
                var response = new ResponseData(success, data);
                if (success)
                {
                    Debug.Log(data.ToString());
                    onResponse.Invoke(response);
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