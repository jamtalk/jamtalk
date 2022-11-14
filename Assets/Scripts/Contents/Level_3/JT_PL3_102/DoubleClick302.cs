using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DoubleClick302 : DoubleClickButton, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Text textPhanix;
    public Image images;
    public DigraphsWordsData data { get; private set; }

    public Action onPress;
    public Action onFirst;
    public Action onDouble;

    public bool isCheck = false;
    private bool isHalf = false;


    public void Init(DigraphsWordsData data)
    {
        this.data = data;
        textPhanix.text = data.Digraphs.ToString().ToLower();
        images.sprite = data.sprite;
    }

    private IEnumerator BeginDrag()
    {
        if (!isHalf)
        {
            transform.DOShakePosition(2f);
            yield return new WaitForSecondsRealtime(1f);
            onFirst?.Invoke();
            isHalf = true;
        }

        SetLastImages();
        yield return new WaitForSecondsRealtime(1f);
        onDouble?.Invoke();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //onPress?.Invoke();
        //transform.position = GameManager.Instance.GetMousePosition();
        StartCoroutine(BeginDrag());
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = GameManager.Instance.GetMousePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
