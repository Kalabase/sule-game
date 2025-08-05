using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public static Wallet Instance { get; private set; }

    public string nickname { get; private set; }
    public List<ItemSlot> inventory { get; private set; }
    public List<List<ID>> hotbarList { get; private set; }
    public List<List<ID>> gearList { get; private set; }
    public Dictionary<string, List<ItemSlot>> utilityList { get; private set; }
    public List<List<ID>> gearPages { get; private set; }
    public int gearPageIndex = 0;

    public int STR, maxSTR, currentSTR;
    public int RST, maxRST, currentRST;
    public int NRG, maxNRG, currentNRG;
    public int AUR, maxAUR, currentAUR;
    public int CHI, maxCHI, currentCHI;
    public int MNA, maxMNA, currentMNA;

    public ID selectedID;
    public string selectedType;
    public int selectedListIndex;
    public int selectedIndex;


    public int selectedHotbarIndex = 0;
    public int selectedHotbarSlotIndex = 0;

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
        Debug.Log("Wallet initialized.");
        nickname = "Kalabase";

        inventory = new List<ItemSlot>();

        hotbarList = new List<List<ID>>();

        gearList = new List<List<ID>>();
        for (int i = 0; i < 4; i++)
        {
            var list = new List<ID>();
            list.Add(new ID(0));
            gearList.Add(list);
        }

        gearPages = new List<List<ID>>();
        for (int i = 0; i < 9; i++)
        {
            gearPages.Add(new List<ID>());
        }


        utilityList = new Dictionary<string, List<ItemSlot>>();
        utilityList["gear"] = new List<ItemSlot>();
        for (int i = 0; i < 9; i++)
        {
            ItemSlot newSlot = new ItemSlot { item = null, count = 0 };
            utilityList["gear"].Add(newSlot);
        }
        utilityList["hotbar"] = new List<ItemSlot>();
        for (int i = 0; i < 4; i++)
        {
            ItemSlot newSlot = new ItemSlot { item = null, count = 0 };
            utilityList["hotbar"].Add(newSlot);
        }

        for(int i = 0; i < 64; i++)
        {
            AddRandomItem();
        }
        
        hotbarList.Add(new List<ID> { new ID(0), inventory[1].id, inventory[2].id, inventory[3].id, new ID(0) });

        UIManager.Instance.Initialize();

        maxSTR = 40;
        currentSTR = 25;
        STR = 25;

        maxRST = 38;
        currentRST = 20;
        RST = 20;

        maxNRG = 35;
        currentNRG = 18;
        NRG = 18;

        maxAUR = 30;
        currentAUR = 15;
        AUR = 15;

        maxCHI = 28;
        currentCHI = 14;
        CHI = 14;

        maxMNA = 32;
        currentMNA = 16;
        MNA = 16;
    }

    public void ReloadSlots(int value)
    {
        for (int i = 0; i < hotbarList.Count; i++)
        {
            for (int j = 0; j < hotbarList[i].Count; j++)
            {
                if (hotbarList[i][j].Value == value)
                {
                    hotbarList[i][j] = new ID(0);
                }
            }
        }

        for (int i = 0; i < gearList.Count; i++)
        {
            for (int j = 0; j < gearList[i].Count; j++)
            {
                if (gearList[i][j].Value == value)
                {
                    gearList[i].RemoveAt(j);
                }
            }
        }
    }

    public void UpdateGearPage()
    {
            // gearPages[gearPageIndex] içindeki ID'lerin Value değerlerini yazdır
        if (gearPages != null && gearPages.Count > gearPageIndex)
        {
            string values = string.Join(", ", gearPages[gearPageIndex].Select(id => id.Value));
            Debug.Log($"gearPages[{gearPageIndex}] ID Values: {values}");
        }

            // 1. Tüm gearList'teki ID'lerden inventory'deki ItemSlot'ları bul ve hepsini Unequip et
        foreach (var gearListInner in gearList)
        {
            foreach (var id in gearListInner.ToList()) // ToList ile silerken hata önlenir
            {
                ItemSlot slot = GetSlotById(id);
                if (slot != null)
                {
                    UnequipItem(slot);
                }
            }
        }

        // 2. gearPages[gearPageIndex] içindeki ID'lerden inventory'deki ItemSlot'ları bul ve hepsini Equip et
        if (gearPages != null && gearPages.Count > gearPageIndex)
        {
            foreach (var id in gearPages[gearPageIndex])
            {
                ItemSlot slot = GetSlotById(id);
                if (slot != null)
                {
                    EquipItem(slot);
                }
            }
        }
    }

    public void SelectSlot(SlotUI slotUI)
    {
        ID slotId = slotUI.slotId;
        string type = slotUI.type;
        int listIndex = slotUI.listIndex;
        int index = slotUI.index;

        void select()
        {
            selectedID = slotId;
            selectedType = type;
            selectedListIndex = listIndex;
            selectedIndex = index;
        }

        if (type == "utility")
        {
            if (UIManager.Instance.bottomMode == "gear")
            {
                if (selectedID == null)
                {
                    if (index != gearPageIndex)
                    {
                        if (gearPages != null && gearPages.Count > gearPageIndex)
                        {
                            gearPages[gearPageIndex].Clear();
                            foreach (var gearListInner in gearList)
                            {
                                foreach (var id in gearListInner)
                                {
                                    gearPages[gearPageIndex].Add(id);
                                }
                            }
                        }
                        gearPageIndex = index;
                        UpdateGearPage();
                    }
                }
                    else
                    {
                        if (selectedID.Value != 0)
                        {
                            Wallet.Instance.utilityList[UIManager.Instance.bottomMode][index] = GetSlotById(selectedID);
                            selectedID = null;
                        }
                        else { selectedID = null; }
                    }
                    
            }
            else if (UIManager.Instance.bottomMode == "hotbar")
            {
                switch (index)
                {
                    case 0:
                        UIManager.Instance.RemoveHotbarSlot();
                        break;
                    case 1:
                        UIManager.Instance.RemoveHotbar();
                        break;
                    case 2:
                        UIManager.Instance.AddHotbar();
                        break;
                    case 3:
                        UIManager.Instance.AddHotbarSlot();
                        break;
                    default:
                        Debug.LogError("Invalid hotbar index for utility selection.");
                        break;
                }
            }
        }
        else if (selectedID == null)
        {
            select();
        }
        else
        {
            if (selectedID.Value == slotId.Value && selectedType == type && selectedListIndex == listIndex && selectedIndex == index)
            {
                selectedID = null;
            }
            else if (selectedID.Value == 0 && slotId.Value == 0)
            {
                select();
            }
            else if (selectedID.Value == 0 && slotId.Value == 0)
            {
                select();
            }
            else if (selectedType == "inventory" && type == "inventory")
            {
                if (slotId == selectedID)
                {
                    selectedID = null;
                    UIManager.Instance.Refresh();
                }
                else
                {
                    select();
                }
            }
            else if (selectedType == "inventory" && type == "hotbar")
            {
                hotbarList[listIndex][index] = selectedID;
                selectedID = null;
            }
            else if (selectedType == "inventory" && type == "gear")
            {
                EquipItem(GetSlotById(selectedID));
                selectedID = null;
            }
            else if (selectedType == "hotbar" && type == "inventory")
            {
                hotbarList[selectedListIndex][selectedIndex] = new ID(0);
                selectedID = null;
            }
            else if (selectedType == "hotbar" && type == "hotbar")
            {
                hotbarList[selectedListIndex][selectedIndex] = slotId;
                hotbarList[listIndex][index] = selectedID;
                selectedID = null;
            }
            else if (selectedType == "hotbar" && type == "gear")
            {
                EquipItem(GetSlotById(selectedID));
                selectedID = null;
            }
            else if (selectedType == "gear" && type == "inventory")
            {
                gearList[selectedListIndex].RemoveAt(selectedIndex);
                selectedID = null;
            }
            else if (selectedType == "gear" && type == "hotbar")
            {
                hotbarList[listIndex][index] = selectedID;
                selectedID = null;
            }
            else if (selectedType == "gear" && type == "gear")
            {
                EquipItem(GetSlotById(slotId));
                selectedID = null;
            }
        }

        UIManager.Instance.Initialize();
    }

    public void SelectHotbar(int index)
    {
        selectedHotbarIndex = index;
        selectedHotbarSlotIndex = 0;
        UIManager.Instance.Initialize();
    }

    public ItemSlot GetSlotById(ID id)
    {
        if (id == null || id.Value == 0)
        {
            return null;
        }
        return inventory.FirstOrDefault(slot => slot.id.Value == id.Value);
    }

    public ItemSlot GetUtilitySlotById(ID id, string type)
    {
        if (id == null || id.Value == 0)
        {
            return null;
        }

        if (utilityList.ContainsKey(type))
        {
            return utilityList[type].FirstOrDefault(slot => slot.id.Value == id.Value);
        }
        else
        {
            Debug.LogError($"Utility type '{type}' not found.");
            return null;
        }
    }

    public void AddRandomItem()
    {
        List<string> itemIds = new List<string>
    {
        "apple",
        "sword",
        "red_pearl",
        "orange_pearl",
        "yellow_pearl",
        "green_pearl",
        "blue_pearl",
        "purple_pearl",
        "pink_pearl",
        "white_pearl",
        "black_pearl"
    };

        string randomItemId = itemIds[Random.Range(0, itemIds.Count)];

        AddItem(randomItemId);

        Debug.Log($"Random item added: {randomItemId}");
    }

    public void RemoveRandomSlot()
    {
        if (inventory == null || inventory.Count == 0)
        {
            Debug.LogError("Inventory is empty. No slot to remove.");
            return;
        }

        int randomIndex = Random.Range(0, inventory.Count);

        ItemSlot removedSlot = inventory[randomIndex];
        int removedValue = removedSlot.id.Value;
        inventory.RemoveAt(randomIndex);
        ReloadSlots(removedValue);

        if (removedSlot.item != null)
        {
            Debug.Log($"Removed slot with item: {removedSlot.item.id}, count: {removedSlot.count}");
        }
        else
        {
            Debug.Log("Removed an empty slot.");
        }

        OrganizeInventory();
        UIManager.Instance.Initialize();
    }


    public void AddItem(string itemId, int count = 1)
    {
        Item item = ItemManager.GetItem(itemId);
        if (item == null)
        {
            Debug.LogError($"Item with ID '{itemId}' not found.");
            return;
        }

        Debug.Log((inventory == null) ? "is null" : "is not null");
        foreach (ItemSlot slot in inventory)
        {
            if (slot.item != null && slot.item.id == item.id)
            {
                slot.count += count;
                Debug.Log($"Added {count}x {item.id} to existing slot. New count: {slot.count}");
                return;
            }
        }

        foreach (ItemSlot slot in inventory)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.count = count;
                Debug.Log($"Added {count}x {item.id} to a new slot.");
                return;
            }
        }

        ItemSlot newSlot = new ItemSlot { item = item, count = count };
        inventory.Add(newSlot);
        OrganizeInventory();
        if (selectedID != null && selectedType == "inventory")
        {
            selectedIndex = inventory.FindIndex(slot => slot.id.Value == selectedID.Value);
        }
        UIManager.Instance.Initialize();
    }

    public void EquipItem(ItemSlot itemSlot)
    {
        if (itemSlot == null)
        {
            return;
        }

        bool alreadyEquipped = gearList.Any(list => list.Any(id => id.Value == itemSlot.id.Value));
        if (alreadyEquipped)
        {
            return;
        }

        if (itemSlot.item.specificType.Contains("equipment"))
        {
            gearList[0].Insert(0, itemSlot.id);
        }
        else if (itemSlot.item.specificType.Contains("accesory"))
        {
            gearList[1].Insert(0, itemSlot.id);
        }
        else if (itemSlot.item.specificType.Contains("clothing"))
        {
            gearList[2].Insert(0, itemSlot.id);
        }
        else if (itemSlot.item.specificType.Contains("ark"))
        {
            gearList[3].Insert(0, itemSlot.id);
        }
        UIManager.Instance.Initialize();
    }

    public void UnequipItem(ItemSlot itemSlot)
    {
        if (itemSlot == null)
        {
            return;
        }

        for (int i = 0; i < gearList.Count; i++)
        {
            List<ID> gear = gearList[i];
            int index = gear.FindIndex(id => id.Value == itemSlot.id.Value);
            if (index != -1)
            {
                gear.RemoveAt(index);
                Debug.Log($"Unequipped item: {itemSlot.item.id} from gear slot {i}");
                UIManager.Instance.Initialize();
                return;
            }
        }

        Debug.LogWarning($"Item with ID {itemSlot.id.Value} is not equipped.");
    }

    public void OrganizeInventory()
    {
        inventory = inventory
            .Where(slot => slot.item != null)
            .OrderBy(slot => slot.item.id)
            .ToList();

        Debug.Log("Inventory organized: Items sorted alphabetically and empty slots removed.");
    }

    public void PrintInventory()
    {
        string inventoryString = "Inventory:\n";
        foreach (ItemSlot slot in inventory)
        {
            inventoryString += slot.ToString() + "\n";
        }
        Debug.Log(inventoryString);
    }
}
