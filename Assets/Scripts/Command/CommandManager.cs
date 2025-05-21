using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    private List<ICommand> commands;

    public CommandManager()
    {
        commands = new List<ICommand>();
    }

    public void AddCommand(ICommand command)
    {
        commands.Add(command);
        command.Do();
        
    }

    public void UndoCommand()
    {
        if (commands.Count == 0) return;
        ICommand command = commands[^1];
        commands.RemoveAt(commands.Count - 1);
        command.Undo();
    }

    
}
