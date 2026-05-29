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
        Add("tool.world.placeblock", "在 {0} 生成方塊 {1} 失敗:{2}");
        Add("tool.world.placeitem", "在 {0} 生成物品 {1} 失敗:{2}");
        Add("tool.world.placeitem.nullorempty", "物品不能為空或空值");  
        Add("tool.world.trygetsprite", "未找到背景精靈: {0}");

        // Tool - Player
        Add("tool.player.bodynull", "玩家身體物件為空");
        Add("tool.player.item.nullorempty", "物品標識符不能為空或空白");
        Add("tool.player.slot.outofrange", "物品欄索引超出範圍。最大槽位數: {0}");
        Add("tool.player.loaditem.fail", "加載或實例化物品資源失敗: '{0}'");
        Add("tool.player.loaditem.missingcomponent", "資源 '{0}' 已加載但缺少所需的 Item 組件");

        // Tool - Multiplayer
        Add("tool.multiplayer.playername.nullorempty", "玩家名稱不能為空或空值");
        Add("tool.multiplayer.teleport.success", "已傳送: {0} 到 {1}");
        Add("tool.multiplayer.teleport.fail", "傳送失敗: {0}");

        // Tool - Config
        Add("tool.config.switchtype", "已將 {0} 設置為 {1}!");

        // Tool - Utils
        Add("tool.utils.checkargumentcount", "預期至少 {0} 個參數 {1},但得到了 {2} 個");
        Add("tool.utils.parse.float.invalid", "'{0}' 不是有效的浮點數值!(2, 0.7, 14.1 等)");
        Add("tool.utils.parse.int.invalid", "'{0}' 不是有效的整數值!");
        Add("tool.utils.string.nullorempty", "輸入字串不能為空或空值");
    }
}