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

public delegate void OnAPIResponse(APIResponse response);
public partial class APIManager : MonoSingleton<APIManager>
{
    private const string ACTAPI = @"https://app.jamtalk.live/shop/api/";
    private const string STTAPI= @"";

    private void Request<T>(T param, OnAPIResponse onResponse) where T:IParam
    {
        StartCoroutine(SendRequest(param, onResponse));
    }


    private IEnumerator SendRequest(IParam param, OnAPIResponse onResponse)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(ACTAPI, param.ToParam()))
        {
            yield return www.SendWebRequest();

            try
            {
                var jobj = RemoveEmptyChildren(JObject.Parse(www.downloadHandler.text));
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("ParsingError!\n{0}\n\n{1}", e, www.downloadHandler.text);
            }
        }
    }

    IEnumerator SendAudio(AudioClip clip, APIResponse onUploaded)
    {
        WWWForm form = new WWWForm();

        var imageData = new byte[] { };
        var fileType = "";
        form.AddBinaryData("files", imageData, Guid.NewGuid().ToString() + "." + fileType, fileType);

        using (UnityWebRequest www = UnityWebRequest.Post(STTAPI, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Print response
                var result = JObject.Parse(www.downloadHandler.text)["Result"]["url"].ToString();
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

[Serializable]
public class APIResponse
{
    public class ResultAttribute
    {
        public int page = 0;
        public int max = 0;
        public int dataTotalCount = 0;
        public int dataCount = 0;
    }
    public bool IsSuccessed { get; set; }
    public bool HasResult { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public object Result { get; set; }
    public T GetResult<T>() where T:class
    {
        if (typeof(T).IsArray)
            return JsonConvert.DeserializeObject<T>(Result.ToString());
        else
            return JsonConvert.DeserializeObject<T[]>(Result.ToString())[0];
    }
    public ResultAttribute ResultAttr;
    public APIResponse() { }

    public APIResponse(bool isSuccessed, bool hasResult, string message, int statusCode, object result)
    {
        IsSuccessed = isSuccessed;
        HasResult = hasResult;
        Message = message;
        StatusCode = statusCode;
        Result = result;
    }

    public string GetLog()
    {
        return string.Format("결과 : {0}\n메세지 : {1}\n에러코드 : {2}\n결과값 보유 여부 : {3}\n---결과값---\n{4}",
            IsSuccessed,
            Message,
            StatusCode,
            HasResult,
            Result);
    }
}
