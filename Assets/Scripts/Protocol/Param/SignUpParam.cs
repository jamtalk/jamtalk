using UnityEngine;

public class SignUpParam : ActParam
{
    protected override eAPIAct act => eAPIAct.register;
    public string user_id;
    public string user_pw;
    public string user_name;
    public string user_email;
    public string user_hp;
    public eProvider provider;
    public string sha;
    public string identifier;
    public string photourl;
    public string displayname;

    public SignUpParam(string user_id, string user_pw, string user_name, string user_email, string user_hp, eProvider provider, string sha, string identifier, string photourl, string displayname)
    {
        this.user_id = user_id;
        this.user_pw = user_pw;
        this.user_name = user_name;
        this.user_email = user_email;
        this.user_hp = user_hp;
        this.provider = provider;
        this.sha = sha;
        this.identifier = identifier;
        this.photourl = photourl;
        this.displayname = displayname;
    }

    public override WWWForm GetForm()
    {
        var form =  base.GetForm();
        var providerValue = provider == eProvider.none ? string.Empty : provider.ToString();

        form.AddField("user_id", user_id);
        form.AddField("user_pw", user_pw);
        form.AddField("user_name", user_name);
        form.AddField("user_email", user_email);
        form.AddField("user_hp", user_hp);
        form.AddField("provider", providerValue);
        form.AddField("sha", sha);
        form.AddField("identifier", identifier);
        form.AddField("photourl", photourl);
        form.AddField("displayname", displayname);
        return form;
    }
}
