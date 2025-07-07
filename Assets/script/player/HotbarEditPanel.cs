using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarEditPanel : MonoBehaviour
{
    void Start()
    {
        SetAsLastChild();
    }

    private ItemSlotContent GetParentItemSlotContent()
    {
        ScrollViewInfo svInfo = GetComponentInParent<ScrollViewInfo>();
        ItemSlotContent itemSlotContent = svInfo.itemSlotContent;
        if (itemSlotContent == null)
        {
            Debug.LogError("ItemSlotContent script not found in parent.");
        }
        return itemSlotContent;
    }

    private bool IsWalletInitialized()
    {
        if (Wallet.Instance == null)
        {
            Debug.LogError("Wallet instance is not initialized.");
            return false;
        }
        return true;
    }

    public void AddHotbar()
    {
        if (!IsWalletInitialized()) return;

        List<ID> newHotbar = new List<ID> { new ID(0) };

        if (Wallet.Instance.selectedID != null && Wallet.Instance.selectedType == "hotbar")
        {
            int insertIndex = Wallet.Instance.selectedListIndex + 1;
            if (insertIndex >= 0 && insertIndex <= Wallet.Instance.hotbarList.Count)
            {
                Wallet.Instance.hotbarList.Insert(insertIndex, newHotbar);
            }
            else
            {
                Wallet.Instance.hotbarList.Add(newHotbar);
            }
        }
        else
        {
            Wallet.Instance.hotbarList.Add(newHotbar);
        }

        UIManager.Instance.Initialize();
    }

    public void AddHotbarSlot()
    {
        if (!IsWalletInitialized()) return;

        // Seçili hotbar ve slot varsa, ilgili hotbar'a, seçili slotun hemen sonrasına ekle
        if (Wallet.Instance.selectedID != null && Wallet.Instance.selectedType == "hotbar")
        {
            int listIndex = Wallet.Instance.selectedListIndex;
            int insertIndex = Wallet.Instance.selectedIndex + 1;

            if (listIndex >= 0 && listIndex < Wallet.Instance.hotbarList.Count)
            {
                List<ID> hotbar = Wallet.Instance.hotbarList[listIndex];
                if (insertIndex >= 0 && insertIndex <= hotbar.Count)
                {
                    hotbar.Insert(insertIndex, new ID(0));
                    UIManager.Instance.Initialize();
                    return;
                }
            }
        }
        if (Wallet.Instance.hotbarList.Count > 0)
        {
            Wallet.Instance.hotbarList[Wallet.Instance.hotbarList.Count - 1].Add(new ID(0));
            UIManager.Instance.Initialize();
        }
    }

    public void RemoveHotbar()
    {
        if (!IsWalletInitialized()) return;

        if (Wallet.Instance.selectedID != null && Wallet.Instance.selectedType == "hotbar")
        {
            int index = Wallet.Instance.selectedListIndex;
            if (index >= 0 && index < Wallet.Instance.hotbarList.Count && Wallet.Instance.hotbarList.Count > 1)
            {
                if (Wallet.Instance.selectedListIndex == index)
                {
                    Wallet.Instance.selectedID = null;
                }
                Wallet.Instance.hotbarList.RemoveAt(index);
                Wallet.Instance.selectedID = null;
                UIManager.Instance.Initialize();
                return;
            }
        }

        if (Wallet.Instance.hotbarList.Count > 1)
        {
            Wallet.Instance.hotbarList.RemoveAt(Wallet.Instance.hotbarList.Count - 1);
            UIManager.Instance.Initialize();
        }
    }

    public void RemoveHotbarSlot()
    {
        if (!IsWalletInitialized()) return;

        if (Wallet.Instance.selectedID != null && Wallet.Instance.selectedType == "hotbar")
        {
            int listIndex = Wallet.Instance.selectedListIndex;
            int slotIndex = Wallet.Instance.selectedIndex;

            if (listIndex >= 0 && listIndex < Wallet.Instance.hotbarList.Count)
            {
                List<ID> hotbar = Wallet.Instance.hotbarList[listIndex];
                if (slotIndex >= 0 && slotIndex < hotbar.Count && hotbar.Count > 1)
                {
                    hotbar.RemoveAt(slotIndex);
                    Wallet.Instance.selectedIndex--;
                    UIManager.Instance.Initialize();
                    return;
                }
            }
        }

        if (Wallet.Instance.hotbarList.Count > 0)
        {
            List<ID> lastHotbar = Wallet.Instance.hotbarList[Wallet.Instance.hotbarList.Count - 1];
            if (lastHotbar.Count > 1)
            {
                lastHotbar.RemoveAt(lastHotbar.Count - 1);
            }
            UIManager.Instance.Initialize();
        }
    }

    public void SetAsLastChild()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            Debug.LogError("HotbarEditPanel does not have a parent object.");
            return;
        }

        if (transform.GetSiblingIndex() == parent.childCount - 1)
        {
            return;
        }

        transform.SetAsLastSibling();
        Debug.Log($"{gameObject.name} is now the last child of its parent: {parent.name}");
    }
}