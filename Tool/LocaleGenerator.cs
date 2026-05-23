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

    public static void SetLogger(ManualLogSource logger)
    {
        _logger = logger;
    }

    public static void Register(ModLangGenBase generator, ManualLogSource logger)
    {
        if (generator == null || Generators.Contains(generator)) return;
        
        var assembly = System.Reflection.Assembly.GetCallingAssembly();
        generator.Initialize(logger, assembly);
        
        Generators.Add(generator);
        _logger = logger ?? _logger;
    }
    
    public static void GenerateAll(string outputDirectory = null)
    {
        if (Generators.Count == 0)
        {
            Warning("[LocaleGenerator] Warning: No language generators registered");
            return;
        }

        Info("=== Starting localization file generation ===");
             
        int successCount = 0;
        int failureCount = 0;

        foreach (var generator in Generators)
        {
            if (generator == null)
            {
                Warning("[LocaleGenerator] Warning: Skipping null generator");
                failureCount++;
                continue;
            }

            try
            {
                var generatorName = generator.GetType().Name;
                Info($"[LocaleGenerator] Generating language file for: {generatorName}");
                generator.Generate(outputDirectory);
                // successCount++;
                // Info($"[LocaleGenerator] Successfully generated: {generatorName}");
            }
            catch (Exception ex)
            {
                // failureCount++;
                // Error($"[LocaleGenerator] Failed to generate for {generator.GetType().Name}: {ex.Message}");
            }
        }

        Info($"=== Generation complete! Success: {successCount}, Failed: {failureCount}, Total: {Generators.Count} ===");
    }

    public static void GenerateSingle(string languageCode, string outputDirectory = null)
    {
        var generator = Generators.Find(g =>
            g.GetType().Name.StartsWith(languageCode, StringComparison.OrdinalIgnoreCase));
            
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
            Info($"  {type}: Language Code: {code}, Entries: {count}");
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
        _logger.LogInfo(text);
    }
    
    private static void Warning(string text)
    {
        _logger.LogWarning(text);
    }
    
    private static void Error(string text)
    {
        _logger.LogError(text);
    }
}