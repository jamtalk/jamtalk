using UnityEngine;
using UnityEngine.EventSystems;

public abstract class KnobPoint107<T> : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public T Parent;
    public event System.Action<PointerEventData> onDrag;
    public event System.Action<PointerEventData> onEndDrag;
    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke(eventData);
    }
}
