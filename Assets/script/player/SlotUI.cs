using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image BGImage;
    public Image itemImage;
    public GameObject panelPrefab;
    public Canvas canvas;
    public ItemSlotContent isc;
    public int listIndex;
    public int index;
    public string type;
    public Sprite sourceImage;
    public Sprite pressedImage;
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
        if (type == "dock" &&
            Wallet.Instance.selectedHotbarSlotIndex == index &&
            Wallet.Instance.selectedHotbarIndex == listIndex &&
            !UIManager.Instance.isBottomPanelOn)
        {
            BGImage.transform.localScale = new Vector3(1.50f, 1.50f, 1.50f);
            itemImage.transform.localScale = new Vector3(1.50f, 1.50f, 1.50f);
        }
        else
        {
            BGImage.transform.localScale = Vector3.one;
            itemImage.transform.localScale = Vector3.one;
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
        else if (eventData.button == PointerEventData.InputButton.Right && slotId.Value != 0)
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
            GameObject panel = Instantiate(panelPrefab, canvas.transform);
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
        BGImage.transform.localScale = new Vector3(1.20f, 1.20f, 1.20f);
        itemImage.transform.localScale = new Vector3(1.20f, 1.20f, 1.20f);

        // Seçili slot bu slot ise item adını göster
        if (Wallet.Instance.selectedID != null &&
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
    }

    // Fare slotun üzerinden ayrıldığında tetiklenir
    public void OnPointerExit(PointerEventData eventData)
    {
        BGImage.transform.localScale = Vector3.one;
        itemImage.transform.localScale = Vector3.one;

        if (currentItemNameObj != null)
        {
            Destroy(currentItemNameObj);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        BGImage.sprite = pressedImage;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BGImage.sprite = sourceImage;
    }
}





