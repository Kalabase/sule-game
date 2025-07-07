using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionPanelUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject content;

    public bool isMouseOver = false;
    public float lifetime = 1f;


    public void Initialize(ItemSlot itemSlot)
    {
        foreach (string functionId in itemSlot.item.functions)
        {
            CreateButton(functionId, itemSlot);
        }
    }

    private void CreateButton(string functionId, ItemSlot itemSlot)
    {
        if (buttonPrefab != null)
        {
            GameObject button = Instantiate(buttonPrefab, content.transform);
            FunctionButtonUI buttonUI = button.GetComponent<FunctionButtonUI>();
            if (buttonUI != null)
            {
                buttonUI.Initialize(functionId, itemSlot);
            }
        }
    }
}
