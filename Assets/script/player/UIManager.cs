using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public ScrollViewContent invSVC;
    public ScrollViewContent hotbarSVC;
    public ScrollViewContent gearSVC;
    public ScrollViewContent utilitySVC;
    public ScrollViewContent dockSVC;
    public StatPanelUI statPanelUI;

    public Transform bottomPanel;
    public Transform hotbarPanel;
    public Transform inventoryPanel;
    public Transform gearPanel;
    public Transform statPanel;
    public Transform utilityPanel;
    public Transform dockPanel;

    public ID informedID;

    public GameObject slotSelectionPrefab;

    private GameObject currentSelection;

    public SpriteCollection blueSelectionCollection;

    public SpriteCollection purpleSelectionCollection;

    public string bottomMode = "hotbar";
    public bool isBottomPanelOn = true;

    public GameObject currentFunctionPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        SlotUI selectedSlotUI = null;

        InitializeInventory();
        if (Wallet.Instance.selectedType == "inventory" && Wallet.Instance.selectedID != null)
        {
            selectedSlotUI = invSVC.scrollViewList[Wallet.Instance.selectedListIndex].GetComponent<ScrollViewInfo>().itemSlotContent.slotObjList[Wallet.Instance.selectedIndex].GetComponent<SlotUI>();
            if (selectedSlotUI == null)
            {
                Debug.LogError("Selected Slot UI is null!");
            }
        }
        if (isBottomPanelOn)
        {
            switch (bottomMode)
            {
                case "hotbar":
                    hotbarPanel.gameObject.SetActive(true);
                    utilityPanel.gameObject.SetActive(true);
                    gearPanel.gameObject.SetActive(false);
                    statPanel.gameObject.SetActive(false);
                    InitializeHotbar();
                    InitializeUtility();
                    if (Wallet.Instance.selectedType == "hotbar" && Wallet.Instance.selectedID != null)
                    {
                        selectedSlotUI = hotbarSVC.scrollViewList[Wallet.Instance.selectedListIndex].GetComponent<ScrollViewInfo>().itemSlotContent.slotObjList[Wallet.Instance.selectedIndex].GetComponent<SlotUI>();
                    }
                    break;
                case "gear":
                    gearPanel.gameObject.SetActive(true);
                    utilityPanel.gameObject.SetActive(true);
                    hotbarPanel.gameObject.SetActive(false);
                    statPanel.gameObject.SetActive(false);
                    InitializeGear();
                    InitializeUtility();
                    if (Wallet.Instance.selectedType == "gear" && Wallet.Instance.selectedID != null)
                    {
                        selectedSlotUI = gearSVC.scrollViewList[Wallet.Instance.selectedListIndex].GetComponent<ScrollViewInfo>().itemSlotContent.slotObjList[Wallet.Instance.selectedIndex].GetComponent<SlotUI>();
                    }
                    break;
                case "stat":
                    statPanel.gameObject.SetActive(true);
                    hotbarPanel.gameObject.SetActive(false);
                    gearPanel.gameObject.SetActive(false);
                    utilityPanel.gameObject.SetActive(false);
                    InitializeStat();
                    break;
                default:
                    InitializeHotbar();
                    break;
            }
        }
        else
        {
            hotbarPanel.gameObject.SetActive(false);
            gearPanel.gameObject.SetActive(false);
            statPanel.gameObject.SetActive(false);
            utilityPanel.gameObject.SetActive(false);

            dockPanel.gameObject.SetActive(true);
            InitializeDock();
            Debug.Log("Dock initialized");

        }
        if (selectedSlotUI != null)
        {
            InitializeSelection(selectedSlotUI);
        }
    }

    
    public void InitializeDock()
    {
        dockSVC.Initialize();
    }
    public void InitializeInventory()
    {
        invSVC.Initialize();
    }

    public void InitializeHotbar()
    {
        hotbarSVC.Initialize();
    }

    public void InitializeGear()
    {
        gearSVC.Initialize();
    }

    public void InitializeStat()
    {
        statPanelUI.Initialize();
    }

    public void InitializeUtility()
    {
        utilitySVC.Initialize();
    }

    public void AddHotbar()
    {
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
                    Initialize();
                    return;
                }
            }
        }
        if (Wallet.Instance.hotbarList.Count > 0)
        {
            Wallet.Instance.hotbarList[Wallet.Instance.hotbarList.Count - 1].Add(new ID(0));
            Initialize();
        }
    }

    public void RemoveHotbar()
    {
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
                Initialize();
                return;
            }
        }

        if (Wallet.Instance.hotbarList.Count > 1)
        {
            Wallet.Instance.hotbarList.RemoveAt(Wallet.Instance.hotbarList.Count - 1);
            Initialize();
        }
    }

    public void RemoveHotbarSlot()
    {
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
                    if (Wallet.Instance.selectedIndex < 0)
                    {
                        Wallet.Instance.selectedIndex = 0;
                    }
                    Initialize();
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
            Initialize();
        }
    }

    public void Refresh()
    {
        Destroy(currentSelection);
        Wallet.Instance.selectedID = null;
        if (currentFunctionPanel != null){Destroy(currentFunctionPanel);}
    }

    public void ToggleBottomModeHotbar()
    {
        ToggleBottomMode("hotbar");
    }
    public void ToggleBottomModeStat()
    {
        ToggleBottomMode("stat");
    }
    public void ToggleBottomModeGear()
    {
        ToggleBottomMode("gear");
    }
    public void ToggleBottomMode(string mode)
    {
        bottomMode = mode;
        Initialize();

    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    /// NewSelectionCode ///

    public void SelectSlot(SlotUI slot)
    {
        Wallet.Instance.SelectSlot(slot);
        informedID = null;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    
    public void InitializeInformation(SlotUI slot)
    {
        informedID = slot.slotId;
        Debug.Log(Wallet.Instance.GetSlotById(slot.slotId).item.id);
    }


    public void InitializeSelection(SlotUI slot)
    {
        if (currentSelection != null)
        {
            Destroy(currentSelection);
        }

        if (slotSelectionPrefab != null && slot != null)
        {
            currentSelection = Instantiate(slotSelectionPrefab, slot.transform);

            RectTransform selectionRect = currentSelection.GetComponent<RectTransform>();
            if (selectionRect != null)
            {
                selectionRect.anchoredPosition = Vector2.zero;
                selectionRect.localScale = Vector3.one;
            }

            LoopingImage loopingImage = currentSelection.GetComponent<LoopingImage>();
            if (loopingImage != null)
            {
                loopingImage.spriteCollection = blueSelectionCollection;
            }
            Debug.Log("Selection succeed");
        }
        else
        {
            Debug.LogError("slotSelectionPrefab veya slot null!");
        }
    }

    public void CloseInvPanel()
    {
        if (inventoryPanel != null)
        {
            invSVC.gameObject.SetActive(false);
            invSVC.ClearScrollViews();
            invSVC.scrollViewList.Clear();
        }
    }

    public void MaximizeInvPanel()
    {
        if (inventoryPanel != null)
        {
            invSVC.gameObject.SetActive(true);
            Initialize();
        }
    }

    public void MinimizeBottomPanel()
    {
        if (bottomPanel != null)
        {
            bottomPanel.gameObject.SetActive(false);
            dockPanel.gameObject.SetActive(false);
            hotbarSVC.ClearScrollViews();
            hotbarSVC.scrollViewList.Clear();
            gearSVC.ClearScrollViews();
            gearSVC.scrollViewList.Clear();
            dockPanel.gameObject.SetActive(true);

            isBottomPanelOn = false;
            Initialize();
        }
    }

    public void MaximizeBottomPanel()
    {
        if (bottomPanel != null)
        {
            bottomPanel.gameObject.SetActive(true);
            dockPanel.gameObject.SetActive(false);

            isBottomPanelOn = true;
            Initialize();
        }
    }
}
