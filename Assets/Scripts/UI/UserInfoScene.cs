using UnityEngine;

public abstract class UserInfoScene : MonoBehaviour
{
    protected virtual void Awake()
    {
#if UNITY_EDITOR
        UserDataManager.Instance.LoadUserData("tabletasd123-asdasdfkjb1-asdas1", Init);
#else
        Init();
#endif
    }

    public virtual void Init() { }
}
