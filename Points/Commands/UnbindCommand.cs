using System;
using Microsoft.Xna.Framework.Input;
using SourceConsole.Binds;
using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.Commands;

public abstract class UnbindCommand : Command
{
    private const string UnbindUsage = "{0} <key>";

    protected UnbindCommand(string name, BindType bindType) : base(name, CommandLocation.Client)
    {
        BindType = bindType;
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        if (args.Length == 0)
        {
            PrintNoArguments();
            return;
        }

        if (Enum.TryParse(args[0], out Keys key))
        {
            Error("You must specify a valid key to unbind.");
            return;
        }

        ModContent.GetInstance<BindsManager>().Remove(key, BindType);
    }

    public BindType BindType { get; }

    public override string Usage => string.Format(UnbindUsage, Command);
}

public class UnbindGlobalCommand : UnbindCommand
{
    public UnbindGlobalCommand(string name) : base(name, BindType.Global)
    {
    }
}

/*public class UnbindWorldCommand : UnbindCommand
{
    public UnbindWorldCommand(string name) : base(name, BindType.World)
    {
    }
}*/

public class UnbindPlayerCommand : UnbindCommand
{
    public UnbindPlayerCommand(string name) : base(name, BindType.Player)
    {
    }
}