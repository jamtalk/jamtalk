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
            SignOutSNS();

            UserDataManager.Instance.SignOut();

            GJSceneLoader.Instance.LoadScene(eSceneName.AD_002);
        });
    }

    private void SignOutSNS()
    {
        var temp = string.IsNullOrEmpty(UserDataManager.Instance.CurrentUser.provider);
        var provider = temp ? eProvider.none : ((eProvider)Enum.Parse(typeof(eProvider), UserDataManager.Instance.CurrentUser.provider));
        Debug.Log("provider : " + provider.ToString());
        if (provider == eProvider.none)
        {
            KakaoSdk.Logout(() =>
            {
                Debug.Log("kakao logout : + " + UserDataManager.Instance.CurrentUser.user_id);
            }, error => Debug.Log("logoutError : " + error));
        }
        else if (provider == eProvider.naver) { }
        else if (provider == eProvider.facebook) { }
        else { }
    }
}
