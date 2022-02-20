using GJGameLibrary;
using UnityEngine;
using DG.Tweening;
using System;

public class LoadingPopup : BasePopup 
{
    public Action ShowLoading()
    {
        return () =>
        {
            PopupManager.Instance.Close();
        };
    }
}
