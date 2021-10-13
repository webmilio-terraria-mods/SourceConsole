using System.Collections.Generic;

namespace SourceConsole.Points.CVars;

public abstract class CVar<T> : ConsolePoint
{
    protected CVar(string name, CommandLocation executionLocation) : base(name, executionLocation)
    {
    }

    protected override void PrintNoArguments()
    {
        PrintValue();
        base.PrintNoArguments();
    }

    protected virtual void PrintValue()
    {
        Log($"\"{Command}\" = \"{Value}\" ( def. \"{Default}\" )");
    }

    public abstract T Value { get; set; }
    public abstract T Default { get; set; }
}