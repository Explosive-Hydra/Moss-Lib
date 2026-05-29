using System;
using UnityEngine;

namespace MossLib.Tool;

public static class Key
{
    public static bool HasKey(string action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (string.IsNullOrWhiteSpace(action))
            return false;

        if (KeyBinds.binds.binds == null)
            KeyBinds.LoadBindsFromFile();

        return KeyBinds.binds.codeBinds != null
               && KeyBinds.binds.codeBinds.ContainsKey(action);
    }

    public static bool IsKey(string action)
    {
        var keyCode = ResolveKeyCode(action);
        return keyCode != KeyCode.None && Input.GetKey(keyCode);
    }

    public static bool IsKeyDown(string action)
    {
        var keyCode = ResolveKeyCode(action);
        return keyCode != KeyCode.None && Input.GetKeyDown(keyCode);
    }

    public static bool IsKeyUp(string action)
    {
        var keyCode = ResolveKeyCode(action);
        return keyCode != KeyCode.None && Input.GetKeyUp(keyCode);
    }

    private static KeyCode ResolveKeyCode(string action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        if (string.IsNullOrWhiteSpace(action))
            return KeyCode.None;

        if (KeyBinds.binds.binds == null)
            KeyBinds.LoadBindsFromFile();

        return KeyBinds.binds.codeBinds != null
               && KeyBinds.binds.codeBinds.TryGetValue(action, out var keyCode)
            ? keyCode
            : KeyCode.None;
    }
}