using System;
using MossLib.Example;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MossLib.Tool.UI;

public static class UIWidgets
{
    private const string LocaleKeyPre = "tool.ui.widgets.";
    private const float DefaultTextWidth = 160f;
    private const float DefaultTextHeight = 40f;
    private const float DefaultButtonWidth = 160f;
    private const float DefaultButtonHeight = 60f;
    private const int DefaultFontSize = 14;
    private const int MaxNamePreviewLength = 20;

    public static Text CreateText(string content, Vector2 anchoredPos,
        Transform parent = null, int? fontSize = null, Vector2? size = null,
        Action<Text> configureText = null)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content), Locale("text.null"));

        if (parent == null)
            parent = UILayout.GetOrCreateCanvas().transform;

        GameObject go = new GameObject(SanitizeObjectName("Text", content));
        go.transform.SetParent(parent, false);

        Text text = go.AddComponent<Text>();
        text.text = content;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = fontSize ?? DefaultFontSize;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        configureText?.Invoke(text);

        Vector2 finalSize = size ?? new Vector2(DefaultTextWidth, DefaultTextHeight);
        UILayout.SetRectTransform(go, anchoredPos, finalSize);

        return text;
    }

    public static Button CreateButton(string label, Vector2 anchoredPos,
        UnityAction onClick = null, Transform parent = null,
        Vector2? size = null, int? fontSize = null,
        Action<Text> configureText = null)
    {
        if (label == null)
            throw new ArgumentNullException(nameof(label), Locale("button.label.null"));

        if (parent == null)
            parent = UILayout.GetOrCreateCanvas().transform;

        GameObject go = new GameObject(SanitizeObjectName("Button", label));
        go.transform.SetParent(parent, false);

        Vector2 finalSize = size ?? new Vector2(DefaultButtonWidth, DefaultButtonHeight);
        UILayout.SetRectTransform(go, anchoredPos, finalSize);

        Image img = go.AddComponent<Image>();
        img.color = Color.black;

        Button button = go.AddComponent<Button>();
        button.targetGraphic = img;
        if (onClick != null)
            button.onClick.AddListener(onClick);

        Text btnText = CreateText(label, Vector2.zero, go.transform, fontSize,
            configureText: configureText);
        ConfigureAsStretchChild(btnText.GetComponent<RectTransform>());

        return button;
    }

    public static Button CreateButtonByLocale(string localeKey, Vector2 anchoredPos,
        UnityAction onClick = null, Transform parent = null,
        object[] formatArgs = null, Vector2? size = null, int? fontSize = null,
        Action<Text> configureText = null)
    {
        if (localeKey == null)
            throw new ArgumentNullException(nameof(localeKey), Locale("button.label.null"));

        string label = formatArgs is { Length: > 0 }
            ? Locale(localeKey, formatArgs)
            : Locale(localeKey);

        return CreateButton(label, anchoredPos, onClick, parent, size, fontSize, configureText);
    }

    private static string SanitizeObjectName(string prefix, string content)
    {
        string preview = content.Length > MaxNamePreviewLength
            ? content.Substring(0, MaxNamePreviewLength) + "..."
            : content;

        // Remove characters that are invalid in Unity hierarchy names (newlines, carriage returns)
        preview = preview.Replace("\n", " ").Replace("\r", " ").Replace("/", "-");

        return $"{prefix}_{preview}";
    }

    private static void ConfigureAsStretchChild(RectTransform rect)
    {
        if (rect == null) return;

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat($"{LocaleKeyPre}{key}", args);
    }
}