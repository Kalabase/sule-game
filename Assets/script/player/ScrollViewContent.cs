using System.Collections.Generic;
using UnityEngine;

public class ScrollViewContent : MonoBehaviour
{
    public GameObject scrollViewPrefab;
    public Transform content;
    public string type;
    public List<List<ID>> idListList;
    public List<GameObject> scrollViewList;
    public List<ItemSlotContent> itemSlotContents;
    
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

    public void PopulateScrollViews(List<List<ID>> idListList)
    {
        ClearScrollViews();
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
                    itemSlotContents.Add(itemSlotContent);
                    itemSlotContent.index = i;
                    itemSlotContent.Initialize(idListList, type, i);
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
        itemSlotContents.Clear();
    }

    public void AddScrollView(int index, List<ID> idList)
    {
        GameObject scrollView = Instantiate(scrollViewPrefab, content);

        // scrollViewList'e belirtilen indexte ekle
        scrollViewList.Insert(index, scrollView);

        // Hierarchy'de doğru sırada olmasını sağla
        scrollView.transform.SetSiblingIndex(index);

        ScrollViewInfo scrollViewInfo = scrollView.GetComponent<ScrollViewInfo>();
        if (scrollViewInfo != null)
        {
            ItemSlotContent itemSlotContent = scrollViewInfo.itemSlotContent;
            if (itemSlotContent != null)
            {
                itemSlotContents.Insert(index, itemSlotContent);
                itemSlotContent.index = index;
                itemSlotContent.Initialize(idListList, type, index);
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

    public void RemoveScrollView(int index)
    {
        // ScrollView objesini ve ItemSlotContent'i sil
        if (scrollViewList != null && index >= 0 && index < scrollViewList.Count)
        {
            Destroy(scrollViewList[index]);
            scrollViewList.RemoveAt(index);
        }

        if (itemSlotContents != null && index >= 0 && index < itemSlotContents.Count)
        {
            itemSlotContents.RemoveAt(index);
        }
    }
}