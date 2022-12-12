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
