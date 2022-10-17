using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragElement301 : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public Vector2 prePos;
    [HideInInspector]
    public float distance = 0;
    [SerializeField]
    private float maxDistance;
    [HideInInspector]
    private float progress => distance / maxDistance;

    public BrushElement301 brush;
    public Image[] colorImages;
    public Image resultColorImage;
    public Text resultText;
    public bool isColors = false;
    public event Action<DragElement301> onDrop;

    private GraphicRaycaster caster => FindObjectOfType<GraphicRaycaster>();
    private Color colors;
    private Color resultColor;

    private void Awake()
    {
        colors = colorImages[0].color;
        resultColor = resultColorImage.color;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isColors)
            return;
        prePos = eventData.position;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (isColors)
            return;

        var results = new List<RaycastResult>();
        caster.Raycast(eventData, results);
        var targets = results
            .Select(x => x.gameObject.GetComponent<DragElement301>())
            .Where(x => x != null);

        if (targets.Count() < 0)
            return;

        var dis = Vector2.Distance(prePos, eventData.position);
        prePos = eventData.position;
        distance += dis;

        colors.a = (2f - progress);
        //resultText.color = colors;
        colorImages[0].color = colors;
        colorImages[1].color = colors;

        resultColor.a = progress;
        resultColorImage.color = resultColor;

        var temp = Camera.main.ScreenToWorldPoint(eventData.position);
        temp.z = 0f;
        brush.transform.position = temp;

        if (progress > 1f)
        {
            resultText.gameObject.SetActive(true);
            distance = 0f;
            isColors = true;
            onDrop?.Invoke(this);
        }
    }
}
