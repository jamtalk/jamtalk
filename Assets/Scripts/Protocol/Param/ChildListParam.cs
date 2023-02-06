using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildListParam : UserParam
{
    protected override eAPIAct act => eAPIAct.child_list;

    public ChildListParam(string user_id)
    {
        this.user_id = user_id;
        //user_id = UserDataManager.Instance.CurrentUser.user_id;
    }
}
