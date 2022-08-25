using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DragWordElement203 : WordElement203, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private bool intractable = false;
    public event Action<WordElement203> onDrop;
    public event Action<DragWordElement203> onDrag;

    public AudioSinglePlayer audioPlayer;
    public AudioClip erorrClip;

    private Vector3 defaultPosition;
    private GraphicRaycaster caster => FindObjectOfType<GraphicRaycaster>();

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
    public void SetDefaultPosition()
    {
        defaultPosition = transform.position;
        intractable = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        var results = new List<RaycastResult>();
        caster.Raycast(eventData, results);
        var targets = results
            .Select(x => x.gameObject)
            .Where(x => x.GetComponent<DragWordElement203>() == null)
            .Select(x => x.GetComponent<WordElement203>())
            .Where(x => x != null);
        if (targets.Count() > 0 && targets.First().value == value)
        {
            gameObject.SetActive(false);
            onDrop?.Invoke(targets.First());
        }
        else
        {
            audioPlayer.Play(erorrClip);
            transform.position = defaultPosition;
        }
    }
}