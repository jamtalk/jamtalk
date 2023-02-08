using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectChild : MonoBehaviour
{
    public RectTransform[] rects;
    public ChildIconElement childIconOrizin;
    private List<ChildIconElement> childList = new List<ChildIconElement>();
    public Action addAction;
    public Action selectAction;

    public void Init()
    {
        var dataList = UserDataManager.Instance.childList;

        if (dataList.Length < 2)
        {
            var addButton = Instantiate(childIconOrizin, rects[dataList.Length]);
            addButton.Init(null);
            addButton.addAction += () => addAction?.Invoke();
        }

        for (int i = 0; i < dataList.Length; i++)
        {
            var childIcon = Instantiate(childIconOrizin, rects[i]);
            childIcon.Init(dataList[i]);
            childIcon.addAction += () =>
            {
                // 선택된 아이 display true 로 변경 , 나머지 아이 false,
                selectAction?.Invoke();
            };
        }
    }

}
