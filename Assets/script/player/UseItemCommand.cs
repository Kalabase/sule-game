using UnityEngine;

public abstract class ItemCommand
{
    protected ItemSlot itemSlot;

    public ItemCommand(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
    }

    public abstract void Execute();
}

public class UseItemCommand : ItemCommand
{
    public UseItemCommand(ItemSlot itemSlot) : base(itemSlot) { }

    public override void Execute()
    {
        if (itemSlot.item != null && itemSlot.count > 0)
        {
            Debug.Log($"Using item: {itemSlot.item.id}");
        }
    }
}
