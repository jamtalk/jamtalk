using GJGameLibrary;
using Kakaotalk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupParents : BasePopup
{
    public Button buttonSingOut;
    protected override void Awake()
    {
        buttonExit.onClick.AddListener(PopupManager.Instance.Clear);
        buttonSingOut.onClick.AddListener(() =>
        {
            //SignSNS.Instance.SignOutSNS();
            UserDataManager.Instance.SignOut();

            GJSceneLoader.Instance.LoadScene(eSceneName.AD_002);
        });
    }
}
