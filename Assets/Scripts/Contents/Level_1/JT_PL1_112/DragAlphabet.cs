using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAlphabet : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GraphicRaycaster caster;
    public Image image;
    public bool intracable=true;
    public eAlphabet alphabet;
    public void Init(eAlphabet alphabet)
    {
        this.alphabet = alphabet;
        image.sprite = GameManager.Instance.GetAlphbetSprite(eAlphabetStyle.Dino, eAlphabetType.Upper, alphabet);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!intracable)
            return;
        var rt = GetComponent<RectTransform>();
        var pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = transform.position.z;
        rt.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!intracable)
            return;
        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(eventData, results);
        var components = results
            .Select(x => x.gameObject.GetComponent<DropToggle>())
            .Where(x=>x!=null)
            .ToList();

        if (components.Count() > 0)
        {
            var target = components.First();
            if(target.alphabet == alphabet)
            {
                target.isOn = true;
                gameObject.SetActive(false);
            }
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
