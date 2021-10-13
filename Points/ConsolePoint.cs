using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Commands;

namespace SourceConsole.Points;

public abstract class ConsolePoint : StandardCommand
{
    protected ConsolePoint(string name, CommandLocation executionLocation) : 
        base(name, executionLocation == CommandLocation.Server ? CommandType.World : CommandType.Chat)
    {
        ExecutionLocation = executionLocation;
    }

    protected void Log(string message) => Main.NewText(message);
    protected void Success(string message) => Main.NewText(message, Color.DarkGreen);
    protected void Error(string message) => Main.NewText(message, Color.DarkRed);

    protected virtual void PrintNoArguments()
    {
        PrintDescription();
    }

    protected virtual void PrintDescription()
    {
        if (!string.IsNullOrWhiteSpace(Description))
        {
            Log($"- {Description}");
        }
    }

    // TODO Implement Cheats check
    public virtual bool Cheat { get; }
    public virtual string Description { get; }

    public CommandLocation ExecutionLocation { get; }
}