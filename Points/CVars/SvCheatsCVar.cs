using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.CVars;

public class SvCheatsCVar : CVar<int>
{
    public SvCheatsCVar() : base("sv_cheats", CommandLocation.Server)
    {
    }

    public override void ServerAction(CommandCaller caller, Player playerCaller, string[] args)
    {
        var cheats = GetBoolean(args[0]);

        Value = cheats ? 1 : 0;
    }

    public override string Description { get; } = "Allows cheats on server.";

    public override int Value { get; set; }
    public override int Default { get; } = 0;
}