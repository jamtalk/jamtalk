using System;

public class LoadingPopup : BasePopup
{
    public Action ShowLoading()
    {
        return () =>
        {
            if(this != null)
                PopupManager.Instance.Close(gameObject);
        };
    }
}
