using System.Diagnostics.CodeAnalysis;

namespace MossLib.Constant;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public sealed class Backgrounds
{
    public string Id { get; }
    public string LocaleKey { get; }

    private Backgrounds(string id, string localeKey)
    {
        Id = id;
        LocaleKey = localeKey;
    }

    public static implicit operator string(Backgrounds background) => background?.Id ?? "";
    public override string ToString() => LocaleKey;
    public override bool Equals(object? obj) => obj is Backgrounds other && Id == other.Id;
    public override int GetHashCode() => Id.GetHashCode();

    public static readonly Backgrounds Fungal = new("fungalBackground", "fungal");
    public static readonly Backgrounds Grass = new("grassBackground", "grass");
    public static readonly Backgrounds Ice = new("iceBackground", "ice");
    public static readonly Backgrounds Rock = new("rockBackground", "rock");
    public static readonly Backgrounds Sand = new("sandBackground", "sand");
    public static readonly Backgrounds Soil = new("soilBackground", "soil");
    public static readonly Backgrounds Steel = new("steelBackground", "steel");
    public static readonly Backgrounds Vents = new("ventsBackground", "vents");
    public static readonly Backgrounds Wasteland = new("wastelandBackground", "wasteland");

    public static Backgrounds FromId(string id) => id switch
    {
        "fungalBackground" => Fungal,
        "grassBackground" => Grass,
        "iceBackground" => Ice,
        "rockBackground" => Rock,
        "sandBackground" => Sand,
        "soilBackground" => Soil,
        "steelBackground" => Steel,
        "ventsBackground" => Vents,
        "wastelandBackground" => Wasteland,
        _ => null!
    };
}
