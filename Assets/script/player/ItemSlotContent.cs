using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotContent : MonoBehaviour
{
    public GameObject itemSlotUIPrefab;
    public Transform content;

    public int index;
    public string type;

    public List<List<ID>> idListList;
    public List<GameObject> slotObjList;
    public Image hotbarSelectionButton;
    public Sprite hotbarSelectionSprite;

    public void Initialize(List<List<ID>> idListList, string type, int index)
    {
        this.idListList = idListList;
        this.type = type;
        this.index = index;
        slotObjList = new List<GameObject>();
        InitializeSlots();
        if (type == "hotbar" && Wallet.Instance.selectedHotbarIndex == index)
        {
            hotbarSelectionButton.sprite = hotbarSelectionSprite;
        }
    }

    public void SelectHotbar()
    {
        Wallet.Instance.SelectHotbar(index);
    }

    public void InitializeSlots()
    {
        Debug.Log("OÇUM BEN AQ AMK AQ");
        ClearSlots();
        if (idListList[index].Count > 0)
        {
            for (int i = 0; i < idListList[index].Count; i++)
            {
                ID id = idListList[index][i];
                GameObject slotObj = Instantiate(itemSlotUIPrefab, content);
                slotObjList.Add(slotObj);
                SlotUI slotUI = slotObj.GetComponent<SlotUI>();
                slotUI.isc = this;
                slotUI.listIndex = index;
                slotUI.index = i;
                slotUI.type = type;
                slotUI.slotId = id;
                slotUI.Initialize();
            }
        }
    }

    public void ClearSlots()
    {
        if (slotObjList != null)
        {
            foreach (GameObject slotObj in slotObjList)
            {
                Destroy(slotObj);
            }
            slotObjList.Clear();
        }
    }

    public void AddSlot(int index, ID id)
    {
        // Yeni slotu oluştur
        GameObject slotObj = Instantiate(itemSlotUIPrefab, content);

        // SlotUI ayarlarını yap
        SlotUI slotUI = slotObj.GetComponent<SlotUI>();
        slotUI.isc = this;
        slotUI.listIndex = this.index;
        slotUI.index = index;
        slotUI.type = this.type;
        slotUI.slotId = id;
        slotUI.Initialize();

        // slotObjList'e doğru indexte ekle
        slotObjList.Insert(index, slotObj);

        // Hierarchy'de doğru sırada olmasını sağla
        slotObj.transform.SetSiblingIndex(index);
    }

    public void RemoveSlot(int index)
    {
        if (index < 0 || index >= slotObjList.Count)
        {
            Debug.LogError("Index out of range for slotObjList.");
            return;
        }

        GameObject slotObj = slotObjList[index];
        slotObjList.RemoveAt(index);
        Destroy(slotObj);
    }
}

