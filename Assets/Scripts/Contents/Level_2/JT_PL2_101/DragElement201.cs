using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragElement201 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool intractable = false;
    public event Action<DragElement201> onDrag;
    public event Action<WordElement201> onDrop;
    public RectTransform rt;

    private Vector3 defaultPosition;
    private GraphicRaycaster caster => FindObjectOfType<GraphicRaycaster>();

    public void ResetPosition()
    {
        transform.position = defaultPosition;
    }

    public void SetDefaultPosition()
    {
        defaultPosition = transform.position;
        intractable = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        var pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = transform.position.z;
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        var results = new List<RaycastResult>();
        caster.Raycast(eventData, results);
        var targets = results
            .Select(x => x.gameObject)
            .Where(x => x.GetComponent<DragElement201>() == null)
            .Select(x => x.GetComponent<WordElement201>())
            .Where(x => x != null)
            .Where(x => x.name == name);
        
        if (targets.Count() > 0 && targets.First().name == name)
        {
            gameObject.SetActive(false);
            targets.First().GetComponent<Image>().sprite = GetComponent<Image>().sprite;
            onDrop?.Invoke(targets.First());
        }
        else
        {
            //audioPlayer.Play(erorrClip);
            transform.position = defaultPosition;
        }
    }
}
