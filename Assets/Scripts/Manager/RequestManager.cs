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

public partial class RequestManager : MonoSingleton<RequestManager>
{
    public delegate void OnResponse(ResponseData data);
    private const string ACTAPI = @"https://app.jamtalk.live/shop/api/";
    private const string STTAPI= @"https://api.maum.ai/stt/cnnSttSimple";

    public void RequestAct<T>(T param, OnResponse onResponse) where T:IParam
    {
        StartCoroutine(SendRequest(ACTAPI,param, onResponse));
    }
    public void RequestSTT<T>(T param, OnResponse onResponse) where T : IParam
    {
        StartCoroutine(SendRequest(STTAPI,param, onResponse));
    }


    private IEnumerator SendRequest(string url, IParam param, OnResponse onResponse)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, param.GetForm()))
        {
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
                    File.WriteAllText(Application.streamingAssetsPath + "/Test.txt", response.GetLog());
                    Debug.LogFormat(
                        response.GetLog());
                    //onResponse.Invoke(response);
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
