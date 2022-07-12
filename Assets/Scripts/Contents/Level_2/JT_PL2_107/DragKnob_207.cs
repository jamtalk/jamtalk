using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragKnob_207 : DragKnob_107
{
    public SandEffect effect;

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        effect.gameObject.SetActive(true);
        effect.transform.position = eventData.position;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        effect.gameObject.SetActive(false);
    }
}
