using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupProfile : BasePopup
{
    public Toggle toggleProfile;
    public Toggle toggleEduInfo;
    protected override void Awake()
    {
        buttonExit.onClick.AddListener(PopupManager.Instance.Clear);
    }
    public void ShowProfile()
    {
        toggleProfile.isOn = true;
        toggleProfile.interactable = false;
        toggleEduInfo.isOn = false;
        toggleEduInfo.interactable = true;
    }
    public void ShowEduInfo()
    {
        toggleProfile.isOn = false;
        toggleProfile.interactable = true;
        toggleEduInfo.isOn = true;
        toggleEduInfo.interactable = false;
    }
}
