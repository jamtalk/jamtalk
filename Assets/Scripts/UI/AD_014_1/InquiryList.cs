using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InquiryList : MonoBehaviour
{
    public InquiryElement inquiryOrizin;
    public InquiryDetail inquiryDetail;
    public RectTransform elementParent;

    private List<InquiryElement> inquiries = new List<InquiryElement>();


    private void OnEnable()
    {
        GetInquiry();
    }


    /// <summary>
    /// user id 가 작성한대한 문의내역 목록 조회
    /// </summary>
    private void GetInquiry()
    {
        var param = new BoardParam(eBoardType.qa, UserDataManager.Instance.currentUserUID);
        RequestManager.Instance.Request(param, (res) =>
        {
            var result = res.GetResult<BoardReqeustResult>();

            if (result.code == eErrorCode.Success)
            {
                CreateInquiry(result.data);
            }
        });
    }


    /// <summary>
    /// 문의내역 호출하여 생성
    /// </summary>
    private void CreateInquiry(BoardData[] data)
    {
        for (int i = 0; i < inquiries.Count; i++)
            Destroy(inquiries[i].gameObject);
        inquiries.Clear();

        for (int i = 0; i < data.Length; i++)
        {
            var element = Instantiate(inquiryOrizin, elementParent);
            element.Init(data[i]);
            element.onClick.AddListener(inquiryDetail.Init);
            inquiries.Add(element);
        }
    }
}
