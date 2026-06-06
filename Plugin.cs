using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MossLib.Example;
using MossLib.Tool;

namespace MossLib;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger;
    public const string Guid = "org.explosivehydra.mosslib";
    public const string Name = "Moss Lib";
    public const string Version = "1.1.1";
    private readonly Harmony _harmony = new(Guid);

    private const string LocaleKeyPre = "mosslib.";

    public void Awake()
    {
        Logger = base.Logger;

        ModLocale.Initialize(Logger);
        _harmony.PatchAll();
        
        LocaleGenerator.SetLogger(Logger);
        LocaleGenerator.InitializeDefaults();
        LocaleGenerator.GenerateAll();

        Locale("welcome");
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