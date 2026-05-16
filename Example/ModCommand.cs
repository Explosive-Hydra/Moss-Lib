using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using MossLib.Base;
using MossLib.Tool;
using UnityEngine;
using Console = MossLib.Tool.Console;

namespace MossLib.Example;

[HarmonyPatch(typeof(ConsoleScript))]
public class ModCommand : ModCommandBase
{
    private const string LocaleKeyPre = "command.";
    private static ManualLogSource _logger;

    private static ModCommand Instance { get; set; } = new();

    public static void Initialize(ManualLogSource logger)
    {
        if (Instance != null)
            return;
        Instance = new ModCommand();
        _logger = logger;
        Instance.Initialize(logger, Assembly.GetExecutingAssembly());
    }
    
    [HarmonyPatch("RegisterAllCommands")]
    [HarmonyPostfix]
    public static void RegisterCustomCommands()
    {
        ConsoleScript.Commands.Add(new Command(
            "testhello",
            ModLocale.GetFormat($"{LocaleKeyPre}testhello.description"), _ => Log.Cla(
                ModLocale.GetFormat($"{LocaleKeyPre}testhello.text", $"{LocaleKeyPre}testhello.description"),
                _logger, (bool) (UnityEngine.Object) Console.ConsoleScript), 
            null)
        );
    }
    

    [HarmonyPatch("Awake")] 
    [HarmonyPostfix] 
    public static void AddCustomLogCallback()
    {
        Application.logMessageReceived += Instance.ApplicationLogCallback;
    }
}

