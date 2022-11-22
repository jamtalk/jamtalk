using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

public class MonsterElemet : PoolingElement
{
    public GameObject element;
    public Image image;
    public Sprite[] sprites;
    public RectTransform topRt;
    public RectTransform centerRt;
    public RectTransform bottomRt;

    public enum eDirection { Top, Bottom, Center }
    public eDirection position = eDirection.Center;
    public eDirection direction = eDirection.Top;

    Coroutine coroutine;
    private void OnEnable()
    {
        image.sprite = sprites[Random.Range(0, sprites.Length)];
        image.preserveAspect = true;

        
    }
    public void StartCoroutine()
    {
        coroutine = StartCoroutine(MoveElement()); 
    }

    public void StopCoroutine()
    {
        if (coroutine == null)
            return;

        StopCoroutine(coroutine);
    }

    IEnumerator MoveElement()
    {
        yield return new WaitForSecondsRealtime(Random.Range(0f, 2.0f));
        while(true)
        {
            Move();
            var delay = Random.Range(3, 10);
            yield return new WaitForSecondsRealtime(delay);
        }
    }


    private void Move()
    {
        RectTransform rt;
        if (position == eDirection.Center)
        {
            direction = Random.Range(0, 2) == 0 ? eDirection.Top : eDirection.Bottom;
            rt = direction == eDirection.Top ? topRt : bottomRt;
        }
        else
        {
            direction = eDirection.Center;
            rt = centerRt;
        }

        position = direction;

        Sequence seq = DOTween.Sequence();

        seq.Append(element.transform.DOMove(rt.position, 2f));

        seq.Play();
    }
}
