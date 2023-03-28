using UnityEngine;

public class BoardParam : UserParam
{
    public override eAPIAct act => eAPIAct.board;

    public eBoardType boardType;
    public int uid;

    public BoardParam(eBoardType boardType, int uid) : base()
    {
        this.boardType = boardType;
        this.uid = uid;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();
        form.AddField("bbs", boardType.ToString());
        form.AddField("uid", uid);

        return form;
    }
}
