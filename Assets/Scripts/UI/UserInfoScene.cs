using UnityEngine;

public abstract class UserInfoScene : MonoBehaviour
{
    protected virtual void Awake()
    {
#if UNITY_EDITOR
        //var id = UserDataManager.Instance.CurrentUser.user_id.Replace("email:", string.Empty);
        var id = UserDataManager.Instance.CurrentUser.user_id;
        UserDataManager.Instance.LoadUserData(id, Init);
#else
        Init();
#endif
    }

    public virtual void Init() { }
}
