public static class CommandExecutor
{
    public static void Execute(string functionId, ItemSlot itemSlot)
    {
        ItemCommand command = CreateCommand(functionId, itemSlot);
        if (command != null)
        {
            CommandInvoker invoker = new CommandInvoker();
            invoker.AddCommand(command);
            invoker.ExecuteCommands();
        }
    }

    public static ItemCommand CreateCommand(string functionId, ItemSlot itemSlot)
    {
        switch (functionId)
        {
            case "consume":
                return new ConsumeCommand(itemSlot);
            case "equip":
                return new EquipCommand(itemSlot);
            case "unequip":
                return new UnequipCommand(itemSlot);
            default:
                return null;
        }
    }
}