using UnityEngine;

public class MemberOutParam : UserParam
{
    public override eAPIAct act => eAPIAct.memberout;

    public string user_name;
    public string user_hp;

    public MemberOutParam():base()
    {
        user_name = UserDataManager.Instance.CurrentUser.name;
        user_hp = string.Empty;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_name", user_name);
        form.AddField("user_hp", user_hp);

        return form;
    }
}
