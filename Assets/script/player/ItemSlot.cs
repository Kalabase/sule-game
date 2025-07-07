using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    public ID id { get; private set; } // Benzersiz ID
    public Item item;
    public int count;

    public ItemSlot()
    {
        id = new ID(); // Yeni bir benzersiz ID olu�tur
    }

    public override string ToString()
    {
        return $"ID: {id}, Item: {item?.id ?? "Empty"}, Count: {count}";
    }
}

