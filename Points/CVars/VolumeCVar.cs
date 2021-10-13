using Terraria;
using Terraria.ModLoader;

namespace SourceConsole.Points.CVars;

public abstract class VolumeCVar : CVar<float>
{
    protected VolumeCVar(string name) : base(name, CommandLocation.Client)
    {
    }

    protected override void ActionLocal(CommandCaller caller, Player player, string input, string[] args)
    {
        if (args.Length != 1)
        {
            PrintNoArguments();
            return;
        }

        if (!float.TryParse(args[0], out var volume))
        {
            Error("The volume must be a float value.");
            return;
        }

        Value = volume;
    }

    public override float Default { get; } = 1;
}

public class MusicVolumeCVar : VolumeCVar
{
    public MusicVolumeCVar() : base("musicvol")
    {
    }

    public override float Value
    {
        get => Main.musicVolume;
        set => Main.musicVolume = value;
    }

    public override string Description { get; } = "Music volume.";
}

public class SoundVolumeCVar : VolumeCVar
{
    public SoundVolumeCVar() : base("soundvol")
    {
    }

    public override float Value
    {
        get => Main.soundVolume;
        set => Main.soundVolume = value;
    }

    public override string Description { get; } = "Sound volume.";
}

public class AmbientVolumeCVar : VolumeCVar
{
    public AmbientVolumeCVar() : base("ambientvol")
    {
    }

    public override float Value
    {
        get => Main.ambientVolume;
        set => Main.ambientVolume = value;
    }

    public override string Description { get; } = "Ambient volume.";
}