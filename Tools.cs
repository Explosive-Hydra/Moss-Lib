using System;
using System.Globalization;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MossLib.Example;
using UnityEngine;

namespace MossLib;

public static class Tools
{
    private static ConsoleScript _consoleScript;
    
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
    public static void LogToConsole(string text)
    {
        _consoleScript.logs.Add($"[<alpha=#55>{TimeSpan.FromSeconds(Time.realtimeSinceStartup):mm\\:ss}<alpha=#FF>] {text}");
        if (_consoleScript.logs.Count > 100)
            _consoleScript.logs.RemoveAt(0);
        if (!_consoleScript.active)
            return;
        UpdateLogScreen(_consoleScript);
    }

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public static void LogToConsoleAndLog(string text, ManualLogSource logger)
    {
        LogToConsole(text);
        logger.LogInfo(text);
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogCla(string text, ManualLogSource logger, bool important = false)
    {
        Alert(text, important);
        LogToConsoleAndLog(text, logger);
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
    // ReSharper disable once UnusedMember.Global
    public static void SwitchType(string guid, ConfigEntry<bool> configEntry, string configName, ManualLogSource logger, bool important)
    {
        ChangeConfig(guid, configEntry, !configEntry.Value);
        LogCla(ModLocale.GetFormat("tools.switchtype", configName, configEntry.Value), logger, important);
    }
    
    [HarmonyPatch(typeof(ConsoleScript), "Awake")]
    public class ConsoleScriptAwakePatcher
    {
        [HarmonyPostfix]
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static void GetConsoleScript(ConsoleScript __instance)
        {
            _consoleScript = __instance;
        }
    }
    
    public static bool IsMultiplayerActive()
    {
        try
        {
            var mpAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "KrokoshaCasualtiesMP");
                
            if (mpAssembly != null)
            {
                var mpType = mpAssembly.GetType("KrokoshaCasualtiesMP.KrokoshaScavMultiplayer");
                if (mpType != null)
                {
                    var networkSystemField = mpType.GetField("network_system_is_running", 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (networkSystemField != null)
                    {
                        var isRunning = networkSystemField.GetValue(null) as bool?;
                        return isRunning == true;
                    }
                }
            }
        }
        catch
        {
        }

        return false;
    }
}