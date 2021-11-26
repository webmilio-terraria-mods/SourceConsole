using Microsoft.Xna.Framework;
using SourceConsole.Points.Commands;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WebmilioCommons.Commands;
using WebmilioCommons.Extensions;

namespace SourceConsole.Points;

public abstract class ConsolePoint : StandardCommand
{
    protected ConsolePoint(string name, CommandLocation executionLocation) : 
        base(name, executionLocation == CommandLocation.Server ? CommandType.World : CommandType.Chat)
    {
        ExecutionLocation = executionLocation;
    }

    protected bool GetBoolean(string str)
    {
        return sbyte.TryParse(str, out var i) && i == 1 ||
               bool.TryParse(str, out var b) && b;
    }

    protected override void Action(CommandCaller caller, Player player, string input, string[] args)
    {
        if (args.Length == 0 && MinArgCount > 0)
        {
            PrintNoArguments();
            return;
        }

        if (!CanExecute(caller, player, input, args))
        {
            return;
        }

        if (ExecutionLocation.HasFlag(CommandLocation.Client) && Main.netMode != NetmodeID.Server)
        {
            player.DoIfLocal(p => CommonAction(p, args));
            player.DoIfLocal(p => ClientAction(p, args));
        }
        
        if (ExecutionLocation.HasFlag(CommandLocation.Server))
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Error($"Can't change replicated ConVar {Command} from console of client, only server operator can change its value.");
                return;
            }

            CommonAction(player, args);
            ServerAction(caller, player, args);
        }
    }

    public virtual bool CanExecute(CommandCaller caller, Player player, string input, string[] args) => true;

    public virtual void CommonAction(Player caller, string[] args) { }
    public virtual void ClientAction(Player caller, string[] args) { }
    public virtual void ServerAction(CommandCaller caller, Player playerCaller, string[] args) { }

    protected static void Log(object message) => Main.NewText(message);
    protected static void Success(object message) => Main.NewText(message, Color.DarkGreen);
    protected static void Error(object message) => Main.NewText(message, Color.DarkRed);

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

    public virtual int MinArgCount { get; } = 1;

    // TODO Implement Cheats check
    public virtual bool Cheat { get; }
    public virtual string Description { get; }

    public CommandLocation ExecutionLocation { get; }
}