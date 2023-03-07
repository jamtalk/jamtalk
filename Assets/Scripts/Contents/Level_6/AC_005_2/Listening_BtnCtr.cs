using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Listening_BtnCtr : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonReplay;
    public Button buttonNext;
    public Button buttonPrevious;

    private int index = 0;
    public Action action;
    public Action nextAction;

    private void Awake()
    {
        buttonReplay.onClick.AddListener(() => ShowPage(index));
        buttonPlay.onClick.AddListener(() => ShowPage(0));
        buttonPrevious.onClick.AddListener(() => ShowPage(index - 1));
        buttonNext.onClick.AddListener(NextPage);
    }

    public void ShowPage(int index)
    {
        this.index = index;
        action?.Invoke();
    }

    public void NextPage()
    {
        nextAction?.Invoke();
    }

    public void SetActive(bool isActive)
    {
        buttonPlay.gameObject.SetActive(isActive);
        buttonReplay.gameObject.SetActive(isActive);
        buttonNext.gameObject.SetActive(isActive);
        buttonPrevious.gameObject.SetActive(isActive);
        gameObject.SetActive(isActive);
    }
}
