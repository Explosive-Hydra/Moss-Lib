using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MossLib.Example;

namespace MossLib.Tool;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class Tools
{
    private const string LocaleKeyPre = "tool.utils.";

    public static void CheckArgumentCount(string[] args, int desired)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(args));

        if (args.Length <= desired)
            throw new Exception(ModLocale.GetFormat($"{LocaleKeyPre}checkargumentcount", desired,
                desired > 1 ? "s" : "", args.Length - 1));
    }

    public static float ParseFloat(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException(ModLocale.GetFormat($"{LocaleKeyPre}string.nullorempty"), nameof(s));

        return !float.TryParse(
            s, NumberStyles.Float
               | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result)
            ? throw new FormatException(ModLocale.GetFormat($"{LocaleKeyPre}parse.float.invalid", s))
            : result;
    }

    public static int ParseInt(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            throw new ArgumentException(ModLocale.GetFormat($"{LocaleKeyPre}string.nullorempty"), nameof(s));

        return !int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
            ? throw new FormatException(ModLocale.GetFormat($"{LocaleKeyPre}parse.int.invalid", s))
            : result;
    }

    public static bool TryParseFloat(string s, out float result)
    {
        result = 0;
        if (string.IsNullOrWhiteSpace(s))
            return false;

        return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands,
            CultureInfo.InvariantCulture, out result);
    }
}