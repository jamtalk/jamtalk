using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountViewPage : MonoBehaviour
{
    public Button chashDataButton;
    public Button FindeIDButton;
    public Button ChangePWButton;
    public Button inquiryButton;
    public Button fireButton;
    public Text textNickName;
    public Text textRegData;
    public ToggleElement pustToggle;

    public GameObject firePopup;
    public FindAccount findAccountOrizin;
    private FindAccount findAccount;

    private void Awake()
    {
        FindeIDButton.onClick.AddListener(() => FindAccountAction(FindAccount.ePanelType.FindID));
        ChangePWButton.onClick.AddListener(() => FindAccountAction(FindAccount.ePanelType.ChangePW));
        fireButton.onClick.AddListener(ShowFirePopup);
        pustToggle.pushToggle.onValueChanged.AddListener((value) =>
        {
            UserDataManager.Instance.CurrentUser.isPush = value;
            RequestManager.Instance.Request(new MemberInfoParam(UserDataManager.Instance.CurrentUser, MemberInfoParam.eMemberInfo.on_push, UserDataManager.Instance.CurrentUser.onPush.ToString()),null);
        });
    }
    private void ShowFirePopup()
    {
        PopupManager.Instance.ShowGuidance("Å»Åð ÇÏ½Ã°Ú½À´Ï±î?", () =>
        {
            RequestManager.Instance.Request(new MemberOutParam(), (res) =>
            {
                var result = res.GetResult<ActRequestResult>();

                Debug.Log(result.code);
                if (result.code != eErrorCode.Success)
                {
                    Debug.Log(result.code);
                    AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);
                }
                else
                {
                    AndroidPluginManager.Instance.Toast(res.GetResult<ActRequestResult>().msg);

                    if (PlayerPrefs.HasKey("ID")) PlayerPrefs.DeleteKey("ID");
                    if (PlayerPrefs.HasKey("UID")) PlayerPrefs.DeleteKey("UID");
                    if (PlayerPrefs.HasKey("PROVIDER")) PlayerPrefs.DeleteKey("PROVIDER");
                    PlayerPrefs.Save();

                    GameManager.Instance.SignOut();
                }
            });
        },PopupManager.Instance.Close);
    }

    public void Init()
    {
        textNickName.text = UserDataManager.Instance.CurrentUser.nick;
        textRegData.text = UserDataManager.Instance.CurrentUser.RegistedDate.ToString("yyyy-MM-dd");
    }

    private void FindAccountAction(FindAccount.ePanelType target)
    {
        if (findAccount == null)
            findAccount = Instantiate(findAccountOrizin, transform.parent);
        findAccount.Init(target);
    }
}
