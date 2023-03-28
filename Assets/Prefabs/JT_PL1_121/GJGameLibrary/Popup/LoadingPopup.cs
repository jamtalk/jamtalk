using System;

public class LoadingPopup : BasePopup
{
    public Action ShowLoading()
    {
        return () =>
        {
            if(gameObject != null)
                PopupManager.Instance.Close(gameObject);
        };
    }
}
