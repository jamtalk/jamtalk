using GJGameLibrary;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SceneLoadingPopup : LoadingPopup
{
    public Image progress;
    public AnimationCharactor[] charactors;
    public static List<IEnumerator> SpriteLoader = new List<IEnumerator>();
    public static System.Action onLoaded;

    public void progressbarCharging(float progress)
    {
        if (!this.progress.gameObject.activeSelf)
            this.progress.gameObject.SetActive(true);

        this.progress.fillAmount = progress;
    }
}
