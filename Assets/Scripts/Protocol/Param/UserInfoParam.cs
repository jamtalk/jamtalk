public class UserInfoParam : UserParam
{
    public override eAPIAct act => eAPIAct.member;
    public UserInfoParam(string user_id) : base(user_id) { }
}