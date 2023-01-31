using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System;

public class CharacterParam : UserParam
{
    protected override eAPIAct act => eAPIAct.character;

    public string app_token;
    public int type;
    public DateTime regdate;
    public string title;
    public int level;
    public bool display;
    public string ruid;

    public CharacterParam(string user_id,string app_token, int type, DateTime regdate, string title, int level, bool display, string ruid)
    {
        this.user_id = user_id;
        this.app_token = app_token;
        this.regdate = regdate;
        this.title = title;
        this.level = level;
        this.display = display;
        this.ruid = ruid;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();

        form.AddField("user_id", user_id);
        form.AddField("app_token", app_token);
        form.AddField("type", type);
        form.AddField("regdate", regdate.ToString("yyyy-MM-dd H:mm:ss"));
        form.AddField("title", title);
        form.AddField("level", level);
        form.AddField("display",  Convert.ToInt16(display));
        form.AddField("ruid", ruid);

        return form;
    }
}
