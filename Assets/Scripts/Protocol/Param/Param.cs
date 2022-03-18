using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParam
{
    WWWForm GetForm();
}
public abstract class Param : IParam
{
    public abstract WWWForm GetForm();
}
public abstract class ActParam : Param
{
    protected abstract eAPIAct act { get; }
    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("act", act.ToString());
        return form;
    }

}
public abstract class UserParam : ActParam
{
    protected string user_id;

    protected UserParam()
    {
        user_id = "tabletasd123-asdasdfkjb1-asdas1";
    }
    public override WWWForm GetForm()
    {
        var form = new WWWForm();
        form.AddField("user_id", user_id);
        return form;
    }
}
public class EduLogParam : UserParam
{
    protected override eAPIAct act => eAPIAct.edulog;
    public string app_token;
    public DateTime regdate;
    public eContents contents_title;
    public eAlphabet alphabet;
    public eAlphbetType alphabetType; 
    public int level;
    public DateTime end_time;
    public TimeSpan timespan;
    public int total_score;
    public int correct_score;
    public float due;

    public EduLogParam(string app_token, DateTime regdate, eContents contents_title, eAlphabet alphabet, eAlphbetType alphabetType, int level, DateTime end_time, TimeSpan timespan, int total_score, int correct_score, float due)
        : base()
    {
        this.app_token = app_token;
        this.regdate = regdate;
        this.contents_title = contents_title;
        this.alphabet = alphabet;
        this.alphabetType = alphabetType;
        this.level = level;
        this.end_time = end_time;
        this.timespan = timespan;
        this.total_score = total_score;
        this.correct_score = correct_score;
        this.due = due;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("app_token", app_token);
        form.AddField("regdate", regdate.ToString("yyyy-MM-dd H:mm:ss"));
        form.AddField("contents_title", contents_title.ToString());
        if (alphabetType == eAlphbetType.Upper)
            form.AddField("alphabet", alphabet.ToString().ToUpper());
        else
            form.AddField("alphabet", alphabet.ToString().ToLower());
        form.AddField("level", level);
        form.AddField("end_time", end_time.ToString("yyyy-MM-dd H:mm:ss"));
        form.AddField("timespan", GetTimeSpan());
        form.AddField("total_score", total_score.ToString());
        form.AddField("correct_score", correct_score.ToString());
        form.AddField("due", due.ToString("N2"));
        return form;
    }
    private string GetTimeSpan()
    {
        int h = (int)timespan.TotalHours % 24;
        int m = (int)timespan.TotalMinutes % 60;
        var s = timespan.TotalSeconds % 60;
        return string.Format("00:00:00 {0}:{1}:{2}",
            h < 10 ? "0" : "" + h.ToString(),
            m < 10 ? "0" : "" + m.ToString(),
            s < 10 ? "0" : "" + s.ToString());
    }
}