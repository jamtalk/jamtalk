using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "LocalDB.asset", menuName = "LocalDB/DB")]
public class LocalDB : ScriptableObject
{
    [SerializeField]
    private AudioClip correctClip;
    public static string Path => "LocalDB";
    private static LocalDB _instance=null;
    public static LocalDB Instance
    {
        get
        {
            if (_instance == null)
            { 
                _instance  = Resources.Load<LocalDB>(Path);
            }

            return _instance;

        }
    }
    [SerializeField]
    private LocalDBElement[] elements;

    public T Get<T>() where T:LocalDBElement
    {
        return elements
            .Where(x => x.GetType() == typeof(T))
            .Select(x=>(T)x)
            .FirstOrDefault();
    }
    public AudioClip GetCorrectClip() => correctClip;

    public static T Find<T>(IEnumerable<T> datas, string name) where T:UnityEngine.Object
    {
        var targets = datas.Where(x => x.name == name);
        if (targets.Count() == 1)
            return targets.First();
        else if (targets.Count() == 0)
        {
            Debug.LogWarningFormat("Has not found {0} {1}", typeof(T).Name, name);
            return null;
        }
        else
        {
            Debug.LogWarningFormat("Too many found {0} {1} ({2}ea)", typeof(T).Name, name,targets.Count());
            return targets.First();

        }
    }
    public static IEnumerator Initialize(Action onDone)
    {
        yield return null;
        _instance = null;
        Debug.Log("로딩 시작@");
        var list = new List<LocalDBElement>();
        yield return AddElement<AlphabetSpriteData>(list);
        yield return AddElement<AlphabetAudioData>(list);
        Debug.Log(list.Count);
        //var op = Resources.LoadAsync<LocalDB>(Path);
        //Debug.Log(op.isDone);
        //while (!op.isDone)
        //{
        //    yield return op;
        //    var progress = op.progress * 100f;
        //    Debug.LogFormat("{0}% 진행 완료",progress.ToString("N2"));
        //}
        //_instance = op.asset as LocalDB;
        //Debug.LogFormat("결과 : {0}", _instance);
        //onDone?.Invoke();
    }
    private static IEnumerator AddElement<T>(List<LocalDBElement> elements) where T:LocalDBElement
    {
        var op = Addressables.LoadAssetAsync<T>(typeof(T).Name);
        yield return op;
        Debug.LogFormat("{0} 로딩결과 : {1}", typeof(T).Name, op.Result);
        elements.Add(op.Result);
    }
    //public static IEnumerator Initialize(Action<float> onProgress=null,Action onDone=null)
    //{

    //    var op = Addressables.LoadAssetAsync<LocalDB>(Path);
    //    Debug.Log(op.IsDone);
    //    while (!op.IsDone)
    //    {
    //        var progress = op.P * 100f;
    //        Debug.LogFormat("{0}% 진행중", progress.ToString("N2"));
    //        onProgress?.Invoke(progress);
    //        yield return null;
    //    }
    //    _instance = op.asset as LocalDB;
    //    Debug.LogFormat("완료여부 : {0} ({1}%)\n" +
    //        "원본 : {2}\n" +
    //        "형변환 : {3}",
    //        op.isDone,
    //        op.progress*100f,
    //        op.asset,
    //        op.asset as LocalDB);
    //    Debug.Log(_instance);
    //    onDone?.Invoke();
    //}
}
public abstract class LocalDBElement : ScriptableObject
{
    public virtual bool Loadable => true;
    public abstract void Load(List<Hashtable> data);
}

public abstract class LocalDBElement<T> : LocalDBElement
    where T : DataSource
{
    [SerializeField]
    protected T[] data;
    public T[] Get() => data;
}

public abstract class DataSource
{
    public string value;
    public Sprite sprite;

    protected DataSource(string value, Sprite sprite)
    {
        this.value = value;
        this.sprite = sprite;
    }

    public virtual bool IsNull => string.IsNullOrEmpty(value) || sprite == null;
}
