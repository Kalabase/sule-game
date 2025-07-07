using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    private static List<Item> _allItems = new List<Item>();

    public void Initialize()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("items");
        if (jsonFile == null)
        {
            Debug.LogError("items dosyası bulunamadı.");
            return;
        }
        _allItems = JsonUtility.FromJson<ItemListWrapper>(jsonFile.text).items;

        foreach (Item item in _allItems)
        {
            if (item.functions == null)
                item.functions = new List<string>();

            if (item.specificType != null || item.generalType != null)
            {
                if (item.generalType.Contains("gear") && !item.functions.Contains("equip"))
                    item.functions.Add("equip");
                if (item.generalType.Contains("gear") && !item.functions.Contains("unequip"))
                    item.functions.Add("unequip");
            }

            if (!item.functions.Contains("select"))
                item.functions.Add("select");
            }
    }



    public static Item GetItem(string itemID)
    {
        foreach (Item item in _allItems)
        {
            if (item.id == itemID) return item;
        }
        Debug.LogError("Eşya bulunamadı: " + itemID);
        return null;
    }

    [System.Serializable]
    private class ItemListWrapper
    {
        public List<Item> items;
    }
}