using System;
using System.Collections.Generic;
using BepInEx.Logging;
using MossLib.Base;
using MossLib.Example.Lang;

namespace MossLib.Tool;

public static class LocaleGenerator
{
    private static readonly List<ModLangGenBase> Generators = [];
    private static ManualLogSource _logger;

    public static void Register(ModLangGenBase generator, ManualLogSource logger)
    {
        if (generator == null || Generators.Contains(generator)) return;
        Generators.Add(generator);
        _logger = logger;
    }

    public static void GenerateAll(string outputDirectory = null)
    {
        if (Generators.Count == 0)
        {
            Warning("[LocaleGenerator] Warning: No language generators registered");
            return;
        }

        Info("=== Starting localization file generation ===");
            
        foreach (var generator in Generators)
        {
            generator.Generate(outputDirectory);
        }

        Info($"=== Generation complete! Generated {Generators.Count} language file(s) ===");
    }

    public static void GenerateSingle(string languageCode, string outputDirectory = null)
    {
        var generator = Generators.Find(g => g.GetType().Name.StartsWith(languageCode, StringComparison.OrdinalIgnoreCase));
            
        if (generator == null)
        { 
            Error($"[LocaleGenerator] Error: No generator found for language code '{languageCode}'");
            return;
        }

        generator.Generate(outputDirectory);
    }

    internal static void InitializeDefaults()
    {
        Generators.Clear();
            
        Register(new EnLangGenerator(), _logger);
        Register(new ZhCnLangGenerator(), _logger);
        Register(new ZhTwLangGenerator(), _logger);
    }

    internal static void PrintInfo()
    {
        Info("=== Registered Language Generators ===");
        foreach (var generator in Generators)
        {
            var type = generator.GetType().Name;
            var code = GetPrivateProperty(generator, "LanguageCode");
            var count = generator.Count;
            Info($"  {type}: Language Code={code}, Entries={count}");
        }
    }

    private static string GetPrivateProperty(ModLangGenBase obj, string propertyName)
    {
        try
        {
            var prop = obj.GetType().GetProperty(propertyName, 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.Public);
            return prop?.GetValue(obj)?.ToString() ?? "N/A";
        }
        catch
        {
            return "N/A";
        }
    }

    private static void Info(string text)
    {
        Log.Info(text, _logger);
    }
    
    private static void Warning(string text)
    {
        Log.Warning(text, _logger);
    }
    
    private static void Error(string text)
    {
        Log.Error(text, _logger);
    }
}