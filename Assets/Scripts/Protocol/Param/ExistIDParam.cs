using UnityEngine;

public class ExistIDParam : ActParam
{
    protected override eAPIAct act => eAPIAct.exists;
    public string user_id;

    public ExistIDParam(string user_id)
    {
        this.user_id = "email:"+user_id;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_id", user_id);
        return form;
    }
}