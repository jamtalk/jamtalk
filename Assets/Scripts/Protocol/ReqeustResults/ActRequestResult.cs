public class ActRequestResult
{
    public eErrorCode code;
    public string msg;
}
public class DataRequestResult<T>
{
    public eErrorCode code;
    public T data;
}
