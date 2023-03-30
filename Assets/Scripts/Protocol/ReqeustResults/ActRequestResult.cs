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

public class BoardReqeustResult
{
    public class BoardRequestMessage
    {
        public string current_field;
        public string field_count;
        public string lengths;
        public string num_rows;
        public string type;
    }

    public eErrorCode code;
    public BoardRequestMessage msg;
    public BoardData[] data;
}

public class BoardData
{
    public string wr_subject;
    public string wr_content;
    public string wr_datetime;
    public string wr_coment_title;
    public string wr_coment_detail;
    public string wr_coment_datetime;
    public bool isAnswered => !string.IsNullOrEmpty(wr_coment_title);
}