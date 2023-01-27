using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaverSigner : BaseSigner
{
    protected override eProvider provider => eProvider.naver;
    protected override string snsType => "네이버";

    protected override void SignIn()
    {
        
    }

    protected override void SignOut()
    {
        
    }
}
