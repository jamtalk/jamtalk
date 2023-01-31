using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaverSigner : BaseSigner
{
    protected override eProvider provider => eProvider.naver;
    protected override string snsType => "네이버";

    protected override void SignIn()
    {
        NaverTokenReciver.Instance.AccessLogin((success, result) =>
        {
            var uid = result.response.id;
            var email = result.response.email;
            var name = result.response.name;
            if (success)
                ExistSNS(eProvider.kakao, uid, email, name);
        });
    }

    protected override void SignOut()
    {
    }
}
