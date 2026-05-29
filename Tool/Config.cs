using BepInEx.Configuration;
using BepInEx.Logging;
using MossLib.Example;

namespace MossLib.Tool;

public static class Config
{
    private const string LogKeyPre = "tool.config.";

    public static void ChangeConfig<T>(ConfigEntry<T> entry, T value)
    {
        if (entry == null)
            throw new System.ArgumentNullException(nameof(entry));

        entry.Value = value;
        entry.ConfigFile?.Save();
    }

    public static void SwitchType(ConfigEntry<bool> configEntry, string configName)
    {
        if (configEntry == null)
            throw new System.ArgumentNullException(nameof(configEntry));

        ChangeConfig(configEntry, !configEntry.Value);
        Info($"{LogKeyPre}switchtype", configName, configEntry.Value);
    }

    public static void SwitchType(ConfigEntry<bool> configEntry, string configName,
        ManualLogSource logger, bool important)
    {
        if (configEntry == null)
            throw new System.ArgumentNullException(nameof(configEntry));

        ChangeConfig(configEntry, !configEntry.Value);
        Log.Alert(ModLocale.GetFormat($"{LogKeyPre}switchtype", configName, configEntry.Value),
            logger, important);
    }

    private static void Info(string key, params object[] args)
    {
        Log.Info(ModLocale.GetFormat(key, args), Plugin.Logger);
    }
}