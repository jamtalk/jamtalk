using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildViewPage : MonoBehaviour
{
    public Button childEditButton;
    public Button childSettingButton;
    public Text childName;
    public Text childBirth;
    public Text childGender;
    public Text pointText;

    public ChildProfileEdit profileEdit;
    public ChildSetting childSettingOrizin;
    private ChildSetting childSetting;

    private void Awake()
    {
        childEditButton.onClick.AddListener(() => profileEdit.gameObject.SetActive(true));
        childSettingButton.onClick.AddListener(ChildSetting);
    }

    /// <summary>
    /// 아이 데이터 받아와서 출력
    /// </summary>
    public void Init()
    {
        //GetChildren(); *_*
        //GetChild();
    }

    private void ChildSetting()
    {
        if (childSetting == null)
            childSetting = Instantiate(childSettingOrizin, transform.parent);
        else
            childSetting.gameObject.SetActive(true);
    }

    private void GetChild()
    { // 현재 설정된 아이 조회 > GetChildren 에서 설정된 아이 조회 가능 시 GetChildren 에서 바로 설정

        var param = new ChildInfoParam(string.Empty); // 아이 이름
        RequestManager.Instance.RequestAct(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {
                Debug.Log(result.code);
                AndroidPluginManager.Instance.Toast(result.msg);
            }
            else
            {
                // 아이 정보 설정 *_*
                //childName.text =
                //childBirth.text =
                //childGender.text =
            }
        });
    }

    private void GetChildren()
    { // 아이 목록 조회 
        var param = new ChildListParam();
        RequestManager.Instance.RequestAct(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {
                Debug.Log(result.code);
                AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
            }
            else
            {
                // 아이 목록  *_*
            }
        });
    }
}
