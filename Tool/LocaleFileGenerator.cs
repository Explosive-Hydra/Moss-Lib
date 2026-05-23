namespace MossLib.Tool
{
    public static class LocaleFileGenerator
    {
        public static void Main()
        {
            LocaleGenerator.InitializeDefaults();
            LocaleGenerator.PrintInfo();
            System.Console.WriteLine();
            LocaleGenerator.GenerateAll();
        }

        public static void GenerateEnglishOnly()
        {
            LocaleGenerator.InitializeDefaults();
            LocaleGenerator.GenerateSingle("EN");
        }

        public static void GenerateSimplifiedChineseOnly()
        {
            LocaleGenerator.InitializeDefaults();
            LocaleGenerator.GenerateSingle("zh-CN");
        }

        public static void GenerateTraditionalChineseOnly()
        {
            LocaleGenerator.InitializeDefaults();
            LocaleGenerator.GenerateSingle("zh-TW");
        }

        public static void GenerateCustom()
        {
            LocaleGenerator.InitializeDefaults();
            LocaleGenerator.GenerateAll();
        }
    }
}