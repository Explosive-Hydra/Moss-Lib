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
            throw new ArgumentException(ModLocale.GetFormat($"{LocaleKeyPre}item.nullorempty"), nameof(item));
        
        try
        {
            Utils.Create(item, vector2, 0.0f);
        }
        catch (Exception ex)
        { 
            Error($"{LocaleKeyPre}setitem", vector2, item, ex);
        }
    }

    public static void SetBackground(Vector2 position, string backgroundId)
    {
        CheckForWorld();
        
        if (string.IsNullOrWhiteSpace(backgroundId))
            throw new ArgumentException(ModLocale.GetFormat($"{LocaleKeyPre}background.nullorempty"), nameof(backgroundId));
        
        try
        {
            var gameObject = Utils.Create(backgroundId, position, 0.0f);

            if (gameObject == null) return;
            gameObject.SetActive(true);
                
            if (WorldGeneration.world?.worldGrid != null)
            {
                gameObject.transform.SetParent(WorldGeneration.world.worldGrid.transform, true);
            }
        }
        catch (Exception ex)
        {
            Error($"{LocaleKeyPre}setbackground", position, backgroundId, ex);
        }
    }

    public static void SetBackground(int x, int y, string backgroundId)
    {
        SetBackground(new Vector2(x, y), backgroundId);
    }

    public static void CheckForWorld()
    {
        if (PlayerCamera.main == null)
            throw new Exception(ModLocale.GetFormat($"{LocaleKeyPre}checkforworld"));
    } 
    
    private static void Error(string key, params object[] args)
    {
        var message = ModLocale.GetFormat(key, args);
        Log.Error(message, Plugin.Logger);
    }
}
