using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarElement506 : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public CanvasScaler scaler;
    public GraphicRaycaster caster;
    public bool intractable = true;
    public Image line;
    private RectTransform line_rt => line.GetComponent<RectTransform>();

    public Text text;

    public void Init(string value)
    {
        intractable = true;
        text.text = value;    
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intractable)
            return;
        SetLine(eventData.position);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    private void SetLine(Vector2 position)
    {
        line.gameObject.SetActive(true);

        var v1 = position - (Vector2)line_rt.position;
        var angle = Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg - 90f;
        line_rt.rotation = Quaternion.Euler(0, 0, angle);

        var dis = Vector2.Distance(FactorPos(line_rt.position), FactorPos(position));
        Debug.DrawLine(line_rt.position, position);
        var size = line_rt.sizeDelta;
        size.y = dis;
        line_rt.sizeDelta = size;
    }

    public Vector2 FactorPos(Vector2 pos)
    {
        var resolution = scaler.referenceResolution;
        var current = new Vector2(Screen.width, Screen.height);
        var ratio = current / resolution;
        return pos * ratio;
    }

}
