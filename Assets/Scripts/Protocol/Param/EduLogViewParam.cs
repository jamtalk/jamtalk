public class EduLogViewParam : UserParam
{
    public override eAPIAct act => eAPIAct.edulog_view;
    public EduLogViewParam( )
    {
        if(string.IsNullOrEmpty(user_id))
            user_id = UserDataManager.Instance.CurrentUser.user_id;
    }
}
