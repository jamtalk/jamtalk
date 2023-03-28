using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildListParam : UserParam
{
    public override eAPIAct act => eAPIAct.child_list;

    public ChildListParam(string user_id) : base(user_id) { }
}
