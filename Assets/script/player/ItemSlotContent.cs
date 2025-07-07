using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotContent : MonoBehaviour
{
    public GameObject itemSlotUIPrefab;
    public Transform content;

    public int index;

    public List<ID> idList;
    public List<GameObject> slotObjList;
    public Image hotbarSelectionButton;
    public Sprite hotbarSelectionSprite;

    public void Initialize(List<ID> ids, string type)
    {
        idList = ids;
        slotObjList = new List<GameObject>();
        InitializeSlots(type);
        if (type == "hotbar" && Wallet.Instance.selectedHotbarIndex == index)
        {
            hotbarSelectionButton.sprite = hotbarSelectionSprite;
        }
    }

    public void SelectHotbar()
    {
        Wallet.Instance.SelectHotbar(index);
    }


    public void InitializeSlots(string type)
    {
        if (idList.Count > 0)
        {
            for (int i = 0; i < idList.Count; i++)
            {
                ID id = idList[i];
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
}

