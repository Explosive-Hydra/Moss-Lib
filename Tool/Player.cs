using MossLib.Example;
using UnityEngine;

namespace MossLib.Tool;

public static class Player
{
    public static void Alert(string text, bool important)
    {
        World.CheckForWorld();

        if (string.IsNullOrWhiteSpace(text))
            return;

        PlayerCamera.main.DoAlert(text, important);
    }

    public static void Alert(string text, bool important, float delay)
    {
        World.CheckForWorld();

        if (string.IsNullOrWhiteSpace(text))
            return;

        if (delay < 0)
            delay = 0;

        PlayerCamera.main.StartCoroutine(PlayerCamera.main.DoAlertDelayed(text, important, delay));
    }

    public static void Tp(Vector2 vector2)
    {
        World.CheckForWorld();

        if (Multiplayer.IsNetworkRunning)
        {
            Multiplayer.Tp(vector2);
        }
        else
        {
            if (PlayerCamera.main.body == null)
                throw new System.InvalidOperationException(Locale("player.bodynull"));
            
            PlayerCamera.main.body.transform.position = vector2;
            PlayerCamera.main.transform.position = vector2;
        }
    }
    
    public static void Tp(float x, float y)
    {
        Tp(new Vector2(x, y));
    }

    
    public static void PickItem(string item, int slot, bool force = false)
    {
        World.CheckForWorld();

        if (string.IsNullOrWhiteSpace(item))
            throw new System.ArgumentException(
                Locale("player.item.nullorempty"), nameof(item));

        if (PlayerCamera.main.body == null)
            throw new System.InvalidOperationException(Locale("player.bodynull"));

        var body = PlayerCamera.main.body;
        var position = body.transform.position;

        var createdObject = Utils.Create(item, position, 0.0f);
        if (createdObject == null)
            throw new System.InvalidOperationException(
                Locale("player.loaditem.fail", item));

        var itemComponent = createdObject.GetComponent<Item>();
        if (itemComponent == null)
        {
            Object.Destroy(createdObject);
            throw new System.InvalidOperationException(
                Locale("player.loaditem.missingcomponent", item));
        }

        body.PickUpItem(itemComponent, slot, force);
    }
    
    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat(key, args);
    }
}