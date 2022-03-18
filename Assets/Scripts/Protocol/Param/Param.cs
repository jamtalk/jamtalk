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
    protected abstract eAPIAct act { get; }
    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("act", act.ToString());
        return form;
    }

}
public abstract class UserParam : ActParam
{
    protected int user_id;

    protected UserParam(int user_id)
    {
        this.user_id = user_id;
    }
    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("user_id", user_id.ToString());
        return form;
    }
}