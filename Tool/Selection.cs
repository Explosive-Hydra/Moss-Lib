using System;
using MossLib.Example;
using UnityEngine;

namespace MossLib.Tool;

public static class Selection
{
    private static bool _isSelecting;
    private static Vector2Int _start;
    private static Vector2Int _end;

    public static bool IsSelecting => _isSelecting;

    public static bool BeginSelection()
    {
        if (!TryGetBlockPosition(out var blockPos))
            return false;

        _isSelecting = true;
        _start = blockPos;
        _end = blockPos;
        return true;
    }

    public static bool UpdateSelection()
    {
        if (!_isSelecting)
            return false;

        if (!TryGetBlockPosition(out var blockPos))
            return false;

        _end = blockPos;
        return true;
    }

    public static SelectionResult? EndSelection()
    {
        if (!_isSelecting)
            return null;

        _isSelecting = false;

        if (!TryGetBlockPosition(out var blockPos))
            return null;

        _end = blockPos;
        return GetResult();
    }

    public static void CancelSelection()
    {
        _isSelecting = false;
    }

    public static SelectionResult? CurrentBounds
    {
        get
        {
            if (!_isSelecting)
                return null;

            return GetResult();
        }
    }

    public static Vector2Int? CurrentMouseBlock
    {
        get
        {
            if (!TryGetBlockPosition(out var blockPos))
                return null;

            return blockPos;
        }
    }

    public static Vector2? CurrentMouseWorld
    {
        get
        {
            World.CheckForWorld();
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public static void ForEachBlock(Vector2Int min, Vector2Int max, Action<Vector2Int> action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        World.CheckForWorld();

        var world = WorldGeneration.world;
        var xMin = Mathf.Clamp(min.x, 0, world.width - 1);
        var xMax = Mathf.Clamp(max.x, 0, world.width - 1);
        var yMin = Mathf.Clamp(min.y, 0, world.height - 1);
        var yMax = Mathf.Clamp(max.y, 0, world.height - 1);

        for (var x = (int)xMin; x <= (int)xMax; x++)
        for (var y = (int)yMin; y <= (int)yMax; y++)
            action(new Vector2Int(x, y));
    }

    public static void ForEachBlock(SelectionResult result, Action<Vector2Int> action)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        ForEachBlock(result.Min, result.Max, action);
    }

    private static bool TryGetBlockPosition(out Vector2Int blockPos)
    {
        blockPos = Vector2Int.zero;

        if (WorldGeneration.world == null)
            return false;

        World.CheckForWorld();

        var worldPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blockPos = WorldGeneration.world.WorldToBlockPos(worldPoint);

        return blockPos.x >= 0 && blockPos.x < WorldGeneration.world.width &&
               blockPos.y >= 0 && blockPos.y < WorldGeneration.world.height;
    }

    private static SelectionResult GetResult()
    {
        var min = new Vector2Int(
            Mathf.Min(_start.x, _end.x),
            Mathf.Min(_start.y, _end.y)
        );

        var max = new Vector2Int(
            Mathf.Max(_start.x, _end.x),
            Mathf.Max(_start.y, _end.y)
        );

        var size = new Vector2Int(
            max.x - min.x + 1,
            max.y - min.y + 1
        );

        var center = new Vector2Int(
            (min.x + max.x) / 2,
            (min.y + max.y) / 2
        );

        return new SelectionResult
        {
            Min = min,
            Max = max,
            Start = _start,
            End = _end,
            Size = size,
            Center = center,
            Width = size.x,
            Height = size.y,
            Area = size.x * size.y
        };
    }
}

public class SelectionResult
{
    public Vector2Int Min { get; internal set; }
    public Vector2Int Max { get; internal set; }
    public Vector2Int Start { get; internal set; }
    public Vector2Int End { get; internal set; }
    public Vector2Int Size { get; internal set; }
    public Vector2Int Center { get; internal set; }
    public int Width { get; internal set; }
    public int Height { get; internal set; }
    public int Area { get; internal set; }

    public Vector2 CenterWorld
    {
        get
        {
            if (WorldGeneration.world != null)
                return WorldGeneration.world.BlockToWorldPos(Center);

            return Center;
        }
    }

    public override string ToString()
    {
        return
            $"Selection[min=({Min.x},{Min.y}), max=({Max.x},{Max.y}), size={Width}x{Height}, area={Area}]";
    }
}
