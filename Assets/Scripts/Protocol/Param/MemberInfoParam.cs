using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberInfoParam : UserParam
{
    protected override eAPIAct act => eAPIAct.memberinfo;

    public string user_pw;
    public string user_name;
    public string user_hp;
    public string user_pic;
    public string on_push;
    public string on_event;
    public string device;
    public string device_ver;
    public string device_token;

    public MemberInfoParam(string user_pw, string user_name, string user_hp, string user_pic, string on_push, string on_event, string device, string device_ver, string device_token)
    {
        this.user_pw = user_pw;
        this.user_name = user_name;
        this.user_hp = user_hp;
        this.user_pic = user_pic;
        this.on_push = on_push;
        this.on_event = on_event;
        this.device = device;
        this.device_ver = device_ver;
        this.device_token = device_token;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("user_pw", user_pw);
        form.AddField("user_name", user_name);
        form.AddField("user_hp", user_hp);
        form.AddField("user_pic", user_pic);
        form.AddField("on_push", on_push);
        form.AddField("on_event", on_event);
        form.AddField("device", device);
        form.AddField("device_ver", device_ver);
        form.AddField("device_token", device_token);

        return form;
    }
}
