using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using RotaryHeart.Lib.SerializableDictionary;

public abstract class LocalDBElement : ScriptableObject
{
    public virtual bool Loadable => true;
    public abstract void Load(List<Hashtable> data);
}

public abstract class LocalDBElement<T> : LocalDBElement
    where T : ResourceWordsElement
{
    [SerializeField]
    protected T[] data;
    public T[] Get() => data;
}
