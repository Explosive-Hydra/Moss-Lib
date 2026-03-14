using System;
using System.Globalization;
using UnityEngine;

namespace MossLib;

public class Tools
{
    // ReSharper disable once UnusedMember.Global
    public static void Alert(string text, bool important = false)
    {
        PlayerCamera.main.DoAlert(text, important);
    }
       
    // ReSharper disable once UnusedMember.Global
    public static void CheckForWorld()
    {
        if (!(bool) (UnityEngine.Object) PlayerCamera.main)
            throw new Exception("No world is loaded. Try starting a game?");
    }

    // ReSharper disable once UnusedMember.Global
    public static void CheckArgumentCount(string[] args, int desired)
    {
        if (args.Length <= desired)
            throw new Exception($"Expected at least {desired} argument{(desired > 1 ? "s" : (object) "")}, but got {args.Length - 1}.");
    }
    
    // ReSharper disable once UnusedMember.Global
    public static float ParseFloat(string s)
    {
        return !float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result) ? throw new Exception($"\"{s}\" is not a valid float value! (2, 0.7, 14.1, etc)") : result;
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void LogToConsole(string text, ConsoleScript consoleScript)
    {
        consoleScript.logs.Add($"[<alpha=#55>{TimeSpan.FromSeconds(Time.realtimeSinceStartup):mm\\:ss}<alpha=#FF>] {text}");
        if (consoleScript.logs.Count > 100)
            consoleScript.logs.RemoveAt(0);
        if (!consoleScript.active)
            return;
        UpdateLogScreen(consoleScript);
    }
    
    public static void UpdateLogScreen(ConsoleScript consoleScript)
    {
        consoleScript.logText.text = string.Join("\n", consoleScript.logs);
    }
}