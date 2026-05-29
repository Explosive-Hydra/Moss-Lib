// using System;
// using System.Collections.Generic;
// using BepInEx.Logging;
// using MossLib.Example;
// using TMPro;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.UI;
//
// namespace MossLib.Tool.UI;
//
// public static class UIWidgets
// {
//     private const string LocaleKeyPre = "tool.ui.widgets.";
//     private const float DefaultTextWidth = 160f;
//     private const float DefaultTextHeight = 40f;
//     private const float DefaultButtonWidth = 160f;
//     private const float DefaultButtonHeight = 60f;
//     private const int DefaultFontSize = 14;
//     private const int MaxNamePreviewLength = 20;
//
//     // ── cached assets ────────────────────────────────────────────────
//     private static Sprite _whiteSprite;
//     private static Font _defaultFont;
//     private static GameObject _gameButtonTemplate;
//     private static Sprite _gameButtonSprite;
//     // =================================================================
//     //  White 1×1 sprite
//     // =================================================================
//     private static Sprite GetWhiteSprite()
//     {
//         if (_whiteSprite != null)
//             return _whiteSprite;
//
//         var tex = new Texture2D(1, 1);
//         tex.SetPixel(0, 0, Color.white);
//         tex.Apply();
//         _whiteSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.zero);
//         _whiteSprite.name = "UIWidgets_WhiteSprite";
//         return _whiteSprite;
//     }
//
//     // =================================================================
//     //  Default uGUI font
//     // =================================================================
//     private static Font GetDefaultFont()
//     {
//         if (_defaultFont != null)
//             return _defaultFont;
//
//         _defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
//         return _defaultFont;
//     }
//
//     // =================================================================
//     //  Find a game button GameObject in the scene to clone
//     //  Caches the result for subsequent calls.
//     // =================================================================
//     private static GameObject FindGameButton()
//     {
//         if (_gameButtonTemplate != null)
//             return _gameButtonTemplate;
//
//         // Known paths to try first
//         string[] paths =
//         {
//             "Canvas/LangageSelect/langbutton(Clone)",
//             "Main Camera/Canvas/GammaPanel/Button",
//             "Canvas/GammaPanel/Button",
//             "Canvas/MenuBackground/Play",
//             "Canvas/Button (1)",
//             "Canvas/Button",
//         };
//         foreach (string p in paths)
//         {
//             var found = GameObject.Find(p);
//             if (found != null)
//             {
//                 if (found.GetComponent<Button>() != null && found.GetComponent<Image>() != null)
//                 {
//                     _gameButtonTemplate = found;
//                     return _gameButtonTemplate;
//                 }
//             }
//         }
//
//         // Fallback: scan all active Buttons in the scene
//         Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
//         foreach (var b in allButtons)
//         {
//             if (b == null || b.gameObject == null) continue;
//             string name = b.gameObject.name;
//             if (name.StartsWith("Button_") || name.StartsWith("StyledBtn_") || name.StartsWith("Text_"))
//                 continue;
//             var img = b.targetGraphic as Image;
//             if (img != null && img.sprite != null && b.gameObject.activeInHierarchy)
//             {
//                 _gameButtonTemplate = b.gameObject;
//                 return _gameButtonTemplate;
//             }
//         }
//
//         return null;
//     }
//
//     // =================================================================
//     //  Extract game button sprite from scene (for manual build fallback)
//     // =================================================================
//     /// <summary>
//     /// 获取缓存的游戏按钮背景 Sprite（供 UILayout 等使用）。
//     /// </summary>
//     public static Sprite GetGameButtonSprite()
//     {
//         if (_gameButtonSprite != null)
//             return _gameButtonSprite;
//         return ExtractGameButtonSprite();
//     }
//
//     private static Sprite ExtractGameButtonSprite()
//     {
//         if (_gameButtonSprite != null)
//             return _gameButtonSprite;
//
//         string[] paths =
//         {
//             "Main Camera/Canvas/GammaPanel/Button",
//             "Canvas/GammaPanel/Button",
//             "Canvas/MenuBackground/Play",
//             "Canvas/Button (1)",
//             "Canvas/Button",
//         };
//         foreach (string p in paths)
//         {
//             var found = GameObject.Find(p);
//             if (found != null)
//             {
//                 var img = found.GetComponent<Image>();
//                 if (img != null && img.sprite != null)
//                 {
//                     _gameButtonSprite = img.sprite;
//                     return _gameButtonSprite;
//                 }
//             }
//         }
//
//         Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
//         foreach (var b in allButtons)
//         {
//             if (b == null || b.gameObject == null) continue;
//             string name = b.gameObject.name;
//             if (name.StartsWith("Button_") || name.StartsWith("StyledBtn_") || name.StartsWith("Text_"))
//                 continue;
//             var img = b.targetGraphic as Image;
//             if (img != null && img.sprite != null && b.gameObject.activeInHierarchy)
//             {
//                 _gameButtonSprite = img.sprite;
//                 return _gameButtonSprite;
//             }
//         }
//
//         return null;
//     }
//
//     // =================================================================
//     //  Search loaded sprites for game button sprite
//     // =================================================================
//     private static Sprite FindGameSprite()
//     {
//         if (_gameButtonSprite != null)
//             return _gameButtonSprite;
//
//         Sprite[] allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
//
//         string[] keywords = {
//             "button", "btn", "uibutton", "ui_btn",
//             "block", "panel", "slot", "frame",
//             "uibg", "bg_", "background", "border",
//             "box", "square", "rect", "rounded",
//             "tab", "bar", "dialogue", "dialog",
//             "menu", "option", "setting",
//             "white", "grey", "gray",
//         };
//         foreach (var spr in allSprites)
//         {
//             if (spr == null || string.IsNullOrEmpty(spr.name)) continue;
//             string lower = spr.name.ToLowerInvariant();
//             foreach (var kw in keywords)
//             {
//                 if (lower.Contains(kw))
//                 {
//                     return spr;
//                 }
//             }
//         }
//
//         return null;
//     }
//
//     // =================================================================
//     //  Apply game TMP font to a TextMeshProUGUI
//     //  Prefers active scene fonts over template fonts so custom font
//     //  mods (e.g. Unifont) are correctly picked up.
//     // =================================================================
//     private static void ApplyGameFont(TextMeshProUGUI tmp)
//     {
//         if (tmp == null) return;
//
//         // 1) Try TMP_Settings default font (respects font mods)
//         if (TMP_Settings.defaultFontAsset != null && TMP_Settings.defaultFontAsset != tmp.font)
//         {
//             tmp.font = TMP_Settings.defaultFontAsset;
//             return;
//         }
//
//         // 2) Search active scene TMP texts for their font (excluding our own)
//         TextMeshProUGUI[] allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
//         foreach (var t in allTexts)
//         {
//             if (t == null || t.font == null || !t.gameObject.activeInHierarchy) continue;
//             if (t == tmp) continue;
//             string tName = t.gameObject.name;
//             if (tName.StartsWith("StyledBtn_") || tName.StartsWith("Button_") || tName.StartsWith("Text_") || tName == "Label")
//                 continue;
//             if (t.font != tmp.font)
//             {
//                 tmp.font = t.font;
//                 return;
//             }
//         }
//
//         // 3) Fallback: clear font so TMP uses its built-in default
//         tmp.font = null;
//     }
//
//     // =================================================================
//     //  Procedural game-like button sprite (rounded dark panel)
//     // =================================================================
//     private static Sprite CreateGameLikeSprite()
//     {
//         const int size = 16;
//         const int radius = 4;
//         var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
//         tex.filterMode = FilterMode.Bilinear;
//         tex.wrapMode = TextureWrapMode.Clamp;
//
//         Color clear = Color.clear;
//         Color fill = new Color(0.18f, 0.18f, 0.18f);
//         float aa = 0.75f;
//
//         for (int y = 0; y < size; y++)
//         for (int x = 0; x < size; x++)
//         {
//             int dTL = (x - radius) * (x - radius) + (y - radius) * (y - radius);
//             int dTR = (x - (size - 1 - radius)) * (x - (size - 1 - radius)) + (y - radius) * (y - radius);
//             int dBL = (x - radius) * (x - radius) + (y - (size - 1 - radius)) * (y - (size - 1 - radius));
//             int dBR = (x - (size - 1 - radius)) * (x - (size - 1 - radius)) + (y - (size - 1 - radius)) * (y - (size - 1 - radius));
//
//             bool inTL = x < radius && y < radius;
//             bool inTR = x >= size - radius && y < radius;
//             bool inBL = x < radius && y >= size - radius;
//             bool inBR = x >= size - radius && y >= size - radius;
//
//             int dist = int.MaxValue;
//             if (inTL) dist = dTL;
//             else if (inTR) dist = dTR;
//             else if (inBL) dist = dBL;
//             else if (inBR) dist = dBR;
//
//             if (dist == int.MaxValue)
//                 tex.SetPixel(x, y, fill);
//             else
//             {
//                 float d = Mathf.Sqrt(dist);
//                 float a = Mathf.Clamp01((radius - d) / aa);
//                 tex.SetPixel(x, y, a > 0.01f ? new Color(fill.r, fill.g, fill.b, a) : clear);
//             }
//         }
//
//         tex.Apply();
//         var spr = Sprite.Create(tex, new Rect(0, 0, size, size),
//             new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect,
//             new Vector4(radius, radius, radius, radius));
//         spr.name = "UIWidgets_GameLikeSprite";
//         return spr;
//     }
//
//     // =================================================================
//     //  CreateText
//     // =================================================================
//     public static Text CreateText(
//         string content,
//         Vector2 anchoredPos,
//         Transform parent = null,
//         int? fontSize = null,
//         Vector2? size = null,
//         Action<Text> configureText = null)
//     {
//         if (content == null)
//             throw new ArgumentNullException(nameof(content), Locale("text.null"));
//
//         if (parent == null)
//             parent = UILayout.GetOrCreateCanvas().transform;
//
//         var go = new GameObject(SanitizeObjectName("Text", content));
//         go.transform.SetParent(parent, false);
//
//         var text = go.AddComponent<Text>();
//         text.text = content;
//         text.color = Color.black;
//         text.alignment = TextAnchor.MiddleCenter;
//         text.fontSize = fontSize ?? DefaultFontSize;
//
//         Font font = GetDefaultFont();
//         if (font != null)
//             text.font = font;
//
//         configureText?.Invoke(text);
//
//         Vector2 finalSize = size ?? new Vector2(DefaultTextWidth, DefaultTextHeight);
//         UILayout.SetRectTransform(go, anchoredPos, finalSize);
//
//         return text;
//     }
//
//     // =================================================================
//     //  CreateButton  (basic flat style)
//     // =================================================================
//     public static Button CreateButton(
//         string label,
//         Vector2 anchoredPos,
//         UnityAction onClick = null,
//         Transform parent = null,
//         Vector2? size = null,
//         int? fontSize = null,
//         Action<Text> configureText = null)
//     {
//         if (label == null)
//             throw new ArgumentNullException(nameof(label), Locale("button.label.null"));
//
//         if (parent == null)
//             parent = UILayout.GetOrCreateCanvas().transform;
//
//         var go = new GameObject(SanitizeObjectName("Button", label));
//         go.transform.SetParent(parent, false);
//
//         Vector2 finalSize = size ?? new Vector2(DefaultButtonWidth, DefaultButtonHeight);
//         UILayout.SetRectTransform(go, anchoredPos, finalSize);
//
//         var img = go.AddComponent<Image>();
//         img.color = Color.white;
//         img.sprite = GetWhiteSprite();
//         img.type = Image.Type.Sliced;
//
//         var button = go.AddComponent<Button>();
//         button.targetGraphic = img;
//         if (onClick != null)
//             button.onClick.AddListener(onClick);
//
//         var btnText = CreateText(label, Vector2.zero, go.transform, fontSize,
//             configureText: configureText);
//         ConfigureAsStretchChild(btnText.GetComponent<RectTransform>());
//
//         return button;
//     }
//
//     // =================================================================
//     //  CreateButtonByLocale
//     // =================================================================
//     public static Button CreateButtonByLocale(
//         string localeKey,
//         Vector2 anchoredPos,
//         UnityAction onClick = null,
//         Transform parent = null,
//         object[] formatArgs = null,
//         Vector2? size = null,
//         int? fontSize = null,
//         Action<Text> configureText = null)
//     {
//         if (localeKey == null)
//             throw new ArgumentNullException(nameof(localeKey), Locale("button.label.null"));
//
//         string label = formatArgs is { Length: > 0 }
//             ? Locale(localeKey, formatArgs)
//             : Locale(localeKey);
//
//         return CreateButton(label, anchoredPos, onClick, parent, size, fontSize, configureText);
//     }
//
//     // =================================================================
//     //  CreateStyledButton  ★ 游戏内置按钮样式
//     // =================================================================
//     /// <summary>
//     /// 创建与游戏内置按钮一致的 Button。
//     /// 1) 优先从场景中找到一个游戏按钮并完整克隆（保留半透明背景等视觉效果）
//     /// 2) 清理额外组件（LanguageButton、多余 TMP 子对象）
//     /// 3) 若传入了 description，更新 UITooltip 的描述；否则销毁
//     /// 4) 应用游戏字体（Unifont 等自定义字体模组）
//     /// 5) 若无游戏按钮可克隆，回退到手动构建
//     /// </summary>
//     public static Button CreateStyledButton(
//         string label,
//         Vector2 anchoredPos,
//         UnityAction onClick = null,
//         Transform parent = null,
//         Vector2? size = null,
//         int? fontSize = null,
//         Action<TextMeshProUGUI> configureText = null,
//         Vector2? anchorMin = null,
//         Vector2? anchorMax = null,
//         Vector2? pivot = null,
//         string description = null)
//     {
//         if (label == null)
//             throw new ArgumentNullException(nameof(label), Locale("button.label.null"));
//
//         if (parent == null)
//             parent = UILayout.GetOrCreateCanvas().transform;
//
//         // ── 方案 A：克隆游戏按钮 ──────────────────────────────────────
//         GameObject template = FindGameButton();
//         if (template != null)
//         {
//             return CreateStyledButtonFromClone(template, label, anchoredPos, onClick,
//                 parent, size, fontSize, configureText,
//                 anchorMin, anchorMax, pivot, description);
//         }
//
//         // ── 方案 B：手动构建 ──────────────────────────────────────────
//         Info(Locale("no_template"));
//         return CreateStyledButtonManual(label, anchoredPos, onClick,
//             parent, size, fontSize, configureText,
//             anchorMin, anchorMax, pivot, description);
//     }
//
//     // ── 从克隆创建按钮 ──────────────────────────────────────────────
//     private static Button CreateStyledButtonFromClone(
//         GameObject template,
//         string label,
//         Vector2 anchoredPos,
//         UnityAction onClick,
//         Transform parent,
//         Vector2? size,
//         int? fontSize,
//         Action<TextMeshProUGUI> configureText,
//         Vector2? anchorMin,
//         Vector2? anchorMax,
//         Vector2? pivot,
//         string description)
//     {
//         var clone = UnityEngine.Object.Instantiate(template, parent, false);
//         clone.name = SanitizeObjectName("StyledBtn", label);
//
//         // ── 清理组件 ──────────────────────────────────────────────
//         var langBtn = clone.GetComponent("LanguageButton");
//         if (langBtn != null)
//             UnityEngine.Object.DestroyImmediate(langBtn);
//
//         var btn = clone.GetComponent<Button>();
//         if (btn == null)
//             btn = clone.AddComponent<Button>();
//
//         // 移除除了第一个之外的所有 TMP 子对象
//         int tmpChildIndex = 0;
//         for (int i = clone.transform.childCount - 1; i >= 0; i--)
//         {
//             var child = clone.transform.GetChild(i);
//             var tmp = child.GetComponent<TextMeshProUGUI>();
//             if (tmp != null)
//             {
//                 if (tmpChildIndex == 0)
//                 {
//                     tmp.text = label;
//                     if (fontSize.HasValue)
//                         tmp.fontSize = fontSize.Value;
//                     tmpChildIndex++;
//                 }
//                 else
//                 {
//                     UnityEngine.Object.DestroyImmediate(child.gameObject);
//                 }
//             }
//         }
//
//         // 如果没有找到 TMP 子对象，创建一个
//         TextMeshProUGUI mainTmp = null;
//         for (int i = 0; i < clone.transform.childCount; i++)
//         {
//             mainTmp = clone.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
//             if (mainTmp != null) break;
//         }
//         if (mainTmp == null)
//         {
//             var tgo = new GameObject("Label");
//             tgo.transform.SetParent(clone.transform, false);
//             mainTmp = tgo.AddComponent<TextMeshProUGUI>();
//             mainTmp.text = label;
//             mainTmp.fontSize = fontSize ?? 16f;
//             mainTmp.alignment = TextAlignmentOptions.Center;
//             mainTmp.enableWordWrapping = false;
//             var tr = tgo.GetComponent<RectTransform>();
//             tr.anchorMin = Vector2.zero;
//             tr.anchorMax = Vector2.one;
//             tr.sizeDelta = new Vector2(-30, -20);
//             tr.anchoredPosition = Vector2.zero;
//         }
//
//         ApplyGameFont(mainTmp);
//
//         // ── 处理 UITooltip ───────────────────────────────────────────
//         var uiTooltip = clone.GetComponent("UITooltip");
//         if (uiTooltip != null)
//         {
//             if (description != null)
//             {
//                 var tipDescField = uiTooltip.GetType().GetField("tipDesc",
//                     System.Reflection.BindingFlags.Public |
//                     System.Reflection.BindingFlags.NonPublic |
//                     System.Reflection.BindingFlags.Instance);
//                 if (tipDescField != null)
//                     tipDescField.SetValue(uiTooltip, description);
//
//                 var tipNameField = uiTooltip.GetType().GetField("tipName",
//                     System.Reflection.BindingFlags.Public |
//                     System.Reflection.BindingFlags.NonPublic |
//                     System.Reflection.BindingFlags.Instance);
//                 if (tipNameField != null)
//                     tipNameField.SetValue(uiTooltip, label);
//
//                 var skipLocaleField = uiTooltip.GetType().GetField("skipLocale",
//                     System.Reflection.BindingFlags.Public |
//                     System.Reflection.BindingFlags.NonPublic |
//                     System.Reflection.BindingFlags.Instance);
//                 if (skipLocaleField != null && skipLocaleField.FieldType == typeof(bool))
//                     skipLocaleField.SetValue(uiTooltip, true);
//             }
//             else
//             {
//                 UnityEngine.Object.DestroyImmediate(uiTooltip);
//             }
//         }
//
//         // ── 设置位置和大小 ────────────────────────────────────────────
//         var rect = clone.GetComponent<RectTransform>();
//         if (rect == null)
//             rect = clone.AddComponent<RectTransform>();
//
//         rect.anchorMin = anchorMin ?? new Vector2(0, 1);
//         rect.anchorMax = anchorMax ?? new Vector2(0, 1);
//         rect.pivot = pivot ?? new Vector2(0, 1);
//         Vector2 finalSize = size ?? new Vector2(DefaultButtonWidth, DefaultButtonHeight);
//         rect.anchoredPosition = anchoredPos;
//         rect.sizeDelta = finalSize;
//
//         rect.SetAsLastSibling();
//
//         // ── 点击事件 ──────────────────────────────────────────────────
//         btn.onClick.RemoveAllListeners();
//         if (onClick != null)
//             btn.onClick.AddListener(onClick);
//
//         configureText?.Invoke(mainTmp);
//
//         return btn;
//     }
//
//     // ── 手动构建按钮（后备方案） ────────────────────────────────────
//     private static Button CreateStyledButtonManual(
//         string label,
//         Vector2 anchoredPos,
//         UnityAction onClick,
//         Transform parent,
//         Vector2? size,
//         int? fontSize,
//         Action<TextMeshProUGUI> configureText,
//         Vector2? anchorMin,
//         Vector2? anchorMax,
//         Vector2? pivot,
//         string description)
//     {
//         // ── 查找按钮背景 Sprite ────────────────────────────────────
//         Sprite bgSprite = ExtractGameButtonSprite();
//         bool useGameSprite = bgSprite != null;
//
//         if (!useGameSprite)
//             bgSprite = FindGameSprite();
//         useGameSprite = bgSprite != null;
//
//         if (!useGameSprite)
//         {
//             bgSprite = CreateGameLikeSprite();
//         }
//
//         // ── 构建按钮 ───────────────────────────────────────────────
//         var go = new GameObject(SanitizeObjectName("StyledBtn", label));
//         go.transform.SetParent(parent, false);
//
//         var rect = go.AddComponent<RectTransform>();
//         rect.anchorMin = anchorMin ?? new Vector2(0, 1);
//         rect.anchorMax = anchorMax ?? new Vector2(0, 1);
//         rect.pivot = pivot ?? new Vector2(0, 1);
//         Vector2 finalSize = size ?? new Vector2(DefaultButtonWidth, DefaultButtonHeight);
//         rect.anchoredPosition = anchoredPos;
//         rect.sizeDelta = finalSize;
//
//         var img = go.AddComponent<Image>();
//         img.sprite = bgSprite;
//         img.type = Image.Type.Sliced;
//         img.color = useGameSprite ? new Color(1f, 1f, 1f, 0.5f) : Color.white;
//
//         var btn = go.AddComponent<Button>();
//         btn.targetGraphic = img;
//         btn.transition = Selectable.Transition.ColorTint;
//
//         var colors = btn.colors;
//         if (useGameSprite)
//         {
//             colors.normalColor      = new Color(1f, 1f, 1f, 0.5f);
//             colors.highlightedColor = new Color(1f, 1f, 1f, 0.7f);
//             colors.pressedColor     = new Color(1f, 1f, 1f, 0.9f);
//             colors.selectedColor    = new Color(1f, 1f, 1f, 0.7f);
//         }
//         else
//         {
//             colors.normalColor      = new Color(0.8f, 0.8f, 0.8f, 1f);
//             colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
//             colors.pressedColor     = new Color(0.6f, 0.6f, 0.6f, 1f);
//             colors.selectedColor    = new Color(0.9f, 0.9f, 0.9f, 1f);
//         }
//         colors.disabledColor    = new Color(1f, 1f, 1f, 0.2f);
//         colors.colorMultiplier  = 1f;
//         colors.fadeDuration     = 0.12f;
//         btn.colors = colors;
//         if (onClick != null)
//             btn.onClick.AddListener(onClick);
//
//         var tgo = new GameObject("Label");
//         tgo.transform.SetParent(go.transform, false);
//         var tmp = tgo.AddComponent<TextMeshProUGUI>();
//         tmp.text = label;
//         tmp.fontSize = fontSize ?? 16f;
//         tmp.alignment = TextAlignmentOptions.Center;
//         tmp.enableWordWrapping = false;
//         tmp.color = useGameSprite ? Color.white : new Color(0.9f, 0.9f, 0.9f);
//
//         ApplyGameFont(tmp);
//
//         var tr = tgo.GetComponent<RectTransform>();
//         tr.anchorMin = Vector2.zero;
//         tr.anchorMax = Vector2.one;
//         tr.sizeDelta = new Vector2(-30, -20);
//         tr.anchoredPosition = Vector2.zero;
//
//         rect.SetAsLastSibling();
//
//         configureText?.Invoke(tmp);
//
//         return btn;
//     }
//
//     // =================================================================
//     //  CreateStyledButtonByLocale
//     // =================================================================
//     public static Button CreateStyledButtonByLocale(
//         string localeKey,
//         Vector2 anchoredPos,
//         UnityAction onClick = null,
//         Transform parent = null,
//         object[] formatArgs = null,
//         Vector2? size = null,
//         int? fontSize = null,
//         Action<TextMeshProUGUI> configureText = null,
//         string descriptionLocaleKey = null,
//         object[] descFormatArgs = null)
//     {
//         if (localeKey == null)
//             throw new ArgumentNullException(nameof(localeKey), Locale("button.label.null"));
//
//         string label = formatArgs is { Length: > 0 }
//             ? Locale(localeKey, formatArgs)
//             : Locale(localeKey);
//
//         string description = null;
//         if (descriptionLocaleKey != null)
//         {
//             description = descFormatArgs is { Length: > 0 }
//                 ? Locale(descriptionLocaleKey, descFormatArgs)
//                 : Locale(descriptionLocaleKey);
//         }
//
//         return CreateStyledButton(label, anchoredPos, onClick, parent, size, fontSize,
//             configureText, description: description);
//     }
//
//     // =================================================================
//     //  Helpers
//     // =================================================================
//     private static string BuildTransformPath(Transform t)
//     {
//         var sb = new System.Text.StringBuilder();
//         while (t != null)
//         {
//             if (sb.Length > 0)
//                 sb.Insert(0, "/");
//             sb.Insert(0, t.name);
//             t = t.parent;
//         }
//         return sb.ToString();
//     }
//
//     private static string SanitizeObjectName(string prefix, string content)
//     {
//         string preview = content.Length > MaxNamePreviewLength
//             ? content.Substring(0, MaxNamePreviewLength) + "..."
//             : content;
//
//         preview = preview.Replace("\n", " ").Replace("\r", " ").Replace("/", "-");
//
//         return $"{prefix}_{preview}";
//     }
//
//     private static void ConfigureAsStretchChild(RectTransform rect)
//     {
//         if (rect == null) return;
//
//         rect.anchorMin = Vector2.zero;
//         rect.anchorMax = Vector2.one;
//         rect.sizeDelta = Vector2.zero;
//         rect.anchoredPosition = Vector2.zero;
//     }
//
//     private static void Info(string msg)
//     {
//         Log.Info(msg, Plugin.Logger);
//     }
//
//     private static string Locale(string key, params object[] args)
//     {
//         return ModLocale.GetFormat($"{LocaleKeyPre}{key}", args);
//     }
// }
