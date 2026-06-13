using System;
using System.Diagnostics.CodeAnalysis;
using MossLib.Example;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class GameConsole
{
    private const string LocaleKeyPre = "tool.console.";
    public static readonly ConsoleScript Instance = ConsoleScript.instance;

    public static void RunCommand(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException(
                ModLocale.GetFormat($"{LocaleKeyPre}nullorempty"), nameof(key));

        if (Instance == null)
            throw new InvalidOperationException(
                ModLocale.GetFormat($"{LocaleKeyPre}notinitialized"));

        ConsoleScript.SearchExact(key).action(key.Split());
    }
}