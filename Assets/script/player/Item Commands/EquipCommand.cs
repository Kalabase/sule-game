using UnityEngine;
public class EquipCommand : ItemCommand
{
    public EquipCommand(ItemSlot itemSlot) : base(itemSlot) { }

    public override void Execute()
    {
        Wallet.Instance.EquipItem(itemSlot);
    }
}