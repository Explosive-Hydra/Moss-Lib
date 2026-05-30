using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BepInEx.Logging;
using MossLib.Base;
using MossLib.Example.Lang;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class LocaleGenerator
{
    private static readonly List<ModLangGenBase> Generators = [];
    private static ManualLogSource _logger;

    public static void SetLogger(ManualLogSource logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public static void Register(ModLangGenBase generator, ManualLogSource logger)
    {
        if (generator == null)
            throw new ArgumentNullException(nameof(generator));

        if (Generators.Contains(generator))
            return;

        var assembly = Assembly.GetCallingAssembly();
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

        EnsureLogger();
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
                successCount++;
                Info($"[LocaleGenerator] Successfully generated: {generatorName}");
            }
            catch (Exception ex)
            {
                failureCount++;
                Error($"[LocaleGenerator] Failed to generate for {generator.GetType().Name}: {ex.Message}");
            }
        }

        Info(
            $"=== Generation complete! Success: {successCount}, Failed: {failureCount}, Total: {Generators.Count} ===");
    }

    public static void GenerateSingle(string languageCode, string outputDirectory = null)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
            // ReSharper disable once LocalizableElement
            throw new ArgumentException("Language code cannot be null or empty", nameof(languageCode));

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
        EnsureLogger();
        Info("=== Registered Language Generators ===");
        foreach (var generator in Generators)
        {
            var type = generator.GetType().Name;
            var code = GetLanguageCodeSafely(generator);
            var count = generator.Count;
            Info($"  {type}: Language Code: {code}, Entries: {count}");
        }
    }

    private static string GetLanguageCodeSafely(ModLangGenBase obj)
    {
        try
        {
            var type = obj.GetType();
            // 优先查找 public 属性
            var prop = type.GetProperty("LanguageCode",
                           BindingFlags.Public | BindingFlags.Instance)
                       ?? type.GetProperty("LanguageCode",
                           BindingFlags.NonPublic | BindingFlags.Instance);
            return prop?.GetValue(obj)?.ToString() ?? "N/A";
        }
        catch
        {
            return "N/A";
        }
    }

    private static void EnsureLogger()
    {
        if (_logger == null)
            throw new InvalidOperationException(
                "LocaleGenerator logger has not been set. Call SetLogger() or pass a logger to Register() first.");
    }

    private static void Info(string text)
    {
        _logger?.LogInfo(text);
    }

    private static void Warning(string text)
    {
        _logger?.LogWarning(text);
    }

    private static void Error(string text)
    {
        _logger?.LogError(text);
    }
}