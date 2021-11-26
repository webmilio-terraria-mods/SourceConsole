using On.Terraria.GameInput;
using Terraria;

namespace SourceConsole.Points.Commands.Inputs;

public abstract class Mouse1Command : Command
{
    protected Mouse1Command(bool state) : base((state ? '+' : '-') + "mouse1", CommandLocation.Client)
    {
        State = state;
    }

    public override void ClientAction(Player caller, string[] args)
    {
        Acting = State;
    }

    private static void TriggersPack_OnUpdate(TriggersPack.orig_Update orig, Terraria.GameInput.TriggersPack self)
    {
        orig(self);

        self.Current.MouseLeft = true;
    }

    public override string Usage { get; }
    public override int MinArgCount { get; } = 0;

    private static bool _acting;
    public static bool Acting
    {
        get => _acting;
        set
        {
            if (_acting == value)
                return;

            _acting = value;

            if (value)
                TriggersPack.Update += TriggersPack_OnUpdate;
            else
                TriggersPack.Update -= TriggersPack_OnUpdate;
        }
    }

    public bool State { get; }
}

public class PositiveMouse1Command : Mouse1Command
{
    public PositiveMouse1Command() : base(true)
    {
    }
}

public class NegativeMouse1Command : Mouse1Command
{
    public NegativeMouse1Command() : base(false)
    {
    }
}