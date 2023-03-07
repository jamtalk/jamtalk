public class ActRequestResult
{
    public eErrorCode code;
    public string msg;
}
public class DataRequestResult<T> : ActRequestResult
{
    //public eErrorCode code;
    public T data;
}
