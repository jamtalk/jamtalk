public class DashboardParam : UserParam
{
    public override eAPIAct act => eAPIAct.dashboard;
    public DashboardParam(string user_id)
    {
        this.user_id = user_id;
    }
}
