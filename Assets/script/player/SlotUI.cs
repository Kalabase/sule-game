using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image BGImage;
    public Image itemImage;
    public Canvas canvas;
    public ItemSlotContent isc;
    public int listIndex;
    public int index;
    public string type;
    public Sprite sourceImage;
    public Sprite pressedImage;
    private Vector2 originalItemImagePos;

    private GameObject currentSelectionObj;
    public ID slotId;

    public GameObject itemNamePrefab;
    private GameObject currentItemNameObj;

    public void Initialize()
    {
        canvas = FindFirstObjectByType<Canvas>();
        LoadSlot();
    }

    public void LoadSlots()
    {
        LoadSlot();
    }

    public void LoadSlot()
    {
        if (UIManager.Instance.bottomMode == "hotbar" && type == "utility")
        {
            switch (index)
            {
                case 0:
                    BGImage.sprite = Resources.Load<Sprite>($"sprite/ui/remove_hotbar_slot");
                    break;
                case 1:
                    BGImage.sprite = Resources.Load<Sprite>($"sprite/ui/remove_hotbar");
                    break;
                case 2:
                    BGImage.sprite = Resources.Load<Sprite>($"sprite/ui/add_hotbar");
                    break;
                case 3:
                    BGImage.sprite = Resources.Load<Sprite>($"sprite/ui/add_hotbar_slot");
                    break;
            }
        }

        if (type == "dock" &&
            Wallet.Instance.selectedHotbarSlotIndex == index &&
            !UIManager.Instance.isBottomPanelOn)
        {
            currentSelectionObj = Instantiate(UIManager.Instance.slotSelectionPrefab, BGImage.transform);
        }
        else if (Wallet.Instance.selectedID != null &&
                type == Wallet.Instance.selectedType &&
                Wallet.Instance.selectedListIndex == listIndex &&
                Wallet.Instance.selectedIndex == index
                )
        {
            currentSelectionObj = Instantiate(UIManager.Instance.slotSelectionPrefab, BGImage.transform);
            UIManager.Instance.currentSelectionObj = currentSelectionObj;
        }
        else if (currentSelectionObj != null)
        {
            Destroy(currentSelectionObj);
            UIManager.Instance.currentSelectionObj = null;
        }

        if (slotId.Value == 0)
        {
            Sprite itemSprite = Resources.Load<Sprite>($"sprite/empty");
            itemImage.sprite = itemSprite;
            return;
        }

        ItemSlot itemSlot = null;

        if (type == "utility")
        {
            itemSlot = Wallet.Instance.GetUtilitySlotById(slotId, UIManager.Instance.bottomMode);
        }
        else
        {
            itemSlot = Wallet.Instance.GetSlotById(slotId);
        }

        if (itemSlot == null)
        {
            Debug.LogError($"ItemSlot not found for ID: {slotId.Value}");
            return;
        }

        if (itemSlot.item != null)
        {
            Sprite itemSprite = Resources.Load<Sprite>($"sprite/item/{itemSlot.item.id}");
            if (itemSprite != null)
            {
                itemImage.sprite = itemSprite;
            }
            else
            {
                Debug.LogError($"Sprite not found for item id: {itemSlot.item.id}");
            }
        }
        else
        {
            Sprite itemSprite = Resources.Load<Sprite>($"sprite/empty");
            itemImage.sprite = itemSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Destroy(UIManager.Instance.currentFunctionPanel);
            UIManager.Instance.SelectSlot(this);
        }
        if (eventData.button == PointerEventData.InputButton.Right && slotId.Value != 0)
        {
            Destroy(UIManager.Instance.currentFunctionPanel);
            InitializeFunctionPanel();
        }
    }

    void InitializeFunctionPanel()
    {
        ItemSlot itemSlot = Wallet.Instance.GetSlotById(slotId);

        if (itemSlot.item != null)
        {
            GameObject panel = Instantiate(UIManager.Instance.functionPanelPrefab, canvas.transform);
            UIManager.Instance.currentFunctionPanel = panel;

            panel.transform.position = this.transform.position;

            FunctionPanelUI functionPanelUI = panel.GetComponent<FunctionPanelUI>();
            if (functionPanelUI != null)
            {
                functionPanelUI.Initialize(itemSlot);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var bgRect = BGImage.GetComponent<RectTransform>();
        var itemRect = itemImage.GetComponent<RectTransform>();

        bgRect.offsetMin = new Vector2(bgRect.offsetMin.x, bgRect.offsetMin.y + 3f); // bottom
        bgRect.offsetMax = new Vector2(bgRect.offsetMax.x, bgRect.offsetMax.y + 3f); // top

        itemRect.offsetMin = new Vector2(itemRect.offsetMin.x, itemRect.offsetMin.y + 3f);
        itemRect.offsetMax = new Vector2(itemRect.offsetMax.x, itemRect.offsetMax.y + 3f);

        // if (currentSelectionObj != null)
        // {
        //     var selectionRect = currentSelectionObj.GetComponent<RectTransform>();
        //     selectionRect.localPosition = new Vector2(selectionRect.localPosition.x, selectionRect.localPosition.y + 3f);
        // }

        // Seçili slot bu slot ise item adını göster
        if (Wallet.Instance.selectedID != null &&
            Wallet.Instance.selectedID.Value != 0 &&
            Wallet.Instance.selectedListIndex == listIndex &&
            Wallet.Instance.selectedIndex == index &&
            Wallet.Instance.selectedType == type)
        {
            if (itemNamePrefab != null && currentItemNameObj == null)
            {
                currentItemNameObj = Instantiate(itemNamePrefab, this.transform);
                currentItemNameObj.GetComponentInChildren<LocalizedText>().key = Wallet.Instance.GetSlotById(slotId).item.id;
            }
        }

        UIManager.Instance.pointerSlotUI = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var bgRect = BGImage.GetComponent<RectTransform>();
        var itemRect = itemImage.GetComponent<RectTransform>();

        bgRect.offsetMin = new Vector2(bgRect.offsetMin.x, 0);
        bgRect.offsetMax = new Vector2(bgRect.offsetMax.x, 0);

        itemRect.offsetMin = new Vector2(itemRect.offsetMin.x, 0);
        itemRect.offsetMax = new Vector2(itemRect.offsetMax.x, 0);

        // if (currentSelectionObj != null)
        // {
        //     var selectionRect = currentSelectionObj.GetComponent<RectTransform>();
        //     selectionRect.localPosition = Vector2.zero;
        //     selectionRect.anchoredPosition = Vector2.zero;
        // }

        if (UIManager.Instance.hoveredSlotUI == this)
        {
            Destroy(currentSelectionObj);
            currentSelectionObj = null;
        }

        if (currentItemNameObj != null)
        {
            Destroy(currentItemNameObj);
        }

        UIManager.Instance.pointerSlotUI = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var bgRect = BGImage.GetComponent<RectTransform>();
        var itemRect = itemImage.GetComponent<RectTransform>();
        

        bgRect.offsetMin = new Vector2(bgRect.offsetMin.x, -6f);
        bgRect.offsetMax = new Vector2(bgRect.offsetMax.x, -6f);

        itemRect.offsetMin = new Vector2(itemRect.offsetMin.x, -6f);
        itemRect.offsetMax = new Vector2(itemRect.offsetMax.x, -6f);

        // if (currentSelectionObj != null)
        // {
        //     var selectionRect = currentSelectionObj.GetComponent<RectTransform>();
        //     selectionRect.localPosition = new Vector2(selectionRect.localPosition.x, selectionRect.localPosition.y - 6f);
        // }

        eventData.Use();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var bgRect = BGImage.GetComponent<RectTransform>();
        var itemRect = itemImage.GetComponent<RectTransform>();

        bgRect.offsetMin = new Vector2(0, 0);
        bgRect.offsetMax = new Vector2(0, 0);

        itemRect.offsetMin = new Vector2(0, 0);
        itemRect.offsetMax = new Vector2(0, 0);

        // if (currentSelectionObj != null)
        // {
        //     var selectionRect = currentSelectionObj.GetComponent<RectTransform>();
        //     selectionRect.localPosition = Vector2.zero;
        // }
    }
}





