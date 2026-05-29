using BepInEx.Logging;
using HarmonyLib;
using MossLib.Base;
using MossLib.Tool;

namespace MossLib.Example;

[HarmonyPatch(typeof(ConsoleScript))]
public class ModCommand : ModCommandBase
{
    private new static readonly ManualLogSource Logger = Plugin.Logger;

    [HarmonyPatch("RegisterAllCommands")]
    [HarmonyPostfix]
    public static void RegisterCustomCommands(ConsoleScript __instance)
    {
        ConsoleScript.Commands.Add(new Command(
            "testhello",
            Locale("testhello.description"), _ =>
                Info("testhello.text",
                    TestHello()
                ),
            null)
        );
    }

    private static string TestHello()
    {
        var text = Locale("testhello.description");
        var result = "";
        for (int i = 0; i < text.Length; i++)
        {
            result += Text.Size(text[i].ToString(), (i + 3) * 9);
        }

        return result;
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat($"command.{key}", args);
    }

    private static void Info(string key, params object[] args)
    {
        var message = Locale(key, args);
        Log.Info(message, Logger);
    }
}