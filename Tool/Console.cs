using System;
using MossLib.Example;

namespace MossLib.Tool;

public static class Console
{
    public static readonly ConsoleScript ConsoleScript = ConsoleScript.instance;
    private const string LocaleKeyPre = "tool.console.";

    public static void RunCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            throw new ArgumentException(ModLocale.GetFormat($"{LocaleKeyPre}nullorempty"), nameof(command));

        if (ConsoleScript == null)
            throw new InvalidOperationException(ModLocale.GetFormat($"{LocaleKeyPre}notinitialized"));

        ConsoleScript.instance.ExecuteCommand(command);
    }
}