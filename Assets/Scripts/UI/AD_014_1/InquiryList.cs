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
        CreatList();
    }
    /// <summary>
    /// 문의내역 호출하여 생성
    /// </summary>
    private void CreatList()
    {
        //for(int i = 0; i < List; i++) 
        //{ 
        var element = Instantiate(inquiryOrizin, viewPointRt);
        element.Init(true);
        element.transform.parent = viewPointRt.transform;
        inquiries.Add(element);
        //}

        foreach (var item in inquiries)
            item.clickAction += () => inquiryDetail.Init();
    }
}
