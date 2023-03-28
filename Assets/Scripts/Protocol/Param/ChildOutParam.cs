using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOutParam : UserParam
{
    public override eAPIAct act => eAPIAct.childout;
    public string key => data.child_key;
    public string name=>data.name;
    public string jumin=>data.jumin;
    public int display => data.display;
    public int level=>data.level;
    public int character_pick => data.character_pick;
    public string gender=>data.gender;
    public int point=>data.point;

    public ChildInfoData data { get; private set; }

    public ChildOutParam(ChildInfoData data):base()
    {
        this.data = data;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("key", key);
        form.AddField("name", name);
        form.AddField("jumin", jumin);
        form.AddField("display", display);
        form.AddField("level", level);
        form.AddField("character_pick", character_pick);
        form.AddField("gender", gender);
        form.AddField("point", point);
        return form;
    }
}
