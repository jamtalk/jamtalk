using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using System.Linq;
using System;

public class SelectController : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public Button selectButton;
    public Text text;
    public RectTransform leftRt;
    public RectTransform rightRt;
    public CharactorElement[] elements;
    public RectTransform[] rects;

    private void Awake()
    {
        Init();

        selectButton.onClick.AddListener(() => Selected());
        leftButton.onClick.AddListener(() => OnClickListener(false));
        rightButton.onClick.AddListener(() => OnClickListener(true));
    }

    public void Selected()
    {
        var selectedElement = elements.Where(x => x.index == 2).First();

        selectedElement.selectedAction?.Invoke();
    }

    public void OnClickListener(bool isRight)
    {
        var index = isRight ? 1 : -1;
        var count = 0;

        if (isRight)
            count = elements.Where(x => x.index >= rects.Length).Count();
        else
            count = elements.Where(x => x.index < 0).Count();

        if (elements.Length - count == 3)
            return;

        foreach (var item in elements)
        {
            var temp = index + item.index;
            if (temp == 2) text.text = item.name;

            if (temp >= rects.Length)
                item.Move(rightRt, temp);
            else if (temp < 0)
                item.Move(leftRt, temp);
            else
                item.Move(rects[temp], temp);
        }
    }


    public void Init()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (i < rects.Length)
                elements[i].Init(rects[i], i);
            else
                elements[i].Init(rightRt, i);
            elements[i].name = elements[i].charactor.name;
            if (i == 2) text.text = elements[i].name;
        }
    }
}
