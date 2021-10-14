using System;
using System.Collections.Generic;
using Terraria;

namespace SourceConsole.Points.CVars;

public abstract class CVar<T> : ConsolePoint
{
    protected CVar(string name, CommandLocation executionLocation) : base(name, executionLocation)
    {
    }

    public override void CommonAction(Player caller, string[] args)
    {
        if (!ParseInput(args[0], out var value))
        {
            Error($"Value must of type {nameof(T)}.");
            return;
        }

        Value = value;
    }

    public virtual bool ParseInput(string input, out T value) => throw new NotImplementedException($"You must override this method if you aren't overriding {nameof(ClientAction)}.");

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
    public abstract T Default { get; }
}