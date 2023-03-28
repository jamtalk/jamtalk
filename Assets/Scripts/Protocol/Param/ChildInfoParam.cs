using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInfoParam : UserParam
{
    public override eAPIAct act => eAPIAct.child_info;

    public string user_name;

    public ChildInfoParam(string user_name):base()
    {   // UserDataManager 에 활성화 된 아이 정보 저장 > username 바로 호출 , 매게변수 삭제 예정 
        this.user_name = user_name;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_name", user_name);

        return form;
    }
}
