using UnityEngine;

public class BoardParam : UserParam
{
    protected override eAPIAct act => eAPIAct.board;

    public string bbs;
    public string uid;

    public BoardParam(string bbs, string uid)
    {
        this.bbs = bbs;
        this.uid = uid;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("bbs", bbs);
        form.AddField("uid", uid);

        return form;
    }
}
