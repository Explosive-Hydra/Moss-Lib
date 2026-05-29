using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MossLib.Tool.UI;

public static class UILayout
{
    private static Canvas _cachedCanvas;

    public static Canvas GetOrCreateCanvas()
    {
        if (_cachedCanvas != null) return _cachedCanvas;
        _cachedCanvas = Object.FindObjectOfType<Canvas>();
        if (_cachedCanvas == null)
        {
            GameObject canvasObj = new GameObject("ModCanvas");
            _cachedCanvas = canvasObj.AddComponent<Canvas>();
            _cachedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        return _cachedCanvas;
    }

    public static Image CreatePanel(Vector2 anchoredPos, Vector2 size, Color bgColor, Transform parent = null)
    {
        if (parent == null) parent = GetOrCreateCanvas().transform;
        GameObject go = new GameObject("Panel");
        go.transform.SetParent(parent, false);
        SetRectTransform(go, anchoredPos, size);
        Image img = go.AddComponent<Image>();
        img.color = bgColor;
        return img;
    }

    internal static void SetRectTransform(GameObject go, Vector2 anchoredPos, Vector2 sizeDelta)
    {
        RectTransform rect = go.GetComponent<RectTransform>();
        if (rect == null) rect = go.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = sizeDelta;
    }
}