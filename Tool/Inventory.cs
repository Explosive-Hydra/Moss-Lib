using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MossLib.Example;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class Inventory
{
    private const string LocaleKeyPre = "tool.inventory.";

    private static Body GetBody()
    {
        World.CheckForWorld();

        return PlayerCamera.main.body == null
            ? throw new InvalidOperationException(Locale("bodynull"))
            : PlayerCamera.main.body;
    }

    public static bool IsSlotOccupied(int slot)
    {
        var body = GetBody();
        return body.HoldingItem(slot);
    }

    public static bool IsSlotEmpty(int slot)
    {
        return !IsSlotOccupied(slot);
    }

    public static Item GetItem(int slot)
    {
        var body = GetBody();
        return body.GetItem(slot);
    }

    public static string GetItemId(int slot)
    {
        var item = GetItem(slot);
        return item != null ? item.id : null;
    }

    public static bool HasItem(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException(Locale("id.nullorempty"), nameof(id));

        var body = GetBody();
        return body.HoldingItem(id);
    }

    public static bool HasItemThorough(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException(Locale("id.nullorempty"), nameof(id));

        var body = GetBody();
        return body.FindByIdThorough(id, out _);
    }

    public static bool HasAnyItem(params string[] ids)
    {
        if (ids == null || ids.Length == 0)
            return false;

        var body = GetBody();
        return ids.Any(body.HoldingItem);
    }

    public static int CountItem(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return 0;

        var body = GetBody();
        var count = 0;
        for (var i = 0; i < body.slots.Length; i++)
        {
            var item = body.GetItem(i);
            if (item != null && item.id == id)
                count++;
        }

        return count;
    }

    public static int CountItemThorough(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return 0;

        var body = GetBody();

        return body.GetAllItemsThorough().Count(item => item.id == id);
    }

    public static List<Item> GetAllItems()
    {
        var body = GetBody();
        return body.GetAllItems();
    }

    public static List<string> GetAllItemIds()
    {
        var items = GetAllItems();
        var ids = new List<string>(items.Count);
        ids.AddRange(items.Select(item => item.id));
        return ids;
    }

    public static List<Item> GetAllItemsThorough()
    {
        var body = GetBody();
        return body.GetAllItemsThorough();
    }

    public static List<Item> GetWearables()
    {
        var body = GetBody();
        return body.GetAllWearables();
    }

    public static bool HasWearable(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException(Locale("id.nullorempty"), nameof(id));

        var body = GetBody();
        return body.HasWearable(id);
    }

    public static int? FindFirstEmptySlot()
    {
        var body = GetBody();
        return body.FirstEmptySlot();
    }

    public static bool FindById(string id, out Item item)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            item = null;
            return false;
        }

        var body = GetBody();
        return body.FindByIdSurface(id, out item);
    }

    public static bool FindByIdThorough(string id, out Item item)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            item = null;
            return false;
        }

        var body = GetBody();
        return body.FindByIdThorough(id, out item);
    }

    public static bool FindByTag(string tag, out Item item)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            item = null;
            return false;
        }

        var body = GetBody();
        return body.FindByTagThorough(tag, out item);
    }

    public static int GetHandSlot()
    {
        var body = GetBody();
        return body.handSlot;
    }

    public static Item GetItemInHand()
    {
        var body = GetBody();
        return body.GetItem(body.handSlot);
    }

    public static string GetItemIdInHand()
    {
        var item = GetItemInHand();
        return item != null ? item.id : null;
    }

    public static int GetSlotCount()
    {
        var body = GetBody();
        return body.slots.Length;
    }

    public static int GetOccupiedSlotCount()
    {
        var body = GetBody();
        var count = 0;
        for (var i = 0; i < body.slots.Length; i++)
            if (body.HoldingItem(i))
                count++;

        return count;
    }

    public static string GetItemIdsString()
    {
        var ids = GetAllItemIds();
        return ids.Count > 0 ? string.Join(", ", ids) : Locale("empty");
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat($"{LocaleKeyPre}{key}", args);
    }
}