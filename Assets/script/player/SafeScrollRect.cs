using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SafeScrollRect : ScrollRect
{
    public RectTransform scrollArea;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(scrollArea, eventData.position, eventData.pressEventCamera))
            return;

        base.OnBeginDrag(eventData);
    }
}
