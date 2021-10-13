using System;
using Microsoft.Xna.Framework.Input;
using SourceConsole.Binds;
using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.Commands;

public abstract class BindCommand : Command
{
    private const string BindUsage = "{0} <key> <command>";

    protected BindCommand(string name, BindType bindType) : base(name, CommandLocation.Client)
    {
        BindType = bindType;

        ModContent.GetInstance<BindsManager>().Register(bindType, name);
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        if (args.Length < 2)
        {
            PrintNoArguments();
            return;
        }

        if (!Enum.TryParse(args[0], true, out Keys key))
        {
            Error($"{args[0]} is not a valid key to bind to.");
            return;
        }

        ModContent.GetInstance<BindsManager>().Set(key, BindType, string.Join(" ", args, 1, args.Length - 1));
    }

    public BindType BindType { get; }

    public override string Usage => string.Format(BindUsage, Command);
}

public class BindGlobalCommand : BindCommand
{
    public BindGlobalCommand() : base("bind", BindType.Global)
    {
    }
}

/*public class BindWorldCommand : BindCommand
{
    public BindWorldCommand() : base("bindworld", BindType.World)
    {
    }
}*/

public class BindPlayerCommand : BindCommand
{
    public BindPlayerCommand() : base("bindplayer", BindType.Player)
    {
    }
}