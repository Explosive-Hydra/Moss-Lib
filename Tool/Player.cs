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
        
        if (PlayerCamera.main.body == null)
            throw new System.InvalidOperationException("Player body is null");
            
        PlayerCamera.main.body.transform.position = vector2;
        PlayerCamera.main.transform.position = vector2;
    }
    
    public static void Tp(int x, int y)
    {
        Tp(new Vector2(x, y));
    }
}