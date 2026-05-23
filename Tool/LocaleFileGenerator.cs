namespace MossLib.Tool;

public static class LocaleFileGenerator
{
    private static bool _initialized;

    public static void Main()
    {
        InitializeIfNeeded();
        LocaleGenerator.PrintInfo();
        System.Console.WriteLine();
        LocaleGenerator.GenerateAll();
    }

    public static void GenerateEnglishOnly()
    {
        InitializeIfNeeded();
        LocaleGenerator.GenerateSingle("EN");
    }

    public static void GenerateSimplifiedChineseOnly()
    {
        InitializeIfNeeded();
        LocaleGenerator.GenerateSingle("zh-CN");
    }

    public static void GenerateTraditionalChineseOnly()
    {
        InitializeIfNeeded();
        LocaleGenerator.GenerateSingle("zh-TW");
    }

    public static void GenerateCustom()
    {
        InitializeIfNeeded();
        LocaleGenerator.GenerateAll();
    }

    private static void InitializeIfNeeded()
    {
        if (_initialized) return;
        LocaleGenerator.InitializeDefaults();
        _initialized = true;
    }
}