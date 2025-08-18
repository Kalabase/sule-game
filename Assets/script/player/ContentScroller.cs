using UnityEngine;
using UnityEngine.UI;

public class ContentScroller : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void Scroll()
    {
        StartCoroutine(DelayedScroll());
    }

    private System.Collections.IEnumerator DelayedScroll()
    {
        yield return null; // 1 frame bekle
        ScrollToSelectedSlot();
    }
    
    public void ScrollToSelectedSlot()
    {
        RectTransform content = GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport;

        float contentWidth = content.rect.width;
        float viewportWidth = viewport.rect.width;

        int slotCount = Wallet.Instance.hotbarList[Wallet.Instance.selectedHotbarIndex].Count;
        int selectedIndex = Wallet.Instance.selectedHotbarSlotIndex;

        float y = contentWidth - viewportWidth;
        if (y < 0) return; // İçerik görünümden küçükse kaydırma yok

        float k = (selectedIndex * 54f) + 30f;
        float p = (contentWidth / 2f) - k;

        float targetX;
        if (Mathf.Abs(p) * 2 <= y)
        {
            targetX = p;
        }
        else if (p > 0)
        {
            targetX = y / 2;
        }
        else // Mathf.Abs(p) > y && p < 0
        {
            targetX = -y / 2;
        }

        content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
        Debug.Log($"slot index: {selectedIndex}, content width: {contentWidth}, viewport width: {viewportWidth}, targetX: {targetX}, slot konumu: {k}, ortadan fark: {p}");
    }
}
