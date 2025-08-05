using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float scrollCooldown = 0.15f; // slot değişimi için minimum süre (saniye)
    private float lastScrollTime = 0f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Wallet.Instance.hotbarList == null || Wallet.Instance.hotbarList.Count == 0)
            return;

        int slotCount = Wallet.Instance.hotbarList[Wallet.Instance.selectedHotbarIndex].Count;
        bool slotChanged = false;

        // Sadece belirli aralıklarla slot değiştir
        if (Time.time - lastScrollTime > scrollCooldown)
        {
            if (scroll > 0.05f)
            {
                if (Wallet.Instance.selectedHotbarSlotIndex > 0)
                {
                    Wallet.Instance.selectedHotbarSlotIndex--;
                    slotChanged = true;
                    lastScrollTime = Time.time;
                }
            }
            else if (scroll < -0.05f)
            {
                if (Wallet.Instance.selectedHotbarSlotIndex < slotCount - 1)
                {
                    Wallet.Instance.selectedHotbarSlotIndex++;
                    slotChanged = true;
                    lastScrollTime = Time.time;
                }
            }
        }

        if (slotChanged)
        {
            UIManager.Instance.InitializeDock();
        }
    }
}