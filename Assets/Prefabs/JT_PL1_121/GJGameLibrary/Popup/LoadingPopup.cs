using GJGameLibrary;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class LoadingPopup : BasePopup 
{
    public Image progress;
    public AnimationCharactor[] charactors;

    public void progressbarCharging(float percent)
    {
        if (!progress.gameObject.activeSelf)
            progress.gameObject.SetActive(true);

        progress.transform.localScale = new Vector3(percent, 1f, 1f);
    }

    public Action ShowLoading()
    {
        return () =>
        {
            PopupManager.Instance.Close();
        };
    }
}
