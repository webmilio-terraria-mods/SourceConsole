using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.CVars;

public class DebugCVar : ConsolePoint
{
    public DebugCVar() : base("debug", CommandLocation.Client)
    {
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        // Replace me with important shit smileyface.
    }
}