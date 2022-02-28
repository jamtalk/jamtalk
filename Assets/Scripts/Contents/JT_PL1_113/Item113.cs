using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

public class Item113 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private bool intractable=true;
    public GraphicRaycaster caster;
    private RectTransform rt => GetComponent<RectTransform>();
    public Image product;
    public Text textValue;
    public event System.Action onThrowing; 
    public event System.Action onThrowed;
    public eAlphabet value { get; private set; }
    public void Init(eAlphabet? value, Sprite product)
    {
        ResetPosition();
        gameObject.SetActive(true);
        this.product.sprite = product;
        intractable = value != null;
        if (value != null)
        {
            this.value = (eAlphabet)value;
            var text = this.value.ToString();
            if (Random.Range(0, 2) > 0)
                text.ToLower();
            textValue.text = text;
        }
        else
        {
            textValue.text = string.Empty;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;

        rt.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;

        var list = new List<RaycastResult>();
        caster.Raycast(eventData, list);
        var components = list
            .Select(x => x.gameObject.GetComponent<Charactor113>())
            .Where(x => x != null);
        if (components.Count() > 0)
        {
            var component = components.First();
            if(component.value == value)
            {
                gameObject.SetActive(false);
                component.SetProduct(product.sprite,textValue.text);
            }
            else
                Throwing();
        }
        else
            ResetPosition();
    }

    public void Throwing()
    {
        var duration = .25f;
        var pos = rt.anchoredPosition;
        float distance = Random.Range(100f, 200f);
        var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * distance;
        var target = pos + direction;

        var seq = DOTween.Sequence();

        var moveTween = rt.DOAnchorPos(target, duration);
        seq.Insert(0, moveTween);

        var rotateTween = rt.DORotate(new Vector3(0, 0, 180), duration);
        seq.Insert(0, rotateTween);

        var scaleTween = rt.DOScale(0, duration);
        seq.Insert(0, scaleTween);

        seq.onComplete += ResetPosition;
        seq.onComplete += () => onThrowed?.Invoke();
        seq.Play();
        onThrowing?.Invoke();
    }
    public void ResetPosition()
    {
        rt.localScale = Vector3.one;
        rt.rotation = Quaternion.Euler(Vector3.zero);
        rt.anchoredPosition = Vector3.zero;
    }
}
