using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exists_emailParam : UserParam
{
    protected override eAPIAct act => eAPIAct.exists_email;

    public Exists_emailParam(string user_id)
    {
        this.user_id = user_id;
    }
}
