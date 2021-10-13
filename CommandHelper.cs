using System;
using System.Reflection;
using Terraria.ModLoader;

namespace SourceConsole;

public class CommandHelper : ModSystem
{
    private ConstructorInfo _commandHandler;
    private MethodInfo _handleMethod;

    public override void Load()
    {
        {
            var a = typeof(ModCommand).Assembly;
            var t = a.GetType("Terraria.ModLoader.ChatCommandCaller");

            // ReSharper disable once PossibleNullReferenceException
            _commandHandler = t.GetConstructor(Type.EmptyTypes);
        }

        _handleMethod = typeof(CommandLoader).GetMethod("HandleCommand", BindingFlags.Static | BindingFlags.NonPublic);
    }

    public bool ExecuteCommand(string command)
    {
        // ReSharper disable once PossibleNullReferenceException
        return (bool) _handleMethod.Invoke(null, new[]
        {
            command,
            _commandHandler.Invoke(Array.Empty<object>())
        });
    }
}