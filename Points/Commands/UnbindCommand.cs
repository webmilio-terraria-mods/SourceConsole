using System;
using Microsoft.Xna.Framework.Input;
using SourceConsole.Binds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SourceConsole.Points.Commands;

public abstract class UnbindCommand : Command
{
    private const string UnbindUsage = "{0} <key>";

    protected UnbindCommand(string name, BindType bindType) : base(name, CommandLocation.Client)
    {
        BindType = bindType;
    }

    public override bool IsLoadingEnabled(Mod mod)
    {
        return Main.netMode != NetmodeID.Server;
    }

    public override void ClientAction(Player caller, string[] args)
    {
        if (!Enum.TryParse(args[0], true, out Keys key))
        {
            Error("You must specify a valid key to unbind.");
            return;
        }

        if (ModContent.GetInstance<BindsManager>().Remove(key, BindType))
        {
            Success($"Unbound {BindType} bind for {key}.");
        }
        else
        {
            Error($"No {BindType} bind for {key}.");
        }
    }

    public BindType BindType { get; }

    public override string Usage => string.Format(UnbindUsage, Command);
}

public class UnbindGlobalCommand : UnbindCommand
{
    public UnbindGlobalCommand() : base("unbind", BindType.Global)
    {
    }
}

/*public class UnbindWorldCommand : UnbindCommand
{
    public UnbindWorldCommand() : base("unbindworld", BindType.World)
    {
    }
}*/

public class UnbindPlayerCommand : UnbindCommand
{
    public UnbindPlayerCommand() : base("unbindplayer", BindType.Player)
    {
    }
}