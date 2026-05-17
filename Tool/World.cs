using System;
using MossLib.Example;
using UnityEngine;

namespace MossLib.Tool;

public static class World
{
    private const string LocaleKeyPre = "tool.world.";

    public static void SetBlock(int x, int y, ushort block)
    {
        SetBlock(new Vector2(x, y), block);
    }

    public static void SetBlock(Vector2 vector2, ushort block)
    {
        CheckForWorld();
        try
        {
            WorldGeneration.world.SetBlock(WorldGeneration.world.WorldToBlockPos(vector2), block);
        }
        catch (Exception ex)
        {
            Error($"{LocaleKeyPre}setblock", vector2, block, ex);
        }
    }

    public static void SetItem(int x, int y, string item)
    {
        SetItem(new Vector2(x, y), item);
    }

    public static void SetItem(Vector2 vector2, string item)
    {
        CheckForWorld();

        if (string.IsNullOrWhiteSpace(item))
            throw new ArgumentException(Locale($"{LocaleKeyPre}item.nullorempty"), nameof(item));

        try
        {
            Utils.Create(item, vector2, 0.0f);
        }
        catch (Exception ex)
        {
            Error($"{LocaleKeyPre}setitem", vector2, item, ex);
        }
    }

    public static void CheckForWorld()
    {
        if (PlayerCamera.main == null)
            throw new Exception(Locale($"{LocaleKeyPre}checkforworld"));
    }

    private static void Error(string key, params object[] args)
    {
        var message = Locale(key, args);
        Log.Error(message, Plugin.Logger);
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat(key, args);
    }
}