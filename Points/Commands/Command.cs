namespace SourceConsole.Points.Commands;

public abstract class Command : ConsolePoint
{
    protected Command(string name, CommandLocation executionLocation) : base(name, executionLocation)
    {
    }

    protected override void PrintDescription()
    {
        if (!string.IsNullOrWhiteSpace(Usage))
        {
            Log($"Usage: {Usage}");
        }

        base.PrintDescription();
    }

    public abstract override string Usage { get; }
}