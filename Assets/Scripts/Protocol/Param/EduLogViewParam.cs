public class EduLogViewParam : UserParam
{
    protected override eAPIAct act => eAPIAct.edulog_view;
    public EduLogViewParam( )
    {
        if(string.IsNullOrEmpty(user_id))
            user_id = UserDataManager.Instance.CurrentUser.user_id;
    }
}
