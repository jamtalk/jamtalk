using System.Collections;
using System.Collections.Generic;
public interface IParam
{
    Dictionary<string, string> ToParam();
}
public abstract class Param : IParam
{
    public abstract Dictionary<string, string> ToParam();
}
public abstract class ActParam : Param
{
    protected abstract eAPIAct act { get; }

    public override Dictionary<string, string> ToParam()
    {
        var param = new Dictionary<string, string>();
        param.Add("act", act.ToString());
        return param;
    }
}
public abstract class UserParam : ActParam
{
    protected int user_id;

    protected UserParam(int user_id)
    {
        this.user_id = user_id;
    }
    public override Dictionary<string, string> ToParam()
    {
        var param = base.ToParam();
        param.Add("user_id", user_id.ToString());
        return param;
    }
}
