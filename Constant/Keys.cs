using System.Diagnostics.CodeAnalysis;

namespace MossLib.Constant;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class Keys
{
    public const string Jump = "jump";
    public const string Up = "up";
    public const string Left = "left";
    public const string Right = "right";
    public const string Down = "down";
    public const string WoundView = "woundview";
    public const string Throw = "throw";
    public const string SwitchHands = "switchhands";
    public const string ToggleInventory = "toggleinventory";
    public const string Speed1 = "speed1";
    public const string Speed2 = "speed2";
    public const string Speed3 = "speed3";
    public const string Ragdoll = "ragdoll";
    public const string ExpandDesc = "expanddesc";
    public const string Attack = "attack";
    public const string ItemInteract = "iteminteract";
    public const string Pause = "pause";
    public const string Console = "console";
    public const string Restart = "restart";
    public const string Craft = "craft";
    public const string Bark = "bark";
    public const string Favourite = "favourite";

    public static string FromAction(string action) => action switch
    {
        "jump" => Jump,
        "up" => Up,
        "left" => Left,
        "right" => Right,
        "down" => Down,
        "woundview" => WoundView,
        "throw" => Throw,
        "switchhands" => SwitchHands,
        "toggleinventory" => ToggleInventory,
        "speed1" => Speed1,
        "speed2" => Speed2,
        "speed3" => Speed3,
        "ragdoll" => Ragdoll,
        "expanddesc" => ExpandDesc,
        "attack" => Attack,
        "iteminteract" => ItemInteract,
        "pause" => Pause,
        "console" => Console,
        "restart" => Restart,
        "craft" => Craft,
        "bark" => Bark,
        "favourite" => Favourite,
        _ => null!
    };
}
