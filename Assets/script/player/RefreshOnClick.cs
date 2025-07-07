using UnityEngine;
using UnityEngine.EventSystems;

public class RefreshOnClick : MonoBehaviour , IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.Refresh();
    }
}
