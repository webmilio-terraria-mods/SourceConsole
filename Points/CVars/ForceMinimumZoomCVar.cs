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

    public override bool ParseInput(string input, out int value) => int.TryParse(input, out value);

    public override string Description { get; } = "Enforce the minimum zoom level.";

    public override int Value
    {
        get => (bool)_zoomField.GetValue(null) ? 0 : Default;
        set => _zoomField.SetValue(null, value != Default);
    }

    public override int Default { get; } = 1;
}