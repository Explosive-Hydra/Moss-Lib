using System;
using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MossLib.Tool;

namespace MossLib.Base;

public abstract class ModLangGenBase
{
    protected abstract string LanguageCode { get; }
    private Dictionary<string, string> LocaleData { get; } = new();
    private const string LangDirectory = "Lang";
    private static readonly ManualLogSource Logger = Plugin.Logger;
    private bool _isInitialized;
    private readonly System.Reflection.Assembly _ownerAssembly;

    protected ModLangGenBase()
    {
        _ownerAssembly = System.Reflection.Assembly.GetCallingAssembly();
        Initialize();
    }

    private void Initialize()
    {
        if (_isInitialized)
            return;

        BuildLocaleData();
        _isInitialized = true;
    }

    protected abstract void BuildLocaleData();

    protected void Add(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            Log.Warning($"[{LanguageCode}] Warning: Skipping empty key", Logger);
            return;
        }

        if (LocaleData.ContainsKey(key))
        {
            Log.Warning($"[{LanguageCode}] Warning: Key '{key}' already exists and will be overwritten", Logger);
        }

        LocaleData[key] = value;
    }

    public void Generate(string outputDirectory = null)
    {
        EnsureInitialized();

        if (LocaleData == null || LocaleData.Count == 0)
        {
            Log.Warning($"[{LanguageCode}] Warning: No localization data to generate", Logger);
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
            Log.Info($"[{LanguageCode}] ✓ Generated: {filePath} ({LocaleData.Count} entries)", Logger);
        }
        catch (Exception ex)
        {
            Log.Error($"[{LanguageCode}] ✗ Generation failed: {ex.Message}", Logger);
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
            Initialize();
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