﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalDB.asset", menuName = "LocalDB/DB")]
public class LocalDB : ScriptableObject
{
    [SerializeField]
    private AudioClip correctClip;
    public static string Path => "LocalDB";
    public static LocalDB Instance => Resources.Load<LocalDB>(Path);
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
