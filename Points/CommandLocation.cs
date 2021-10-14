using System;

namespace SourceConsole.Points;

[Flags]
public enum CommandLocation : byte
{
    Client = 1,
    Server = Client << 1
}