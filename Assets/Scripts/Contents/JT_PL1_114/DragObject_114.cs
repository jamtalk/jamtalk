using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObject_114:MonoBehaviour,IDragHandler,IDropHandler
{
    public GraphicRaycaster caster;
    public AudioSinglePlayer audioPlayer;
    public Image image=>button.image;
    public Button button;
    public RectTransform rt => GetComponent<RectTransform>();
    public event Action onDrop;
    public event Action<bool> onAnswer;
    public event Action onDrag;
    public bool intracable;
    public WordSource data;
    public eAlphabet alphabet => (eAlphabet)Enum.Parse(typeof(eAlphabet), image.sprite.name.First().ToString().ToUpper());
    private void Awake()
    {
        button.onClick.AddListener(() => audioPlayer.Play(data.clip));
    }
    public void Init(WordSource data)
    {
        this.data = data;
        image.sprite = data.sprite;
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!intracable)
            return;
        rt.position = eventData.position;
        onDrag?.Invoke();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!intracable)
            return;
        var list = new List<RaycastResult>();
        caster.Raycast(eventData, list);
        var components = list
            .Select(x => x.gameObject.GetComponent<DropSapceShip_114>())
            .Where(x => x != null);
        if(components.Count()>0)
        {
            var component = components.First();
            var correct = component.alphabet == alphabet;
            if (correct)
            {
                gameObject.SetActive(false);
                onDrop?.Invoke();
                component.InObject(data);
            }
            onAnswer?.Invoke(correct);
        }

        rt.anchoredPosition = Vector2.zero;
    }
}
