using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award_logParam : UserParam
{
    protected override eAPIAct act => eAPIAct.award_log;

    public Award_logParam(string user_id)
    {
        this.user_id = user_id;
    }
}
