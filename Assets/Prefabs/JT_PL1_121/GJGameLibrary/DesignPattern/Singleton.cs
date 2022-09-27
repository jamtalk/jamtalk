using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GJGameLibrary.DesignPattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public bool Initialized { get; protected set; } = false;
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    instance.Initialize();
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        public virtual void Initialize()
        {
            instance.Initialized = true;
            Debug.Log("Singleton of " + typeof(T).ToString() + " initialized");
        }
    }

    public class Singleton<T> where T : Singleton<T>, new()
    {
        protected static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();


                return instance;
            }
        }
        public virtual void Initialize() {
            Debug.Log("Singleton of " + typeof(T).ToString() + " initialized");
        }
        public Singleton() => Initialize();
    }
}
