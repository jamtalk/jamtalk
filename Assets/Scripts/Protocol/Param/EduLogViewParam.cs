public class EduLogViewParam : UserParam
{
    public override eAPIAct act => eAPIAct.edulog_view;
    public EduLogViewParam() : base() { }
}

public class EduLogViewResultData
{
    public string mem_id;
    public string app_token;
    public string child_key;
    public int type;
    public System.DateTime created_at;
    public string contents_title;
    public eContents contentsType => (eContents)System.Enum.Parse(typeof(eContents), contents_title);
    public string alphabet;
    public eAlphabet CurrentAlphabet => (eAlphabet)System.Enum.Parse(typeof(eAlphabet), alphabet);
    public int bookid;
    public string level;
    public System.DateTime end_time;
    public float timespan;
    public int total_score;
    public int correct_score;
    public float due;
    public float day;
    public int award;
    public int claimed_point;
    public int point;
    public int faileCount => total_score - correct_score;
}