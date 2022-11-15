using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanCakeElement : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image BG;
    public Image image;
    public Text text;
    public SpatulaElement spatula;

    public Sprite firstSprite;
    public Sprite secondSprite;
    public Sprite lastSprite;

    public DigraphsWordsData data { get; private set; }

    public Action onFirst;
    public Action onDouble;

    public bool isCheck = false;
    private bool isHalf = false;
    private bool isStart = false;

    Coroutine coroutine;

    public void Init(DigraphsWordsData data)
    {
        this.data = data;
        text.text = data.Digraphs.ToString().ToLower();
        image.sprite = data.sprite;
        image.preserveAspect = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spatula.transform.position = GameManager.Instance.GetMousePosition();
            spatula.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isStart = false;
            if (coroutine != null)
                StopCoroutine(coroutine);
            spatula.gameObject.SetActive(false);
        }
    }

    private IEnumerator BeginDrag()
    {
        isStart = true;
        spatula.gameObject.SetActive(true);
        if (!isHalf)
        {
            transform.DOShakePosition(1f, 10f);
            yield return new WaitForSecondsRealtime(1f);
            onFirst?.Invoke();
            isHalf = true;
        }

        BG.sprite = secondSprite;
        transform.DOShakePosition(1f, 10f);
        yield return new WaitForSecondsRealtime(1f);
        onDouble?.Invoke();
        isStart = false;
        spatula.gameObject.SetActive(false);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCheck || isStart)
            return;
        coroutine = StartCoroutine(BeginDrag());
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = GameManager.Instance.GetMousePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
