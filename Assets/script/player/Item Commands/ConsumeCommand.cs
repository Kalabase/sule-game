using UnityEngine;
public class ConsumeCommand : ItemCommand
{
    public ConsumeCommand(ItemSlot itemSlot) : base(itemSlot) { }

    public override void Execute()
    {
        // itemSlot.item.id ile hangi meyve olduğu bilgisi burada kullanılabilir
        Debug.Log("Consumed: " + itemSlot.item.id);
        // Tüketme işlemleri...
    }
}