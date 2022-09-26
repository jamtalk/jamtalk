using UnityEngine;

public class SignInParam : ActParam
{
    protected override eAPIAct act => eAPIAct.login;
    public string user_id;
    public string user_pw;
    public string provider;
    public string identifier;

    public SignInParam(string user_id, string user_pw, string provider, string identifier)
    {
        this.user_id = user_id;
        this.user_pw = user_pw;
        this.provider = provider;
        this.identifier = identifier;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_id", user_id);
        form.AddField("user_pw", user_pw);
        form.AddField("provider", provider);
        form.AddField("identifier", identifier);
        return form;
    }
}
