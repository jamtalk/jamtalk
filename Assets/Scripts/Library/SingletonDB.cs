using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonDB<T> : ScriptableObject where T:SingletonDB<T>
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>("DB/"+typeof(T).Name);
                instance.Initialize();
            }

            return instance;
        }
    }
    protected virtual void Initialize()
    {
        Debug.Log(typeof(T).Name + " Initialized");
    }
}
