using UnityEngine;

public class MemberOutParam : UserParam
{
    public override eAPIAct act => eAPIAct.memberout;

    public string user_name;
    public string user_hp;

    public MemberOutParam()
    {
        this.user_id = UserDataManager.Instance.CurrentUser.user_id;
        this.user_name = UserDataManager.Instance.CurrentUser.name;
        this.user_hp = string.Empty;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_name", user_name);
        form.AddField("user_hp", user_hp);

        return form;
    }
}
