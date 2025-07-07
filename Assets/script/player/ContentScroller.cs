using UnityEngine;
using UnityEngine.UI;

public class ContentScroller : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void Update()
    {
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

        if (contentWidth <= viewportWidth || slotCount <= 1)
        {
            // Center content if it's smaller than the viewport
            //float offset = (viewportWidth - contentWidth) / 2f;
            //content.anchoredPosition = new Vector2(offset, content.anchoredPosition.y);
            return;
        }

        // Calculate center index (e.g., 3 for 7 slots)
        float centerIndex = (slotCount - 1) / 2f;

        // How far selected index is from center
        float offsetFromCenter = selectedIndex - centerIndex;

        // Scroll amount per slot
        float scrollStep = (contentWidth - viewportWidth) / (slotCount - 1);

        // Negative because moving right means content moves left
        float targetX = -scrollStep * offsetFromCenter;

        content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
    }
}
