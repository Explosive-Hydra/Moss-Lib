using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using BepInEx.Configuration;
using BepInEx.Logging;
using MossLib.Example;
using UnityEngine;

namespace MossLib;

public static class Tools
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public static void Alert(string text, bool important = false)
    {
        PlayerCamera.main.DoAlert(text, important);
    }
       
    // ReSharper disable once UnusedMember.Global
    public static void CheckForWorld()
    {
        if (!(bool) (UnityEngine.Object) PlayerCamera.main)
            throw new Exception(ModLocale.GetFormat("tools.checkforworld"));
    }

    // ReSharper disable once UnusedMember.Global
    public static void CheckArgumentCount(string[] args, int desired)
    {
        if (args.Length <= desired)
            throw new Exception(ModLocale.GetFormat("tools.checkargumentcount", desired, desired > 1 ? "s" : (object) "", args.Length - 1));
    }
    
    // ReSharper disable once UnusedMember.Global
    public static float ParseFloat(string s)
    {
        return !float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result) ? throw new Exception($"\"{s}\" is not a valid float value! (2, 0.7, 14.1, etc)") : result;
    }
    
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public static void LogToConsole(string text, ConsoleScript consoleScript)
    {
        consoleScript.logs.Add($"[<alpha=#55>{TimeSpan.FromSeconds(Time.realtimeSinceStartup):mm\\:ss}<alpha=#FF>] {text}");
        if (consoleScript.logs.Count > 100)
            consoleScript.logs.RemoveAt(0);
        if (!consoleScript.active)
            return;
        UpdateLogScreen(consoleScript);
    }

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public static void LogToConsoleAndLog(string text, ManualLogSource logger, ConsoleScript consoleScript)
    {
        LogToConsole(text, consoleScript);
        logger.LogInfo(text);
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogCla(string text, ManualLogSource logger, ConsoleScript consoleScript, bool important = false)
    {
        Alert(text, important);
        LogToConsole(text, consoleScript);
        logger.LogInfo(text);
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    public static void UpdateLogScreen(ConsoleScript consoleScript)
    {
        consoleScript.logText.text = string.Join("\n", consoleScript.logs);
    }
    
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMember.Global
    public static void ChangeConfig<T>(string guid, ConfigEntry<T> entry, object value)
    {
        var assembly = Assembly.GetCallingAssembly();
        var assemblyLocation = assembly.Location;
        
        if (string.IsNullOrEmpty(assemblyLocation))
            throw new Exception(ModLocale.GetFormat("tools.changeconfig.isnullorempty"));
            
        var configPath = Path.Combine(
            Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(assemblyLocation)!.FullName)!
                    .FullName)!
                .FullName,
            "config", $"{guid}.cfg");
        
        if (!File.Exists(configPath))
            throw new FileNotFoundException(ModLocale.GetFormat("tools.changeconfig.filenotfoundexception", configPath));
            
        entry.BoxedValue = value;
        entry.ConfigFile?.Save();
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void SwitchType(string guid, ConfigEntry<bool> configEntry, string configName, ManualLogSource logger)
    {
        ChangeConfig(guid, configEntry, !configEntry.Value);
        logger.LogInfo(ModLocale.GetFormat("tools.switchtype", configName, configEntry.Value));
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void SwitchType(string guid, ConfigEntry<bool> configEntry, string configName, ManualLogSource logger, ConsoleScript consoleScript)
    {
        ChangeConfig(guid, configEntry, !configEntry.Value);
        LogToConsoleAndLog(ModLocale.GetFormat("tools.switchtype", configName, configEntry.Value), logger, consoleScript);
    }
    
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMember.Global
    public static void SwitchType(string guid, ConfigEntry<bool> configEntry, string configName, ManualLogSource logger, ConsoleScript consoleScript, bool important)
    {
        ChangeConfig(guid, configEntry, !configEntry.Value);
        LogCla(ModLocale.GetFormat("tools.switchtype", configName, configEntry.Value), logger, consoleScript, important);
    }
}