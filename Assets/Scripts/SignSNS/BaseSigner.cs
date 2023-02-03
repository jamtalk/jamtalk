using System;
using System.Collections;
using System.Collections.Generic;
using GJGameLibrary;
using System.Runtime.CompilerServices;
using Kakaotalk;
using UnityEngine;
using GJGameLibrary.DesignPattern;

public abstract class BaseSigner
{
    protected abstract eProvider provider { get; }
    protected string uid { get; private set; }
    protected abstract string snsType { get; }
    protected string id;
    protected string pw;

    protected abstract void SignIn();
    protected abstract void SignOut();
    Action<string, string, eProvider, string> callback;

    public void SignInSNS(Action<string, string, eProvider, string> callback)
    {
        this.callback = callback;
        SignIn();
    }

    public void SignOutSNS()
    {
        SignOut();
    }

    public void ExistSNS(eProvider eProvider, string uid, string email, string name)
    {
        var providerID = eProvider.ToString().Substring(0, 2) + uid;
        var param = new ExistIDParam(providerID);

        RequestManager.Instance.RequestAct(param, (res) =>
        {
            var result = res.GetResult<ActRequestResult>();
            if (result.code == eErrorCode.Success)
                SignUpSNS(eProvider, uid, name, email);
            else
                callback?.Invoke(providerID, providerID, eProvider, uid);
        });
    }

    public void SignUpSNS(eProvider eProvider, string uid, string name, string email)
    {
        var providerID = eProvider.ToString().Substring(0, 2) + uid;
        var param = new SignUpParam(providerID, providerID, name, email, string.Empty, eProvider, string.Empty, uid, string.Empty, string.Empty);
        RequestManager.Instance.RequestAct(param, res =>
        {
            var result = res.GetResult<ActRequestResult>();

            if (result.code != eErrorCode.Success)
            {
                AndroidPluginManager.Instance.Toast(result.msg);
                Debug.Log("SignUp SNS Failed : " + result.msg);
            }
            else
            {
                Debug.Log(string.Format("{0} 회원가입이 완료되었습니다", snsType));
                AndroidPluginManager.Instance.Toast(string.Format("{0} 회원가입이 완료되었습니다", snsType));

                callback?.Invoke(providerID, providerID, eProvider, uid);
            }
        });
    }
}