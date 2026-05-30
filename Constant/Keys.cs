using System.Diagnostics.CodeAnalysis;

namespace MossLib.Constant;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public sealed class Keys
{
    public string Action { get; }

    private Keys(string action)
    {
        Action = action;
    }

    public static implicit operator string(Keys keys) => keys?.Action ?? "";
    public override string ToString() => Action;
    public override bool Equals(object? obj) => obj is Keys other && Action == other.Action;
    public override int GetHashCode() => Action.GetHashCode();

    public static readonly Keys Jump = new("jump");
    public static readonly Keys Up = new("up");
    public static readonly Keys Left = new("left");
    public static readonly Keys Right = new("right");
    public static readonly Keys Down = new("down");
    public static readonly Keys WoundView = new("woundview");
    public static readonly Keys Throw = new("throw");
    public static readonly Keys SwitchHands = new("switchhands");
    public static readonly Keys ToggleInventory = new("toggleinventory");
    public static readonly Keys Speed1 = new("speed1");
    public static readonly Keys Speed2 = new("speed2");
    public static readonly Keys Speed3 = new("speed3");
    public static readonly Keys Ragdoll = new("ragdoll");
    public static readonly Keys ExpandDesc = new("expanddesc");
    public static readonly Keys Attack = new("attack");
    public static readonly Keys ItemInteract = new("iteminteract");
    public static readonly Keys Pause = new("pause");
    public static readonly Keys Console = new("console");
    public static readonly Keys Restart = new("restart");
    public static readonly Keys Craft = new("craft");
    public static readonly Keys Bark = new("bark");
    public static readonly Keys Favourite = new("favourite");

    public static Keys FromAction(string action) => action switch
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
