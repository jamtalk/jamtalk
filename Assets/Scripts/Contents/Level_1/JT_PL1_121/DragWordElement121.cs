using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DragWordElement121 : WordElement121, IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField]
    private bool intractable = false;
    public event Action<WordElement121> onDrop;
    private Vector3 defaultPosition;
    [SerializeField]
    public AudioSinglePlayer audioPlayer;
    private GraphicRaycaster caster => FindObjectOfType<GraphicRaycaster>();
    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        transform.position = eventData.position;
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
            .Where(x => x.GetComponent<DragWordElement121>() == null)
            .Select(x => x.GetComponent<WordElement121>())
            .Where(x => x != null);
        if (targets.Count() > 0 && targets.First().value == value)
        {
            gameObject.SetActive(false);
            onDrop?.Invoke(targets.First());
        }
        else
            transform.position = defaultPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        audioPlayer.Play(GameManager.Instance.schema.GetSiteWordsClip(name));
    }
}
