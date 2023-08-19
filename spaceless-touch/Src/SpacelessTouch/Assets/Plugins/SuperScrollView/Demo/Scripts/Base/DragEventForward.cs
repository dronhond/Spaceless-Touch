using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class DragEventForward : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect parentScrollRect;
    ScrollRect scrollRect;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        if (parentScrollRect == null)
        {
            parentScrollRect = transform.parent.GetComponentInParent<ScrollRect>();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(parentScrollRect != null)
        {
            parentScrollRect.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float dragAngle = Vector2.Angle(eventData.delta, Vector2.up);
        bool isHorizonalDrag = (dragAngle > 45f && dragAngle < 135f);
        if(isHorizonalDrag != scrollRect.horizontal)
        {
            if (parentScrollRect != null)
            {
                parentScrollRect.OnDrag(eventData);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentScrollRect != null)
        {
            parentScrollRect.OnEndDrag(eventData);
        }
    }
}
