using System;
using System.Collections.Generic;
using BepInEx.Logging;
using MossLib.Base;
using MossLib.Example.Lang;

namespace MossLib.Tool
{
    public static class LocaleGenerator
    {
        private static readonly List<ModLangGenBase> Generators = new();
        private static readonly ManualLogSource Logger = Plugin.Logger;

        public static void Register(ModLangGenBase generator)
        {
            if (generator != null && !Generators.Contains(generator))
            {
                Generators.Add(generator);
            }
        }

        public static void GenerateAll(string outputDirectory = null)
        {
            if (Generators.Count == 0)
            {
                Log.Warning("[LocaleGenerator] Warning: No language generators registered", Logger);
                return;
            }

            Log.Info("=== Starting localization file generation ===", Logger);
            
            foreach (var generator in Generators)
            {
                generator.Generate(outputDirectory);
            }

            Log.Info($"=== Generation complete! Generated {Generators.Count} language file(s) ===", Logger);
        }

        public static void GenerateSingle(string languageCode, string outputDirectory = null)
        {
            var generator = Generators.Find(g => g.GetType().Name.StartsWith(languageCode, StringComparison.OrdinalIgnoreCase));
            
            if (generator == null)
            {
                Log.Error($"[LocaleGenerator] Error: No generator found for language code '{languageCode}'", Logger);
                return;
            }

            generator.Generate(outputDirectory);
        }

        public static void InitializeDefaults()
        {
            Generators.Clear();
            
            Register(new EnLangGenerator());
            Register(new ZhCnLangGenerator());
            Register(new ZhTwLangGenerator());
        }

        public static void PrintInfo()
        {
            Log.Info("=== Registered Language Generators ===", Logger);
            foreach (var generator in Generators)
            {
                var type = generator.GetType().Name;
                var code = GetPrivateProperty(generator, "LanguageCode");
                var count = generator.Count;
                Log.Info($"  {type}: Language Code={code}, Entries={count}", Logger);
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
    }
}
