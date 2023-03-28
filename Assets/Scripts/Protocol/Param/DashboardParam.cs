public class DashboardParam : UserParam
{
    public override eAPIAct act => eAPIAct.dashboard;
    public DashboardParam(string user_id) : base(user_id) { }
}
