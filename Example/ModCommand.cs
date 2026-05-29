using BepInEx.Logging;
using HarmonyLib;
using MossLib.Base;
using MossLib.Tool;

namespace MossLib.Example;

[HarmonyPatch(typeof(ConsoleScript))]
public class ModCommand : ModCommandBase
{
    private new static readonly ManualLogSource Logger = Plugin.Logger;

    [HarmonyPatch("RegisterAllCommands")]
    [HarmonyPostfix]
    public static void RegisterCustomCommands(ConsoleScript __instance)
    {
        ConsoleScript.Commands.Add(new Command(
            "testhello",
            Locale("testhello.description"), _ =>
                Info("testhello.text",
                    TestHello()
                ),
            null)
        );
        // ConsoleScript.Commands.Add(new Command(
        //     "testui",
        //     "TestUI", _ =>
        //     {
        //         // ── 创建样式 Panel 容器 ──────────────────────────────────
        //         var panelRect = UILayout.CreateStyledPanel(
        //             anchoredPos: new Vector2(50, -50),
        //             width: 400f,
        //             spacing: 12f
        //         );
        //
        //         // ── Panel 标题 ───────────────────────────────────────────
        //         var titleGo = new GameObject("Title");
        //         titleGo.transform.SetParent(panelRect, false);
        //         var titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
        //         titleTmp.text = "Hello UIWidgets!";
        //         titleTmp.fontSize = 20;
        //         titleTmp.alignment = TextAlignmentOptions.Center;
        //         titleTmp.color = Color.cyan;
        //         titleTmp.enableWordWrapping = false;
        //         var titleRect = titleGo.GetComponent<RectTransform>();
        //         titleRect.sizeDelta = new Vector2(370, 30);
        //
        //         // ── Basic Button ─────────────────────────────────────────
        //         UIWidgets.CreateStyledButton(
        //             "Basic Button",
        //             Vector2.zero,
        //             onClick: () => Debug.Log("Basic button clicked!"),
        //             parent: panelRect,
        //             size: new Vector2(370, 44),
        //             fontSize: 16
        //         );
        //
        //         // ── Styled Button with description ───────────────────────
        //         UIWidgets.CreateStyledButton(
        //             "你好 Game！",
        //             Vector2.zero,
        //             onClick: () => Debug.Log("你好！"),
        //             parent: panelRect,
        //             size: new Vector2(370, 44),
        //             fontSize: 16,
        //             configureText: t => t.fontStyle = FontStyles.Bold,
        //             description: "测试按钮描述"
        //         );
        //
        //         // ── Styled Button By Locale ──────────────────────────────
        //         UIWidgets.CreateStyledButtonByLocale(
        //             "Game Button",
        //             Vector2.zero,
        //             onClick: () => Debug.Log("Styled locale button clicked!"),
        //             parent: panelRect,
        //             size: new Vector2(370, 44),
        //             fontSize: 16,
        //             configureText: t => t.fontStyle = FontStyles.Bold
        //         );
        //     },
        //     null)
        // );
    }

    private static string TestHello()
    {
        var text = Locale("testhello.description");
        var result = "";
        for (int i = 0; i < text.Length; i++)
        {
            result += RichText.Size(text[i].ToString(), (i + 3) * 9);
        }

        return result;
    }

    private static string Locale(string key, params object[] args)
    {
        return ModLocale.GetFormat($"command.{key}", args);
    }

    private static void Info(string key, params object[] args)
    {
        var message = Locale(key, args);
        Log.Info(message, Logger);
    }
}
