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
    public int currentIndex=>UserDataManager.Instance.CurrentChild.character_pick;
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
        leftButton.onClick.AddListener(() => OnClickListener(true));
        rightButton.onClick.AddListener(() => OnClickListener(false));

        if(currentIndex<2)
        {
            for (int i = 0; i < 2 - currentIndex; i++)
                OnClickListener(true);
        }
        else
        {
            for (int i = 0; i < currentIndex - 2; i++)
                OnClickListener(false);
        }
    }

    public void Selected()
    {
        var selectedElement = elements.Where(x => x.index == 2).First();
        int index = elements.ToList().IndexOf(selectedElement);
        var child = UserDataManager.Instance.CurrentChild;
        child.character_pick = index;
        RequestManager.Instance.Request(new ChildOutParam(UserDataManager.Instance.CurrentChild), response =>
        {
            UserDataManager.Instance.UpdateChildren(()=>
            {
                AndroidPluginManager.Instance.Toast("캐릭터가 변경되었습니다");
            });
        });
        selectedElement.selectedAction?.Invoke();
    }

    public void OnClickListener(bool isLeft)
    {
        var index = isLeft ? 1 : -1;
        var count = 0;

        if (isLeft)
            count = elements.Where(x => x.index >= rects.Length).Count();
        else
            count = elements.Where(x => x.index < 0).Count();

        if (elements.Length - count == 3)
            return;

        foreach (var item in elements)
        {
            var temp = index + item.index;
            if (temp == 2)
            {
                item.aniCharactor.CenterAction();
                text.text = item.name;
            }
            else
            {
                item.aniCharactor.SideAction();
            }

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
            elements[i].name = elements[i].aniCharactor.charactor.name;
            if (i == 2) text.text = elements[i].name;
        }
    }
}
