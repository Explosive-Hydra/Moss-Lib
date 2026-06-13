// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// namespace MossLib.Tool.UI;
//
// public static class UILayout
// {
//     private static Canvas _cachedCanvas;
//
//     public static Canvas GetOrCreateCanvas()
//     {
//         if (_cachedCanvas != null) return _cachedCanvas;
//         _cachedCanvas = Object.FindObjectOfType<Canvas>();
//         if (_cachedCanvas == null)
//         {
//             GameObject canvasObj = new GameObject("ModCanvas");
//             _cachedCanvas = canvasObj.AddComponent<Canvas>();
//             _cachedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
//             canvasObj.AddComponent<CanvasScaler>();
//             canvasObj.AddComponent<GraphicRaycaster>();
//         }
//         // Ensure high sorting order so mod UI renders above game UI
//         if (_cachedCanvas.sortingOrder < 100)
//             _cachedCanvas.sortingOrder = 100;
//
//         if (Object.FindObjectOfType<EventSystem>() == null)
//         {
//             GameObject es = new GameObject("EventSystem");
//             es.AddComponent<EventSystem>();
//             es.AddComponent<StandaloneInputModule>();
//         }
//
//         return _cachedCanvas;
//     }
//
//     /// <summary>
//     /// 创建简单的纯色 Panel。
//     /// </summary>
//     public static Image CreatePanel(Vector2 anchoredPos, Vector2 size, Color bgColor, Transform parent = null)
//     {
//         if (parent == null) parent = GetOrCreateCanvas().transform;
//         GameObject go = new GameObject("Panel");
//         go.transform.SetParent(parent, false);
//         SetRectTransform(go, anchoredPos, size);
//         Image img = go.AddComponent<Image>();
//         img.color = bgColor;
//         return img;
//     }
//
//     /// <summary>
//     /// 搜索已加载的 Sprite 中匹配 Panel 背景风格（如 uiBlockNano）的纹理。
//     /// 优先精确匹配，避免误配小图标或血条等非 Panel Sprite。
//     /// </summary>
//     private static Sprite FindGamePanelSprite()
//     {
//         Sprite[] allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
//
//         // 第 1 优先级：精确的 Panel 背景名称（uiBlockNano 系列）
//         string[] exactKeywords = { "uiblocknano", "uiblocks", "uiblock", "blocknano" };
//         foreach (var spr in allSprites)
//         {
//             if (spr == null || string.IsNullOrEmpty(spr.name)) continue;
//             string lower = spr.name.ToLowerInvariant();
//             foreach (var kw in exactKeywords)
//             {
//                 if (lower.Contains(kw))
//                     return spr;
//             }
//         }
//
//         // 第 2 优先级：窗口 / 背景 / 框架类
//         string[] broadKeywords = { "window", "dialogue", "dialog", "menu", "frame", "bg_", "background" };
//         foreach (var spr in allSprites)
//         {
//             if (spr == null || string.IsNullOrEmpty(spr.name)) continue;
//             string lower = spr.name.ToLowerInvariant();
//             foreach (var kw in broadKeywords)
//             {
//                 if (lower.Contains(kw))
//                     return spr;
//             }
//         }
//
//         return null;
//     }
//
//     /// <summary>
//     /// 创建带 VerticalLayoutGroup 的样式 Panel（直接复现 SaveSaveMenuPatcher 的构造逻辑）。
//     ///
//     /// 不使用场景中的游戏对象（如 GammaPanel / EnhancedSettingsPanel），
//     /// 而是直接构建：新建 GameObject → VerticalLayoutGroup → ContentSizeFitter(仅垂直) → Image(Sliced + uiBlockNano + 半透明黑)。
//     ///
//     /// 参考：SaveSaveMenuPatcher.BuildSaveUI()
//     ///   - childControlWidth = true, childControlHeight = true
//     ///   - childForceExpandHeight = false, childForceExpandWidth = true (default)
//     ///   - ContentSizeFitter 仅 verticalFit = PreferredSize（宽度由 sizeDelta 固定）
//     /// </summary>
//     public static RectTransform CreateStyledPanel(
//         Vector2 anchoredPos,
//         float width = 400f,
//         Transform parent = null,
//         RectOffset padding = null,
//         float spacing = 10f)
//     {
//         if (parent == null)
//             parent = GetOrCreateCanvas().transform;
//
//         // ── 创建 Panel 容器 ──
//         var container = new GameObject("StyledPanel");
//         container.transform.SetParent(parent, false);
//         var rect = container.AddComponent<RectTransform>();
//         rect.anchorMin = new Vector2(0, 1);
//         rect.anchorMax = new Vector2(0, 1);
//         rect.pivot = new Vector2(0, 1);
//         rect.anchoredPosition = anchoredPos;
//         rect.sizeDelta = new Vector2(width, 0f); // 宽度固定，高度由 ContentSizeFitter 自动调整
//
//         // ── VerticalLayoutGroup（匹配 SaveSaveMenuPatcher）──
//         var vlg = container.AddComponent<VerticalLayoutGroup>();
//         vlg.childAlignment = TextAnchor.UpperCenter;
//         vlg.childControlHeight = true;
//         vlg.childControlWidth = true;
//         vlg.childForceExpandHeight = false;
//         // childForceExpandWidth 默认 true（让子元素水平填满 Panel）
//         vlg.spacing = spacing;
//         vlg.padding = padding ?? new RectOffset(10, 10, 10, 10);
//
//         // ── ContentSizeFitter（仅垂直自适应，宽度固定）──
//         var fitter = container.AddComponent<ContentSizeFitter>();
//         fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
//
//         // ── 背景 Image（Sliced + uiBlockNano + 半透明黑）──
//         var bgImg = container.AddComponent<Image>();
//         bgImg.type = Image.Type.Sliced;
//
//         Sprite panelSpr = FindGamePanelSprite();
//         if (panelSpr != null)
//         {
//             bgImg.sprite = panelSpr;
//             bgImg.color = new Color(0f, 0f, 0f, 0.5f);
//         }
//         else
//         {
//             // 后备：使用按钮 Sprite
//             Sprite btnSprite = UIWidgets.GetGameButtonSprite();
//             if (btnSprite != null)
//             {
//                 bgImg.sprite = btnSprite;
//                 bgImg.color = new Color(0f, 0f, 0f, 0.5f);
//             }
//             else
//             {
//                 bgImg.color = new Color(0.12f, 0.12f, 0.12f, 0.85f);
//             }
//         }
//
//         rect.SetAsLastSibling();
//         return rect;
//     }
//
//     internal static void SetRectTransform(GameObject go, Vector2 anchoredPos, Vector2 sizeDelta)
//     {
//         RectTransform rect = go.GetComponent<RectTransform>();
//         if (rect == null) rect = go.AddComponent<RectTransform>();
//         rect.anchorMin = new Vector2(0, 1);
//         rect.anchorMax = new Vector2(0, 1);
//         rect.pivot = new Vector2(0, 1);
//         rect.anchoredPosition = anchoredPos;
//         rect.sizeDelta = sizeDelta;
//     }
// }

