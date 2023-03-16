using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberInfoParam : UserParam
{
    public override eAPIAct act => eAPIAct.memberinfo;

    public string user_pw;
    public string user_name;
    public string user_hp;
    public string user_pic;
    public int on_push;
    public int on_event;
    public string device;
    public string device_ver;
    public string device_token;

    public MemberInfoParam(string user_id, string user_pw, string user_name, string user_hp, string user_pic, int on_push, int on_event, string device, string device_ver, string device_token)
    {
        this.user_id = user_id;
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
    public enum eMemberInfo
    {
        user_id,
        user_pw,
        user_name,
        user_hp,
        user_pic,
        on_push,
        on_event,
        device,
        device_ver,
        device_token
    }
    public MemberInfoParam(UserInfoData data, eMemberInfo eInfo, string value)
    {
        user_id = data.user_id;
        user_pw = string.Empty;
        user_name = data.name;
        user_hp = string.Empty;
        user_pic = string.Empty;
        on_push = data.onPush;
        on_event = data.onEvent;
        device = data.device;
        device_ver = data.device_ver;
        device_token = data.device_token;

        if (eInfo == eMemberInfo.user_id) user_id = value;
        else if (eInfo == eMemberInfo.user_pw) user_pw = value;
        else if (eInfo == eMemberInfo.user_hp) user_hp = value;
        else if (eInfo == eMemberInfo.user_pic) user_pic = value;
        else if (eInfo == eMemberInfo.on_push) on_push = int.Parse(value);
        else if (eInfo == eMemberInfo.on_event) on_event= int.Parse(value);
        else if (eInfo == eMemberInfo.device) user_pw = value;
        else if (eInfo == eMemberInfo.device_ver) device_ver = value;
        else if (eInfo == eMemberInfo.device_token) device_token = value;
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
