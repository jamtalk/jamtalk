using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InquiryList : MonoBehaviour
{
    public InquiryElement inquiryOrizin;
    public InquiryDetail inquiryDetail;
    public RectTransform viewPointRt;

    private List<InquiryElement> inquiries = new List<InquiryElement>();


    private void Awake()
    {
        //GetInquiry();
    }


    /// <summary>
    /// user id 가 작성한대한 문의내역 목록 조회
    /// </summary>
    private void GetInquiry()
    {
        var param = new BoardParam(eBoardType.notice, 1);
        //var param = new BoardListParam();
        RequestManager.Instance.Request(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {
                Debug.Log(result.code);
                AndroidPluginManager.Instance.Toast(result.msg);
            }
            else
            {
                CreateInquiry();
            }
        });
    }


    /// <summary>
    /// 문의내역 호출하여 생성
    /// </summary>
    private void CreateInquiry()
    {
        //for(int i = 0; i < List; i++) 
        //{ 
        var element = Instantiate(inquiryOrizin, viewPointRt);
        element.Init(true);
        element.transform.parent = viewPointRt.transform;
        inquiries.Add(element);
        //}

        //foreach (var item in inquiries)
        //    item.clickAction += () => inquiryDetail.Init(0);
    }
}
