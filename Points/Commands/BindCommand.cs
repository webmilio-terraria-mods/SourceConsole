using System;
using Microsoft.Xna.Framework.Input;
using SourceConsole.Binds;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SourceConsole.Points.Commands;

public abstract class BindCommand : Command
{
    private const string BindUsage = "{0} <key> <command>";
    private const string NoBindsFound = "No {0} binds for key {1}.";

    private BindsManager _binds;

    protected BindCommand(string name, BindType bindType) : base(name, CommandLocation.Client)
    {
        BindType = bindType;
    }

    public override bool IsLoadingEnabled(Mod mod)
    {
        return Main.netMode != NetmodeID.Server;
    }

    public override void Load()
    {
        _binds = ModContent.GetInstance<BindsManager>();
        _binds.Register(BindType, Command);
    }

    public override void ClientAction(Player caller, string[] args)
    {
        if (!Enum.TryParse(args[0], true, out Keys key))
        {
            Error($"{args[0]} is not a valid key to bind to.");
            return;
        }

        if (args.Length == 1)
        {
            if (!_binds.TryGet(key, out var bindGroup))
            {
                Log(string.Format(NoBindsFound, BindType, key));
                return;
            }

            var bind = bindGroup.Get(BindType);

            Log(string.IsNullOrWhiteSpace(bind) ?
                string.Format(NoBindsFound, BindType, key) :
                $"Bind for {key}: {bind}");
        }
        else if (args.Length >= 2)
        {
            var bindSentence = string.Join(" ", args, 1, args.Length - 1);

            _binds.Set(key, BindType, bindSentence);
            Success($"Created {BindType} bind for {key} : {bindSentence}");
        }
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