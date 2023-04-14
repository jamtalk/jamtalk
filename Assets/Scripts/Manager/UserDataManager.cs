using System.Collections;
using System.Collections.Generic;
using GJGameLibrary.DesignPattern;
using System;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
/// <summary>
/// 사용자 데이터
/// </summary>
public class UserDataManager : MonoSingleton<UserDataManager>
{
    public UserInfoData CurrentUser { get; private set; }
    public string currentUserUID => CurrentChild.mem_id;
    public DashBoardData DashBoard { get; private set; }
    public ChildInfoData[] children { get; private set; } = new ChildInfoData[0];
    public ChildInfoData CurrentChild
    {
        get
        {
            if (children.Length == 0)
            {
                Debug.LogFormat("자녀목록 비어있음({0})",GetInstanceID());
                return null;
            }
            if (children.Where(x => x.Selected).Count() == 0)
            {
                var child = children.OrderByDescending(x => x.RegistedDate).First();
                Debug.LogFormat("선택된 자녀 없어서 새로 생성 : {0}", child.name);
                child.Selected = true;
                return child;
            }
            else
            {
                Debug.Log("자녀 목록중 선택된 자녀 띄움");
                return children.ToList().Find(x => x.Selected);
            }
        }
    }
    public bool UserDataLoaded { get; private set; } = false;
    public eProvider UserProvider { get; private set; }
    public void LoadUserData(string id, Action callback)
    {
        Debug.Log("유저 데이터 불러오기");
        StartCoroutine(LoadAll(id, callback));
    }
    public void UpdateChildren(Action callback = null)
    {
        RequestManager.Instance.Request(new ChildListParam(CurrentUser.user_id), response =>
        {
            var successed = response.GetResult<ActRequestResult>().code == eErrorCode.Success;
            Debug.LogFormat("자녀 목록 업데이트 결과 : {0}", successed);
            if (successed)
                children = response.GetResult<DataRequestResult<ChildInfoData[]>>().data.Where(x => x.isDislplay).ToArray();
            Debug.Log(string.Join("\n", children.Select(x => JObject.FromObject(x))));
            callback?.Invoke();
        });
    }
    private IEnumerator LoadAll(string id, Action callback)
    {
        var dic = new Dictionary<ActParam, RequestManager.OnResponse>();
        var list = new List<bool>();
        dic.Add(new UserInfoParam(id), callback =>
        {
            var successed = callback.GetResult<ActRequestResult>().code == eErrorCode.Success;
            if (successed)
            {
                CurrentUser = callback.GetResult<DataRequestResult<UserInfoData>>().data;
                var temp = string.IsNullOrEmpty(Instance.CurrentUser.provider);
                UserProvider = temp ? eProvider.none : ((eProvider)Enum.Parse(typeof(eProvider), Instance.CurrentUser.provider));
                Debug.Log("load all userPrvider : " + UserProvider.ToString());
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
        dic.Add(new ChildListParam(id), res =>
        {
            var successed = res.GetResult<ActRequestResult>().code == eErrorCode.Success;
            Debug.LogFormat("자녀목록 불러오기 결과 : {0}", successed);
            if (successed)
                children = res.GetResult<DataRequestResult<ChildInfoData[]>>().data.Where(x => x.isDislplay).ToArray();
            list.Add(successed);
        });

        foreach(var item in dic)
        {
            RequestManager.Instance.Request(item.Key, item.Value);
        }

        while (list.Count < dic.Count) { yield return null; }
        UserDataLoaded = true;
        callback?.Invoke();
    }
    public void LoadChildList()
    {
        var param = new ChildListParam(CurrentUser.user_id);

        RequestManager.Instance.Request(param, res =>
        {
            var result = res.GetResult<ActRequestResult>().code == eErrorCode.Success;
            if (result)
                children = res.GetResult<DataRequestResult<ChildInfoData[]>>().data.Where(x => x.isDislplay).ToArray();
        });
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
