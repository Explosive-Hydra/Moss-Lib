using MossLib.Example;

namespace MossLib.Tool;

public static class GameConsole
{
    public static readonly ConsoleScript Instance = ConsoleScript.instance;

    private const string LocaleKeyPre = "tool.console.";

    public static void RunCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            throw new System.ArgumentException(
                ModLocale.GetFormat($"{LocaleKeyPre}nullorempty"), nameof(command));

        if (Instance == null)
            throw new System.InvalidOperationException(
                ModLocale.GetFormat($"{LocaleKeyPre}notinitialized"));

        Instance.ExecuteCommand(command);
    }
}