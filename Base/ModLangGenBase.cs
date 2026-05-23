using System;
using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MossLib.Base;

public abstract class ModLangGenBase
{
    protected abstract string LanguageCode { get; }
    private Dictionary<string, string> LocaleData { get; } = new();
    private const string LangDirectory = "Lang";
    private bool _isInitialized;
    private System.Reflection.Assembly _ownerAssembly = System.Reflection.Assembly.GetCallingAssembly();
    private ManualLogSource _log;

    internal void Initialize(ManualLogSource logger, System.Reflection.Assembly pluginAssembly, Harmony harmonyInstance = null)
    {
        if (_isInitialized)
            return;

        _log = logger;
        _ownerAssembly = pluginAssembly;
        
        BuildLocaleData();
        _isInitialized = true;
    }

    protected abstract void BuildLocaleData();

    protected void Add(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            _log?.LogWarning($"[{LanguageCode}] Warning: Skipping empty key");
            return;
        }

        if (LocaleData.ContainsKey(key))
        {
            _log?.LogWarning($"[{LanguageCode}] Warning: Key '{key}' already exists and will be overwritten");
        }

        LocaleData[key] = value;
    }

    public void Generate(string outputDirectory = null)
    {
        EnsureInitialized();

        if (LocaleData == null || LocaleData.Count == 0)
        {
            _log?.LogWarning($"[{LanguageCode}] Warning: No localization data to generate");
            return;
        }

        if (string.IsNullOrEmpty(outputDirectory))
        {
            var pluginDirectory = Path.GetDirectoryName(_ownerAssembly.Location);
            outputDirectory = Path.Combine(pluginDirectory, LangDirectory);
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            var jsonObject = BuildNestedJson();
            var filePath = Path.Combine(outputDirectory, $"{LanguageCode}.json");
            var jsonContent = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                
            File.WriteAllText(filePath, jsonContent + Environment.NewLine);
            _log?.LogInfo($"[{LanguageCode}] ✓ Generated: {filePath} ({LocaleData.Count} entries)");
        }
        catch (Exception ex)
        {
            _log?.LogError($"[{LanguageCode}] ✗ Generation failed: {ex.Message}");
        }
    }

    private JObject BuildNestedJson()
    {
        var root = new JObject();

        foreach (var kvp in LocaleData)
        {
            SetNestedValue(root, kvp.Key, kvp.Value);
        }

        return root;
    }

    private static void SetNestedValue(JObject root, string path, string value)
    {
        var keys = path.Split('.');
        JObject current = root;

        for (int i = 0; i < keys.Length - 1; i++)
        {
            var key = keys[i];

            if (!current.ContainsKey(key) || current[key].Type != JTokenType.Object)
            {
                current[key] = new JObject();
            }

            current = (JObject)current[key];
        }

        current[keys[keys.Length - 1]] = value;
    }

    private void EnsureInitialized()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException($"{GetType().Name} has not been initialized. Call Initialize() first.");
        }
    }

    public int Count
    {
        get
        {
            EnsureInitialized();
            return LocaleData?.Count ?? 0;
        }
    }
}