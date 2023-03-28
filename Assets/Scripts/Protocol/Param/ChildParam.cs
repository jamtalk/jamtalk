using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChildParam : UserParam
{
    public override eAPIAct act => eAPIAct.child;

    public string name;
    public string jumin;
    public bool display;
    public int level;
    public int character=0;
    public char gender;

    public ChildParam(string name, string jumin, bool display, int level, int character, char gender):base()
    {
        this.name = name;
        this.jumin = jumin;
        this.display = display;
        this.level = level;
        this.character = character;
        this.gender = gender;
    }

    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("name", name);
        form.AddField("jumin", jumin);
        form.AddField("display", Convert.ToInt16(display));
        form.AddField("jumin", jumin);
        form.AddField("level", level);
        form.AddField("character", character);
        form.AddField("gender", gender);
        return form;
    }
}
