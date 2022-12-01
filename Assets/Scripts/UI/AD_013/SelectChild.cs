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

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        // child list 전송받은 후 생성 및 출력

        if (childList.Count == 0)
        {
            var addButton = Instantiate(childIconOrizin, rects[0]);
            addButton.Init(true);
            addButton.addAction += () => addAction?.Invoke();
        }
    }

}
