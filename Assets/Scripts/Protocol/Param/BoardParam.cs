using UnityEngine;

public class BoardParam : UserParam
{
    protected override eAPIAct act => eAPIAct.board;

    public eBoardType boardType;
    public int uid;

    public BoardParam(eBoardType boardType, int uid)
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
