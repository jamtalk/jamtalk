using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class BoardWriteParam : UserParam
{
    protected override eAPIAct act => eAPIAct.board_write;

    public eBoardType boardType;
    public string user_name;
    public string wr_subject;
    public string wr_content;

    public BoardWriteParam(eBoardType boardType, string wr_subject, string wr_content)
    {
        this.boardType = boardType;
        this.user_name = UserDataManager.Instance.CurrentUser.name;
        this.user_id = UserDataManager.Instance.CurrentUser.user_id;
        this.wr_subject = wr_subject;
        this.wr_content = wr_content;
    }
    public override WWWForm GetForm()
    {
        var form = base.GetForm();

        form.AddField("bbs", boardType.ToString());
        form.AddField("user_name", user_name);
        form.AddField("wr_subject", wr_subject);
        form.AddField("wr_content", wr_content);

        return form;
    }
}
