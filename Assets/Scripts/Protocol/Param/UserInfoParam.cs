public class UserInfoParam : UserParam
{
    public override eAPIAct act => eAPIAct.member;
    public UserInfoParam(string user_id)
    {
        this.user_id = user_id;
    }
}