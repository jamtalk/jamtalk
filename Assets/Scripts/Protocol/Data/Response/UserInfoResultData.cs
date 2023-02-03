using System;
public class UserInfoResultData
{
    public string mobile;
    public string sex;
    public string birth;
    public string addr;
    public string point;
    public string regdate;
    public DateTime RegistedDate => DateParser.Parse(regdate);
    public string device;
    public string device_ver;
    public string device_token;
    public string provider;
    public string photo_url;
    public string identifier;
    public string displayname;
    public string mb_certify;
}

public class UserInfoData
{
    public string user_id;
    public string name;
    public string nick;
    public string email;
    public string mobile;
    public string sex;
    public string birth;
    public DateTime BirthDay => DateParser.Parse(birth);
    public string addr;
    public string point;
    public string regdate;
    public DateTime RegistedDate => DateParser.Parse(regdate);
    public string device;
    public string device_ver;
    public string device_token;
    public string partner;
    public string provider;
    public string photo_url;
    public string identifier;
    public int onPush = 0;
    public int onEvent = 0;
    public bool isPush => Convert.ToBoolean(onPush);
    public bool isEvent => Convert.ToBoolean(onEvent);
    public string displayname;
    public string mb_certify;
    public int Age => DateTime.Now.Year - BirthDay.Year + 1;
}

public class DateParser
{
    public static DateTime Parse(string value)
    {
        UnityEngine.Debug.Log(value);
        var date = value.Split(' ')[0];
        var time = value.Split(' ')[1];
        UnityEngine.Debug.LogFormat("{0} {1}", date, time);
        return new DateTime(
            int.Parse(date.Split('-')[0]),
            int.Parse(date.Split('-')[1]),
            int.Parse(date.Split('-')[2]),
            int.Parse(time.Split(':')[0]),
            int.Parse(time.Split(':')[1]),
            int.Parse(time.Split(':')[2]));
    }
}
