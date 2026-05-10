using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MossLib.Example;
using UnityEngine;

namespace MossLib;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class Tools
{
    private static ConsoleScript _consoleScript;
    
    public static void Alert(string text, bool important = false)
    {
        PlayerCamera.main.DoAlert(text, important);
    }
       
    public static void CheckForWorld()
    {
        if (!(bool) (UnityEngine.Object) PlayerCamera.main)
            throw new Exception(ModLocale.GetFormat("tools.checkforworld"));
    }

    public static void CheckArgumentCount(string[] args, int desired)
    {
        if (args.Length <= desired)
            throw new Exception(ModLocale.GetFormat("tools.checkargumentcount", desired, desired > 1 ? "s" : (object) "", args.Length - 1));
    }
    
    public static float ParseFloat(string s)
    {
        return !float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result) ? throw new Exception($"\"{s}\" is not a valid float value! (2, 0.7, 14.1, etc)") : result;
    }
    
    public static void LogToConsole(string text)
    {
        _consoleScript.logs.Add($"[<alpha=#55>{TimeSpan.FromSeconds(Time.realtimeSinceStartup):mm\\:ss}<alpha=#FF>] {text}");
        if (_consoleScript.logs.Count > 100)
            _consoleScript.logs.RemoveAt(0);
        if (!_consoleScript.active)
            return;
        UpdateLogScreen(_consoleScript);
    }

    public static void LogInfo(string text, ManualLogSource logger)
    {
        LogToConsole(text);
        logger.LogInfo(text);
    }
    
    public static void LogError(string text, ManualLogSource logger)
    {
        LogToConsole($"[ERROR] {text}");
        logger.LogError(text);
    }
    
    public static void LogWarning(string text, ManualLogSource logger)
    {
        LogToConsole($"[WARNING] {text}");
        logger.LogWarning(text);
    }
    
    public static void LogCla(string text, ManualLogSource logger, bool important = false)
    {
        Alert(text, important);
        LogInfo(text, logger);
    }
    
    public static void UpdateLogScreen(ConsoleScript consoleScript)
    {
        consoleScript.logText.text = string.Join("\n", consoleScript.logs);
    }
    
    public static void ChangeConfig<T>(ConfigEntry<T> entry, object value)
    {
        entry.BoxedValue = value;
        entry.ConfigFile?.Save();
    }
    
    public static void SwitchType(ConfigEntry<bool> configEntry, string configName, ManualLogSource logger)
    {
        ChangeConfig(configEntry, !configEntry.Value);
        logger.LogInfo(ModLocale.GetFormat("tools.switchtype", configName, configEntry.Value));
    }
    
    public static void SwitchType(ConfigEntry<bool> configEntry, string configName, ManualLogSource logger, bool important)
    {
        ChangeConfig(configEntry, !configEntry.Value);
        LogCla(ModLocale.GetFormat("tools.switchtype", configName, configEntry.Value), logger, important);
    }
    
    [HarmonyPatch(typeof(ConsoleScript), "Awake")]
    public class ConsoleScriptAwakePatcher
    {
        [HarmonyPostfix]
        // ReSharper disable once InconsistentNaming
        public static void GetConsoleScript(ConsoleScript __instance)
        {
            _consoleScript = __instance;
        }
    }
    
    public static void SetBlock(int x, int y, ushort block)
    {
        Vector2 vector2 = new(x, y);
        SetBlock(vector2, block);
    }
    
    public static void SetBlock(Vector2 vector2, ushort block)
    {
        CheckForWorld();
        try
        {
            WorldGeneration.world.SetBlock(WorldGeneration.world.WorldToBlockPos(vector2), block);
        }
        catch (Exception ex)
        { 
            Error(ModLocale.GetFormat("tools.setblock", vector2, block, ex));
        }
    }
    
    public static void SetItem(int x, int y, string item)
    {
        Vector2 vector2 = new(x, y);
        SetItem(vector2, item);
    }

    public static void SetItem(Vector2 vector2, string item)
    {
        CheckForWorld();
        try
        {
            Utils.Create(item, vector2, 0.0f);
        }
        catch (Exception ex)
        { 
            Error(ModLocale.GetFormat("tools.setitem", vector2, item, ex));
        }
    }

    private static void Error(string text)
    {
        LogError(text, Plugin.Logger);
    }
}