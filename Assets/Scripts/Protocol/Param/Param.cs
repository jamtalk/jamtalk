using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IParam
{
    WWWForm GetForm();
}
public abstract class Param : IParam
{
    public abstract WWWForm GetForm();
    public override string ToString()
    {
        return string.Format("{0} Param\n{1}", GetType().Name, string.Join("\n", GetType()
            .GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
            .Select(x => string.Format("{1} : {2}", x.FieldType, x.Name, x.GetValue(this).ToString()))
            .Union(
                GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
                .Select(x => string.Format("{1} : {2}", x.PropertyType, x.Name, x.GetValue(this).ToString()))
                )
            ));
    }
}
public abstract class ActParam : Param
{
    public abstract eAPIAct act { get; }
    private int apptoken => 1;
    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("act", act.ToString());
        form.AddField("apptoken", apptoken);
        return form;
    }

}
public abstract class UserParam : ActParam
{
    public string user_id;
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_id", user_id);
        return form;
    }
    public UserParam(string user_id)
    {
        this.user_id = user_id;
    }
    public UserParam()
    {
        user_id = UserDataManager.Instance.CurrentUser.user_id;
    }
}
