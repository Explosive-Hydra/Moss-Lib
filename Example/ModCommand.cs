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
                Info("testhello.text", Locale("testhello.description")),
            null)
        );
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