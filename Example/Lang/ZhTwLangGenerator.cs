using MossLib.Base;

namespace MossLib.Example.Lang;

public class ZhTwLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "zh-TW";

    protected override void BuildLocaleData()
    {
        // Welcome
        Add("welcome", "歡迎!");

        // Command - TestHello
        Add("command.testhello.description", "測試Hello");
        Add("command.testhello.text", "你好世界 by Moss Lib! {0}!");

        // Tool - Console
        Add("tool.console.nullorempty", "命令不能為空或空值");
        Add("tool.console.notinitialized", "ConsoleScript 未初始化");

        // Tool - World
        Add("tool.world.checkforworld", "沒有加載任何世界。要不試試開始遊戲?");
        Add("tool.world.setblock", "在 {0} 生成方塊 {1} 失敗:{2}");
        Add("tool.world.setitem", "在 {0} 生成物品 {1} 失敗:{2}");
        Add("tool.world.setbackground", "在 {0} 生成背景物件 {1} 失敗:{2}");
        Add("tool.world.item.nullorempty", "物品不能為空或空值");
        Add("tool.world.background.nullorempty", "背景物件ID不能為空或空值");

        // Tool - Player
        Add("tool.player.bodynull", "玩家身體物件為空");

        // Tool - Multiplayer
        Add("tool.multiplayer.playername.nullorempty", "玩家名稱不能為空或空值");

        // Tool - Config
        Add("tool.config.switchtype", "已將 {0} 設置為 {1}!");
        Add("tool.config.change.isnullorempty", "無法獲取程式集位置!");
        Add("tool.config.change.filenotfoundexception", "找不到配置文件 {0}");

        // Tool - Utils
        Add("tool.utils.checkargumentcount", "預期至少 {0} 個參數 {1},但得到了 {2} 個");
        Add("tool.utils.parse.float.invalid", "\"{0}\" 不是有效的浮點數值!(2, 0.7, 14.1 等)");
        Add("tool.utils.parse.int.invalid", "\"{0}\" 不是有效的整數值!");
        Add("tool.utils.string.nullorempty", "輸入字串不能為空或空值");
    }
}