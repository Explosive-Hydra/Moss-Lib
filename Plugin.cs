using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MossLib.Example;
using MossLib.Example.Lang;
using MossLib.Tool;

namespace MossLib;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "org.explosivehydra.mosslib";
    public const string Name = "Moss Lib";
    public const string Version = "1.1.2";
    private readonly Harmony _harmony = new(Guid);
    public new static ManualLogSource Logger;
    private static readonly Dictionary<string, ConfigEntryBase> Registry = new();
    private const string LocaleKeyPre = "mosslib.";
    
    public static ConfigEntry<string> Test;


    public void Awake()
    {
        Logger = base.Logger;
        
        LocaleGenerator.SetLogger(Logger);
        LocaleGenerator.Register(new EnLangGenerator(), Logger);
        LocaleGenerator.Register(new ZhCnLangGenerator(), Logger);
        LocaleGenerator.Register(new ZhTwLangGenerator(), Logger);    
        LocaleGenerator.GenerateAll();

        ModLocale.Initialize(Logger);
        _harmony.PatchAll();

        Test = RegisterConfig(Config, "Test", "test", "test");
        Locale("welcome");
    }
    
    private static ConfigEntry<T> RegisterConfig<T>(ConfigFile configFile, string section, string key, T defaultValue)
    {
        return MossLib.Tool.Config.Register(configFile, section, key, defaultValue,
            _ => Locale($"config.{section}.{key}.description"), Registry);
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat(key, args);
    }

    private new static void Info(string key, params object[] args)
    {
        var message = Locale($"log.{LocaleKeyPre}{key}", args);
        Log.Info(message, Logger);
    }

    private static void Error(string key, params object[] args)
    {
        var message = Locale($"log.{LocaleKeyPre}{key}", args);
        Log.Error(message, Logger);
    }

    private static void Warning(string key, params object[] args)
    {
        var message = Locale($"log.{LocaleKeyPre}{key}", args);
        Log.Warning(message, Logger);
    }
}