using System.Collections;
using System.Collections.Generic;
using GJGameLibrary.DesignPattern;
using System;
using UnityEngine;
/// <summary>
/// 사용자 데이터
/// </summary>
public class UserDataManager : MonoSingleton<UserDataManager>
{
    public UserInfoData CurrentUser { get; private set; }
    public DashBoardData DashBoard { get; private set; }
    public bool UserDataLoaded { get; private set; } = false;
    public eProvider UserProvider { get; private set; }

    public void LoadUserData(string id, Action callback)
    {
        StartCoroutine(LoadAll(id, callback));
    }
    private IEnumerator LoadAll(string id, Action callback)
    {
        var dic = new Dictionary<IParam, RequestManager.OnResponse>();
        var list = new List<bool>();
        dic.Add(new UserInfoParam(id), callback =>
        {
            var successed = callback.GetResult<ActRequestResult>().code == eErrorCode.Success;
            if (successed)
            {
                CurrentUser = callback.GetResult<DataRequestResult<UserInfoData>>().data;
            }

            list.Add(successed);
        });
        dic.Add(new DashboardParam(id), callback =>
        {
            var successed = callback.GetResult<ActRequestResult>().code == eErrorCode.Success;
            if (successed)
            {
                DashBoard = callback.GetResult<DataRequestResult<DashBoardData>>().data;
            }

            list.Add(successed);
        });

        foreach(var item in dic)
        {
            RequestManager.Instance.RequestAct(item.Key, item.Value);
        }

        while (list.Count < dic.Count) { yield return null; }
        UserDataLoaded = true;
        callback?.Invoke();
    }
    public void SignOut()
    {
        if (PlayerPrefs.HasKey("PW"))
            PlayerPrefs.DeleteKey("PW");
        PlayerPrefs.Save();
    }
}
//[Serializable]
//public class DigraphsWordsData
//{
//    public eDigraphs type;
//    public bool IsPair() => IsPair(type);
//    public ePairDigraphs GetPair() => GetPair(type);
//    public static bool IsPair(eDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(ePairDigraphs))
//            .Select(x => (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        return pairs.Contains(num);
//    }
//    public static bool IsPair(ePairDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(eDigraphs))
//            .Select(x => (eDigraphs)Enum.Parse(typeof(eDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        return pairs.Contains(num);
//    }
//    public static ePairDigraphs GetPair(eDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(ePairDigraphs))
//            .Select(x => (ePairDigraphs)Enum.Parse(typeof(ePairDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        if (pairs.Contains(num))
//            return (ePairDigraphs)num;
//        else
//            return 0;
//    }
//    public static eDigraphs GetPair(ePairDigraphs digraphs)
//    {
//        var num = (int)digraphs;
//        var pairs = Enum.GetNames(typeof(eDigraphs))
//            .Select(x => (eDigraphs)Enum.Parse(typeof(eDigraphs), x))
//            .Select(x => (int)x)
//            .ToArray();
//        if (pairs.Contains(num))
//            return (eDigraphs)num;
//        else
//            return 0;
//    }
//    private DigraphsAudioData digrpahsAudio => GameManager.Instance.Schema.GetDigrpahsAudio(type);
//    private DigraphsAudioData pairDigraphsAudio => GameManager.Instance.Schema.GetDigrpahsAudio(GetPair());
//    public string act;
//    public string clip;

//    public DigraphsWordsData(eDigraphs type, string value, string act, string clip, int targetLevel) : base(value)
//    {
//        this.type = type;
//        this.act = act;
//        this.clip = clip;
//        TargetLevel = targetLevel;
//    }

//    public int TargetLevel { get; private set; }

//    public override bool Equals(object obj)
//    {
//        return obj is DigraphsWordsData source &&
//               type == source.type &&
//               value == source.value;
//    }

//    public override int GetHashCode()
//    {
//        var hashCode = 1148455455;
//        hashCode = hashCode * -1521134295 + type.GetHashCode();
//        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(value);
//        return hashCode;
//    } 
//}
