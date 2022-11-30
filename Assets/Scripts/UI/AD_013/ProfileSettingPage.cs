using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileSettingPage : MonoBehaviour
{
    [Header("Left")]
    public Button childSettingButton;
    public Button childSelectButton;
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

    private void Awake()
    {
        childSettingButton.onClick.AddListener(() => profileEdit.gameObject.SetActive(true));

        Init();
    }

    /// <summary>
    /// 아이 데이터 받아와서 출력
    /// </summary>
    private void Init()
    {

    }
}
