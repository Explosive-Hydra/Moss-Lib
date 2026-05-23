using System.Collections.Generic;
using System.Reflection;
using BepInEx.Logging;
using MossLib.Base;
using MossLib.Tool;

namespace MossLib.Example;

public class ModLocale : ModLocaleBase
{
    private static ModLocale _instance;
    private static ManualLogSource _logger;

    public static void Initialize(ManualLogSource logger)
    {
        if (_instance != null)
            return;
        _logger = logger;
        _instance = new ModLocale();
        _instance.Initialize(logger, Assembly.GetExecutingAssembly());
    }

    public static string GetFormat(string key, params object[] args)
    {
        if (_instance == null)
        {
            return $"[{key}]";
        }

        try
        {
            var result = _instance.GetStringFormatted(key, args);
            return string.IsNullOrEmpty(result) ? $"[{key}]" : result;
        }
        catch (System.Exception ex)
        {
            Warning($"Failed to get formatted string for key: {key}, error: {ex.Message}");
            return $"[{key}]";
        }
    }

    public static string Get(string key)
    {
        if (_instance == null)
        {
            return $"[{key}]";
        }

        try
        {
            var result = _instance.GetString(key);
            return string.IsNullOrEmpty(result) ? $"[{key}]" : result;
        }
        catch (System.Exception ex)
        {
            Warning($"Failed to get string for key: {key}, error: {ex.Message}");
            return $"[{key}]";
        }
    }

    public static bool ContainsKey(string key)
    {
        return _instance != null && _instance.HasKey(key);
    }

    public static string GetOnDictionary(string dictionary, string key)
    {
        if (_instance == null)
        {
            return $"[{key}]";
        }

        try
        {
            var result = _instance.GetStringOnDictionary(dictionary, key);
            return string.IsNullOrEmpty(result) ? $"[{key}]" : result;
        }
        catch (System.Exception ex)
        {
            Warning($"Failed to get string from dictionary: {dictionary}, key: {key}, error: {ex.Message}");
            return $"[{key}]";
        }
    }

    public static string[] GetArray(string key)
    {
        if (_instance == null)
        {
            return [$"[{key}]"];
        }

        try
        {
            var result = _instance.GetStringArray(key);
            return result == null || result.Length == 0 ? [$"[{key}]"] : result;
        }
        catch (System.Exception ex)
        {
            Warning($"Failed to get string array for key: {key}, error: {ex.Message}");
            return [$"[{key}]"];
        }
    }

    public static Dictionary<string, string> GetDictionary(string key)
    {
        if (_instance == null)
        {
            return new Dictionary<string, string>();
        }

        try
        {
            var result = _instance.GetStringDictionary(key);
            return result ?? new Dictionary<string, string>();
        }
        catch (System.Exception ex)
        {
            Warning($"Failed to get string dictionary for key: {key}, error: {ex.Message}");
            return new Dictionary<string, string>();
        }
    }

    private static void Warning(string text)
    {
        Log.Warning(text, _logger);
    }
}