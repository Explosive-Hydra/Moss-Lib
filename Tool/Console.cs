using System.Diagnostics.CodeAnalysis;
using MossLib.Example;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class GameConsole
{
    public static readonly ConsoleScript Instance = ConsoleScript.instance;

    private const string LocaleKeyPre = "tool.console.";

    public static void RunCommand(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new System.ArgumentException(
                ModLocale.GetFormat($"{LocaleKeyPre}nullorempty"), nameof(key));

        if (Instance == null)
            throw new System.InvalidOperationException(
                ModLocale.GetFormat($"{LocaleKeyPre}notinitialized"));

        ConsoleScript.SearchExact(key).action(key.Split());
    }
}