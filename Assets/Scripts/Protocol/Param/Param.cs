using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParam
{
    WWWForm GetForm();
}
public abstract class Param : IParam
{
    public abstract WWWForm GetForm();
}
public abstract class ActParam : Param
{
    public abstract eAPIAct act { get; }
    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("act", act.ToString());
        return form;
    }

}
public abstract class UserParam : ActParam
{
    protected string user_id;
    public override WWWForm GetForm()
    {
        var form = base.GetForm();

        if (string.IsNullOrEmpty(user_id))
            user_id = UserDataManager.Instance.CurrentUser.user_id;
        form.AddField("user_id", user_id);
        return form;
    }
}
