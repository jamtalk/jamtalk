using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileSettingPage : MonoBehaviour
{
    public ChildViewPage childViewPage;
    public AccountViewPage accountViewPage;


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
        childViewPage.Init();
        accountViewPage.Init();
    }
}
