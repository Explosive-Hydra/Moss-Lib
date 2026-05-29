using MossLib.Base;

namespace MossLib.Example.Lang;

public class ZhCnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "zh-CN";
    protected override void BuildLocaleData()
    {
        // Welcome
        Add("welcome", "欢迎!");

        // Command - TestHello
        Add("command.testhello.description", "测试Hello");
        Add("command.testhello.text", "你好世界 by Moss Lib! {0}!");

        // Tool - Console
        Add("tool.console.nullorempty", "命令不能为空或空值");
        Add("tool.console.notinitialized", "ConsoleScript 未初始化");

        // Tool - World
        Add("tool.world.checkforworld", "没有加载任何世界。要不试试开始游戏?");
        Add("tool.world.placeblock", "在 {0} 生成方块 {1} 失败:{2}");
        Add("tool.world.placeitem", "在 {0} 生成物品 {1} 失败:{2}");
        Add("tool.world.placeitem.nullorempty", "物品不能为空或空值");
        Add("tool.world.trygetsprite", "未找到背景精灵: {0}");

        // Tool - Player
        Add("tool.player.bodynull", "玩家身体对象为空");
        Add("tool.player.item.nullorempty", "物品标识符不能为空或空白");
        Add("tool.player.loaditem.fail", "加载或实例化物品资源失败: '{0}'");
        Add("tool.player.loaditem.missingcomponent", "资源 '{0}' 已加载但缺少所需的 Item 组件");

        // Tool - Multiplayer
        Add("tool.multiplayer.playername.nullorempty", "玩家名称不能为空或空值");

        // Tool - Config
        Add("tool.config.switchtype", "已将 {0} 设置为 {1}!");
        Add("tool.config.change.isnullorempty", "无法获取程序集位置!");
        Add("tool.config.change.filenotfoundexception", "找不到配置文件 {0}");

        // Tool - Utils
        Add("tool.utils.checkargumentcount", "预期至少 {0} 个参数 {1},但得到了 {2} 个");
        Add("tool.utils.parse.float.invalid", "'{0}' 不是有效的浮点数值!(2, 0.7, 14.1 等)");
        Add("tool.utils.parse.int.invalid", "'{0}' 不是有效的整数值!");
        Add("tool.utils.string.nullorempty", "输入字符串不能为空或空值");
    }
}