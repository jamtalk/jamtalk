using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileSettingPage : MonoBehaviour
{
    public ChildViewPage childViewPage;
    public AccountViewPage accountViewPage;

    private void Awake()
    {
        childViewPage.Init();
        accountViewPage.Init();
    }
}
