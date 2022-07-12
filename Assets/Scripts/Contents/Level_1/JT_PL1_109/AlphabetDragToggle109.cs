using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class AlphabetDragToggle109 : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public GraphicRaycaster caster;
    public Image image;
    private Sprite spriteOff;
    private Sprite spriteOn;
    private bool _isOn = false;
    public event System.Action onEndDrag;
    public int index { get; private set; }
    public bool isOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            image.sprite = value ? spriteOn : spriteOff;
            image.preserveAspect = true;
        }
    }
    public eAlphabet value { get; private set; }
    public void Init(eAlphabet value,int index ,bool isOn = false)
    {
        this.value = value;
        this.index = index;
        spriteOn = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.NeonRed,eAlphabetType.Lower,value);
        spriteOff = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.NeonYellow, eAlphabetType.Lower, value);
        this.isOn = isOn;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isOn = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var list = new List<RaycastResult>();
        caster.Raycast(eventData, list);
        var components = list.
            Select(x => x.gameObject.GetComponent<AlphabetDragToggle109>())
            .Where(x => x != null)
            .Where(x=>!x.isOn);
        if(components.Count()>0)
        {
            var target = components.First();
            target.isOn = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke();
    }
}
