using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.CVars;

public class ZoomCVar : CVar<float>
{
    public ZoomCVar() : base("zoom", CommandLocation.Client)
    {
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        if (args.Length != 1)
        {
            PrintNoArguments();
            return;
        }

        if (!float.TryParse(args[0], out var zoom))
        {
            Error("The zoom level must be a floating-point value.");
            return;
        }

        Value = zoom;
    }

    public override string Description { get; } = "The client's zoom level.";

    public override float Value
    {
        get => Main.GameZoomTarget;
        set => Main.GameZoomTarget = value;
    }

    public override float Default { get; } = 1f;
}