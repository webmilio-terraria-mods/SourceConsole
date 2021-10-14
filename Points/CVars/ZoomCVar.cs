using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.CVars;

public class ZoomCVar : CVar<float>
{
    public ZoomCVar() : base("zoom", CommandLocation.Client)
    {
    }

    public override string Description { get; } = "The client's zoom level.";

    public override float Value
    {
        get => Main.GameZoomTarget;
        set => Main.GameZoomTarget = value;
    }

    public override float Default { get; } = 1f;
}