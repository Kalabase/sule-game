using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (!UIManager.Instance.isBottomPanelOn)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Wallet.Instance.hotbarList == null || Wallet.Instance.hotbarList.Count == 0)
                return;

            int slotCount = Wallet.Instance.hotbarList[Wallet.Instance.selectedHotbarIndex].Count;
            int prevSelectedIndex = Wallet.Instance.selectedHotbarSlotIndex;
            bool slotChanged = false;

            if (scroll > 0.05f)
            {
                if (Wallet.Instance.selectedHotbarSlotIndex > 0)
                {
                    Wallet.Instance.selectedHotbarSlotIndex--;
                    slotChanged = true;
                }
            }
            else if (scroll < -0.05f)
            {
                if (Wallet.Instance.selectedHotbarSlotIndex < slotCount - 1)
                {
                    Wallet.Instance.selectedHotbarSlotIndex++;
                    slotChanged = true;
                }
            }

            if (slotChanged)
            {
                // Eski ve yeni slotları güncelle
                var dockSvc = UIManager.Instance.dockSVC;
                var itemSlotContent = dockSvc.scrollViewList[0].GetComponent<ScrollViewInfo>().itemSlotContent;

                // Eski slot
                if (prevSelectedIndex >= 0 && prevSelectedIndex < itemSlotContent.slotObjList.Count)
                {
                    var prevSlotUI = itemSlotContent.slotObjList[prevSelectedIndex].GetComponent<SlotUI>();
                    prevSlotUI.LoadSlot();
                }
                // Yeni slot
                int newSelectedIndex = Wallet.Instance.selectedHotbarSlotIndex;
                if (newSelectedIndex >= 0 && newSelectedIndex < itemSlotContent.slotObjList.Count)
                {
                    var newSlotUI = itemSlotContent.slotObjList[newSelectedIndex].GetComponent<SlotUI>();
                    newSlotUI.LoadSlot();
                }

                var contentScroller = itemSlotContent.GetComponent<ContentScroller>();
                if (contentScroller != null)
                {
                    contentScroller.ScrollToSelectedSlot();
                }
            }
        }
    }
}