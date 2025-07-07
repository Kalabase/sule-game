using TMPro;
using UnityEngine;

public class FunctionButtonUI : MonoBehaviour
{
    private string functionId;
    private ItemSlot itemSlot;

    public void Initialize(string functionId, ItemSlot itemSlot)
    {
        this.functionId = functionId;
        this.itemSlot = itemSlot;
        GetComponentInChildren<TMP_Text>().text = functionId;
    }

    public void OnButtonClick()
    {
        CommandExecutor.Execute(functionId, itemSlot);
    }
}
