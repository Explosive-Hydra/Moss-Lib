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

        // Command - SpawnBlock
        Add("command.spawnblock.description", "在鼠标位置生成一个方块。");
        Add("command.spawnblock.blockid", "要生成的方块 ID。");
        Add("command.spawnblock.invalidblockid", "'{0}' 不是有效的方块 ID。");
        Add("command.spawnblock.success", "已生成方块 {0}。");

        // Command - ListBackground
        Add("command.listbackground.description", "列出所有可用的背景 ID。");
        Add("command.listbackground.header", "找到 {0} 个背景资源：");
        Add("command.listbackground.none", "未找到背景资源。");

        // Command - SpawnBackground
        Add("command.spawnbackground.description", "在鼠标位置生成一个背景。");
        Add("command.spawnbackground.backgroundid", "要生成的背景 ID。");
        Add("command.spawnbackground.invalidbackgroundid", "背景 ID 不能为空。");
        Add("command.spawnbackground.success", "已生成背景 {0}。");

        // Tool - Config
        Add("tool.config.getconfig.notexistconfig", "不存在 {0} 这个配置");
        Add("tool.config.getconfig.notexistket", "{0} 不存在 {1} 这个键");

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
        Add("tool.player.slot.outofrange", "物品栏索引超出范围。最大槽位数: {0}");
        Add("tool.player.loaditem.fail", "加载或实例化物品资源失败: '{0}'");
        Add("tool.player.loaditem.missingcomponent", "资源 '{0}' 已加载但缺少所需的 Item 组件");

        // Tool - Multiplayer
        Add("tool.multiplayer.playername.nullorempty", "玩家名称不能为空或空值");
        Add("tool.multiplayer.teleport.success", "已传送: {0} 到 {1}");
        Add("tool.multiplayer.teleport.fail", "传送失败: {0}");

        // Tool - Config
        Add("tool.config.switchtype", "已将 {0} 设置为 {1}!");

        // Tool - Utils
        Add("tool.utils.checkargumentcount", "预期至少 {0} 个参数 {1},但得到了 {2} 个");
        Add("tool.utils.parse.float.invalid", "'{0}' 不是有效的浮点数值!(2, 0.7, 14.1 等)");
        Add("tool.utils.parse.int.invalid", "'{0}' 不是有效的整数值!");
        Add("tool.utils.string.nullorempty", "输入字符串不能为空或空值");

        // // Tool - UI Widgets
        // Add("tool.ui.widgets.text.null", "文本内容不能为 null。");
        // Add("tool.ui.widgets.button.label.null", "按钮标签不能为 null。");
        // // Tool - UI Widgets - Logs
        // Add("tool.ui.widgets.notemplate", "未找到游戏按钮模板，回退到手动构建");
    }
}