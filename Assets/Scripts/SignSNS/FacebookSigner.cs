using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookSigner : BaseSigner
{
    protected override eProvider provider => eProvider.facebook;

    protected override string snsType => "페이스북";

    protected override void SignIn()
    {
        
    }

    protected override void SignOut()
    {
        
    }
}
