using UnityEngine;
public class UnequipCommand : ItemCommand
{
    public UnequipCommand(ItemSlot itemSlot) : base(itemSlot) { }

    public override void Execute()
    {
        Wallet.Instance.UnequipItem(itemSlot);
    }
}