using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parent_ContentsInfo : MonoBehaviour
{
    private bool isToday = true;
    public Parent_ContentsInfo_Element today;
    public Parent_ContentsInfo_Element previous;

    private void Awake()
    {
        RequestManager.Instance.Request(new EduLogViewParam(), response =>
        {
            var data = response.GetResult<DataRequestResult<EduLogViewResultData>>().data;
            today.Init(GameManager.Instance.GetCommentData(data.contentsType), data.faileCount);
            previous.Init(GameManager.Instance.GetCommentData(data.contentsType), data.faileCount);
        });
    }
    public void OnClickToday()
    {
        isToday = true;
    }
    public void OnClickPrevious()
    {
        isToday = false;
    }
}
