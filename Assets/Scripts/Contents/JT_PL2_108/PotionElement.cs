using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PotionElement : MonoBehaviour , IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private bool interactable = false;
    public event Action<PotionElement> onDrop;
    public event Action<PotionElement> onDrag;
    public Text textValue;
    private Vector3 defaultPosition;
    
    public Image image;
    [SerializeField]
    private Sprite[] potionSprites;

    private GraphicRaycaster caster => FindObjectOfType<GraphicRaycaster>();

    public WordSource data { get; private set; }

    public void Init(WordSource data)
    {
        this.data = data;
        textValue.text = data.value;
        name = data.value;
        image.sprite = potionSprites[Random.Range(0, potionSprites.Length)];
        gameObject.SetActive(true);
    }

    public void SetDefaultPosition()
    {
        defaultPosition = transform.position;
        interactable = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable)
            return;
        transform.position = GameManager.Instance.GetMousePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!interactable)
            return;

        var results = new List<RaycastResult>();
        caster.Raycast(eventData, results);

        var targets = results
            .Select(x => x.gameObject.GetComponent<PotElement>())
            .Where(x => x != null);

        if (targets.Count() > 0)
        {
            gameObject.SetActive(false);
            onDrop?.Invoke(this);
        }
        else
        {
            ResetPosition();
            image.gameObject.SetActive(true);
            textValue.gameObject.SetActive(true);
        }
            
    }
    public void ResetPosition()
    {
        transform.position = defaultPosition;
        gameObject.SetActive(true);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.position = GameManager.Instance.GetMousePosition();
        onDrag?.Invoke(this);
    }
}
