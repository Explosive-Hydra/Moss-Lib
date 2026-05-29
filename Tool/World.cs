using System;
using System.Collections.Generic;
using MossLib.Example;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MossLib.Tool;

public static class World
{
    private const string LocaleKeyPre = "tool.world.";
    private static GameObject _cachedBgTemplate;

    private static readonly Dictionary<string, Sprite> SpriteCache =
        new(StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> MissingSpriteWarnings =
        new(StringComparer.OrdinalIgnoreCase);

    public static void PlaceBlock(int x, int y, ushort block)
    {
        PlaceBlock(new Vector2(x, y), block);
    }

    public static void PlaceBlock(Vector2 vector2, ushort block)
    {
        CheckForWorld();
        try
        {
            WorldGeneration.world.SetBlock(WorldGeneration.world.WorldToBlockPos(vector2), block);
        }
        catch (Exception ex)
        {
            Error($"placeblock", vector2, block, ex);
        }
    }

    public static void PlaceItem(int x, int y, string item)
    {
        PlaceItem(new Vector2(x, y), item);
    }

    public static void PlaceItem(Vector2 vector2, string item)
    {
        CheckForWorld();

        if (string.IsNullOrWhiteSpace(item))
            throw new ArgumentException(Locale($"placeitem.nullorempty"), nameof(item));

        try
        {
            Utils.Create(item, vector2, 0.0f);
        }
        catch (Exception ex)
        {
            Error($"placeitem", vector2, item, ex);
        }
    }

    public static void PlaceBackground(Vector2Int pos, string backgroundId)
    {
        if (WorldGeneration.world == null)
            return;

        if (!TryGetSprite(backgroundId, out Sprite sprite))
            return;

        GameObject template = GetBgTemplate();
        if (template == null)
            return;

        Vector3 worldPos3 = WorldGeneration.world.BlockToWorldPos(pos);
        GameObject go = UnityEngine.Object.Instantiate(template, worldPos3,
            Quaternion.identity);
        go.name = $"BgTile_{pos.x}_{pos.y}";
        go.SetActive(true);

        Transform parent = WorldGeneration.world.worldGrid?.transform;
        if (parent == null)
        {
            Tilemap chunk = WorldGeneration.world.GetClosestChunk(pos);
            if (chunk != null)
                parent = chunk.transform;
        }

        if (parent != null)
            go.transform.SetParent(parent, true);

        MeshFilter mf = go.GetComponent<MeshFilter>();
        mf.mesh = CreateTileMesh(pos);

        MeshRenderer mr = go.GetComponent<MeshRenderer>();
        Material mat = new(WorldGeneration.world.defaultMat)
        {
            mainTexture = sprite.texture,
            mainTextureScale = Vector2.one,
            mainTextureOffset = Vector2.one
        };
        mr.material = mat;
        mr.sortingOrder = -5000;
        mr.material.color = Color.gray;
    }

    public static Mesh CreateTileMesh(Vector2Int pos)
    {
        const int tileCount = 4;
        int u = pos.x % tileCount;
        int v = pos.y % tileCount;
        if (u < 0) u += tileCount;
        if (v < 0) v += tileCount;

        float step = 1f / tileCount;
        float u0 = u * step;
        float u1 = (u + 1) * step;
        float v0 = v * step;
        float v1 = (v + 1) * step;

        Mesh mesh = new()
        {
            vertices =
            [
                new(-0.5f, -0.5f, 0),
                new(0.5f, -0.5f, 0),
                new(-0.5f, 0.5f, 0),
                new(0.5f, 0.5f, 0)
            ],
            uv =
            [
                new(u0, v0),
                new(u1, v0),
                new(u0, v1),
                new(u1, v1)
            ],
            triangles = [0, 2, 1, 2, 3, 1]
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    private static GameObject GetBgTemplate()
    {
        if (_cachedBgTemplate != null)
            return _cachedBgTemplate;

        _cachedBgTemplate = new GameObject("World_BgTemplate");
        _cachedBgTemplate.AddComponent<MeshFilter>();
        _cachedBgTemplate.AddComponent<MeshRenderer>();
        _cachedBgTemplate.SetActive(false);
        UnityEngine.Object.DontDestroyOnLoad(_cachedBgTemplate);
        return _cachedBgTemplate;
    }

    private static bool TryGetSprite(string backgroundId, out Sprite sprite)
    {
        if (SpriteCache.TryGetValue(backgroundId, out sprite)
            && sprite != null)
            return true;

        sprite = Resources.Load<Sprite>(backgroundId);
        if (sprite == null)
        {
            if (MissingSpriteWarnings.Add(backgroundId))
                Warning("trygetsprite", backgroundId);
            return false;
        }

        SpriteCache[backgroundId] = sprite;
        return true;
    }

    public static void CheckForWorld()
    {
        if (PlayerCamera.main == null)
            throw new Exception(Locale("checkforworld"));
    }

    private static void Warning(string key, params object[] args)
    {
        var message = Locale(key, args);
        Log.Warning(message, Plugin.Logger);
    }

    private static void Error(string key, params object[] args)
    {
        var message = Locale(key, args);
        Log.Error(message, Plugin.Logger);
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat($"tool.{LocaleKeyPre}.{key}", args);
    }
}