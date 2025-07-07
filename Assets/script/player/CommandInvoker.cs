using System.Collections.Generic;

public class CommandInvoker
{
    private Queue<ItemCommand> commandQueue = new Queue<ItemCommand>();

    public void AddCommand(ItemCommand command)
    {
        commandQueue.Enqueue(command);
    }

    public void ExecuteCommands()
    {
        while (commandQueue.Count > 0)
        {
            ItemCommand command = commandQueue.Dequeue();
            command.Execute();
        }
    }
}
