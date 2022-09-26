public class EduLogViewParam : UserParam
{
    protected override eAPIAct act => eAPIAct.edulog_view;
    public EduLogViewParam(string user_id)
    {
        this.user_id = user_id;
    }
}
