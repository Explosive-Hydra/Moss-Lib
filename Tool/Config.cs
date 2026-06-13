using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BepInEx.Configuration;
using MossLib.Example;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class Config
{
    private const string LocaleKeyPre = "tool.config.";

    public static ConfigEntry<T> Register<T>(ConfigFile configFile, string section, string key, T defaultValue,
        Func<string, string> getLocale, Dictionary<string, ConfigEntryBase> registry)
    {
        var entry = configFile.Bind(section, key, defaultValue,
            getLocale($"config.{key}.description"));
        registry[key] = entry;
        return entry;
    }

    public static bool HasConfig(string config, Dictionary<string, ConfigEntryBase> registry)
    {
        return registry.ContainsKey(config);
    }

    public static ConfigEntryBase GetConfig(string config, Dictionary<string, ConfigEntryBase> registry)
    {
        var hasConfig = registry.TryGetValue(config, out var entry);
        if (hasConfig) return entry;

        Error("getconfig.notexistconfig", config);
        return null;
    }

    public static object GetConfigValue(string config, Dictionary<string, ConfigEntryBase> registry)
    {
        if (registry.TryGetValue(config, out var entry)) return entry.BoxedValue;

        Error("getconfig.notexistconfig", config);
        return null;
    }

    public static string GetConfigKey<T>(ConfigEntry<T> configEntry, Dictionary<string, ConfigEntryBase> registry)
    {
        var entry = registry.FirstOrDefault(x => x.Value == configEntry)
            .Key;

        if (!string.IsNullOrEmpty(entry))
            return entry;

        Error("getconfig.notexistkey", configEntry, entry);
        return null;
    }

    private static void Error(string key, params object[] args)
    {
        Log.Error(Locale(key, args), Plugin.Logger);
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat($"{LocaleKeyPre}{key}", args);
    }
}