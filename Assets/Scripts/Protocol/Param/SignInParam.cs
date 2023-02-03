using UnityEngine;

public class SignInParam : ActParam
{
    protected override eAPIAct act => eAPIAct.login;
    public string user_id;
    public string user_pw;
    public eProvider provider;
    public string identifier;

    public SignInParam(string user_id, string user_pw, eProvider provider, string identifier)
    {
        this.user_id = user_id;
        this.user_pw = user_pw;
        this.provider = provider;
        this.identifier = identifier;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        var providerValue = provider == eProvider.none ? string.Empty : provider.ToString();
        //if (provider == eProvider.none) user_id = "email:" + user_id;
        if(provider == eProvider.none)
        {
            var isEmail = user_id.Contains("email:");
            if (!isEmail) user_id = "email:" + user_id;
        }

        form.AddField("user_id", user_id);
        form.AddField("user_pw", user_pw);
        form.AddField("provider", providerValue);
        form.AddField("identifier", identifier);
        return form;
    }
}
