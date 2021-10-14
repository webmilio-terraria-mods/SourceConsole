using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.CVars;

public class ForceMinimumZoomCVar : CVar<int>
{
    private const string CommandName = "forceminimumzoom";
    private readonly FieldInfo _zoomField;


    public ForceMinimumZoomCVar() : base(CommandName, CommandLocation.Client)
    {
        _zoomField = typeof(ModLoader).GetField("removeForcedMinimumZoom", BindingFlags.NonPublic | BindingFlags.Static);
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        if (!int.TryParse(args[0], out var zoom) || zoom is < 0 or > 1)
        {
            Error("Value must be an integer between 0 and 1.");
            return;
        }

        Value = zoom;
    }

    public override string Description { get; } = "Enforce the minimum zoom level.";

    public override int Value
    {
        get => (bool)_zoomField.GetValue(null) ? 0 : Default;
        set => _zoomField.SetValue(null, value != Default);
    }

    public override int Default { get; } = 1;
}