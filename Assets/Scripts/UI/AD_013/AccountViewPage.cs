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
        fireButton.onClick.AddListener(() => firePopup.gameObject.SetActive(true));
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
