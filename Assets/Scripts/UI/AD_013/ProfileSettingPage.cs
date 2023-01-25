using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileSettingPage : MonoBehaviour
{
    [Header("Left")]
    public Button childEditButton;
    public Button childSettingButton;
    public Text childName;
    public Text childBirth;
    public Text childGender;

    [Header("Right")]
    public Button chashDataButton;
    public Button FindeIDButton;
    public Button ChangePWButton;
    public Button inquiryButton;
    public Button fireButton;
    public Text nickName;
    public Text joinDate;
    public Text pointText;
    public ToggleElement pustToggle;

    [Header("others")]
    public ChildProfileEdit profileEdit;
    public ChildSetting childSettingOrizin;
    public FindAccount findAccountOrizin;

    private ChildSetting childSetting;
    private FindAccount findAccount;

    private void Awake()
    {
        childEditButton.onClick.AddListener(() => profileEdit.gameObject.SetActive(true));
        childSettingButton.onClick.AddListener(() => ChildSetting());
        FindeIDButton.onClick.AddListener(() => FindAccountAction(FindAccount.eTarget.FindID));
        ChangePWButton.onClick.AddListener(() => FindAccountAction(FindAccount.eTarget.ChangePW));

        Init();
    }

    /// <summary>
    /// 아이 데이터 받아와서 출력
    /// </summary>
    private void Init()
    {
        //GetChildren(); *_*
        //GetChild();
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
                AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
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

    private void FindAccountAction(FindAccount.eTarget target)
    {
        if (findAccount == null)
            findAccount = Instantiate(findAccountOrizin, transform.parent);
        findAccount.Init(target);
    }

    private void ChildSetting()
    {
        if (childSetting == null)
            childSetting = Instantiate(childSettingOrizin, transform);
        else
            childSetting.gameObject.SetActive(true);
    }
}
