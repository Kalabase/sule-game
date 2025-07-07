using System.Collections.Generic;
using UnityEngine;

public class ScrollViewContent : MonoBehaviour
{
    public GameObject scrollViewPrefab;
    public Transform content;
    public string type;
    public List<List<ID>> idListList;
    public List<GameObject> scrollViewList;
    
    public void Initialize()
    {
        if (type == "hotbar")
        {
            idListList = Wallet.Instance.hotbarList;
        }
        else if (type == "inventory")
        {
            if (Wallet.Instance.inventory.Count > 0)
            {
                idListList = new List<List<ID>> { Wallet.Instance.inventory.ConvertAll(slot => slot.id) };
            }
            else
            {
                idListList = new List<List<ID>> { new List<ID>() };
            }
        }
        else if (type == "utility")
        {
            idListList = new List<List<ID>> { Wallet.Instance.utilityList[UIManager.Instance.bottomMode].ConvertAll(slot => slot.id) };
        }
        else if (type == "gear")
        {
            idListList = Wallet.Instance.gearList;
        }
        else if (type == "dock")
        {
            idListList = new List<List<ID>> { Wallet.Instance.hotbarList[Wallet.Instance.selectedHotbarIndex] };
        }
        else
        {
            idListList ??= new List<List<ID>> { new List<ID>() };
            Debug.LogError("idListList error!");
            return;
        }

        ClearScrollViews();

        scrollViewList = new List<GameObject>();

        PopulateScrollViews(idListList);
    }

    public void ClearScrollViews()
    {
        if (scrollViewList != null)
        {
            foreach (GameObject scrollView in scrollViewList)
            {
                Destroy(scrollView);
            }
            scrollViewList.Clear();
        }
    }

    public void PopulateScrollViews(List<List<ID>> idListList)
    {
        for (int i = 0; i < idListList.Count; i++)
            {
                GameObject scrollView = Instantiate(scrollViewPrefab, content);
                scrollViewList.Add(scrollView);

                // ScrollViewInfo script'ini al
                ScrollViewInfo scrollViewInfo = scrollView.GetComponent<ScrollViewInfo>();
                if (scrollViewInfo != null)
                {
                    // ItemSlotContent script'ini al
                    ItemSlotContent itemSlotContent = scrollViewInfo.itemSlotContent;
                    if (itemSlotContent != null)
                    {
                        // slotListList'teki ilgili slotList'i ayarla
                        itemSlotContent.index = i;
                        itemSlotContent.Initialize(idListList[i], type);
                    }
                    else
                    {
                        Debug.LogError("ItemSlotContent referansı atanmadı.");
                    }
                }
                else
                {
                    Debug.LogError("ScrollViewInfo script'i bulunamadı.");
                }
            }
    }
}