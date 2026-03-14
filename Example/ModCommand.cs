using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using MossLib.Base;
using UnityEngine;

namespace MossLib.Example;

public class ModCommand : ModCommandBase
{
    private static ModCommand Instance { get; set; } = new();
    
    private static ModCommand _instance;
    private const string LocalePre = "command.mosslib.";
    private static ManualLogSource _logger;
    
    public static void Initialize(ManualLogSource logger)
    { 
        if (_instance != null) return;
        _instance = new ModCommand();
        Instance = _instance;
        _logger = logger;
        _instance.Initialize(logger, Plugin.Guid, Plugin.Name, Assembly.GetExecutingAssembly());
    }
    
    [HarmonyPatch(typeof(ConsoleScript), "RegisterAllCommands")]
    public class ConsoleScriptRegisterAllCommandsPatcher
    {
        [HarmonyPostfix]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void RegisterCustomCommands(ConsoleScript __instance)
        {
            ConsoleScript.Commands.Add(new Command(
                "testhello", 
                ModLocale.GetFormat(LocalePre + "testhello.description"),
                _ =>
                {
                    Tools.LogCla(ModLocale.GetFormat(LocalePre + "testhello.text", LocalePre), _logger, __instance);
                },
                null
            ));
            ConsoleScript.Commands.Add(new Command(
                    "undeadmode", 
                    ModLocale.GetFormat(LocalePre + "undeadmode.description"),
                    _ =>
                    {
                        Tools.CheckForWorld();
                        Tools.SwitchType(Plugin.Guid, Plugin.UndeadMode, ModLocale.GetFormat(LocalePre + "undeadmode.name"), _logger, __instance);
                        UndeadMode.UndeadModeConfigs.Update();
                    }, null
                )
            );
        }
    }

    [HarmonyPatch(typeof(ConsoleScript), "Awake")]
    public new class ConsoleScriptAwakePatcher
    {
        [HarmonyPostfix]
        // ReSharper disable once UnusedMember.Global
        public static void AddCustomLogCallback()
        {
            Application.logMessageReceived += Instance.ApplicationLogCallback;
        }
    }
}