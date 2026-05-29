![alt text](Cover.png)

[English Guide](README.md)

# Moss Lib

[GitHub](https://github.com/Black-Moss/Moss-Lib) / [Nexus Mods](https://www.nexusmods.com/scavprototype/mods/8)

_只是一个 [Black_Moss](https://github.com/Black-Moss) 的模组前置库。:)_

---

## 目录

- [概述](#概述)
- [安装](#安装)
- [快速开始](#快速开始)
- [本地化系统](#本地化系统)
- [指令系统](#指令系统)
- [语言生成器](#语言生成器)
- [工具参考](#工具参考)
  - [Log - 日志](#log---日志)
  - [GameConsole - 游戏控制台](#gameconsole---游戏控制台)
  - [World - 世界操作](#world---世界操作)
  - [Player - 玩家操作](#player---玩家操作)
  - [Multiplayer - 多人游戏](#multiplayer---多人游戏)
  - [Config - 配置](#config---配置)
  - [Tools - 工具函数](#tools---工具函数)

---

## 概述

**Moss Lib** 是一个针对 **Casualties Unknown**（及其 Demo）的 BepInEx 模组前置库，提供了一系列可复用的基类和实用工具，以简化模组开发流程。包含以下模块：

| 模块 | 说明 |
|---|---|
| [`ModLocaleBase`](Base/ModLocaleBase.cs) | 多语言本地化系统，基于 JSON 语言文件 |
| [`ModCommandBase`](Base/ModCommandBase.cs) | 自定义游戏内控制台指令注册基类 |
| [`ModLangGenBase`](Base/ModLangGenBase.cs) | 语言文件生成器，从代码自动生成 JSON 本地化文件 |
| [`Log`](Tool/Log.cs) | 增强的游戏内控制台日志工具 |
| [`GameConsole`](Tool/Console.cs) | 编程式执行游戏控制台命令的封装 |
| [`World`](Tool/World.cs) | 世界操作：放置方块、物品、背景图块 |
| [`Player`](Tool/Player.cs) | 玩家操作：传送、屏幕提示、物品栏管理 |
| [`Multiplayer`](Tool/Multiplayer.cs) | 多人游戏支持，通过反射集成 KrokoshaCasualtiesMP |
| [`Config`](Tool/Config.cs) | BepInEx 配置项开关辅助 |
| [`Tools`](Tool/Tools.cs) | 参数验证、浮点/整数解析工具 |

---

## 安装

1. 为 Casualties Unknown 安装 [BepInEx 5.x](https://github.com/BepInEx/BepInEx)。
2. 从 [Releases 页面](https://github.com/Black-Moss/Moss-Lib/releases)（或 [Nexus Mods](https://www.nexusmods.com/scavprototype/mods/8)）下载最新版 `MossLib.dll`。
3. 将 `MossLib.dll` 放入 `BepInEx/plugins/` 文件夹。
4. （可选）如需多人游戏功能，需安装 **KrokoshaCasualtiesMP** 作为软依赖。

> **对于模组开发者：** 在项目中添加 `MossLib.dll` 引用，并在插件类上添加 `BepInDependency("blackmoss.mosslib")` 特性。

---

## 快速开始

### 1. 添加 BepInEx 依赖

```csharp
[BepInPlugin(Guid, Name, Version)]
[BepInDependency("blackmoss.mosslib")]  // 添加此行
public class MyPlugin : BaseUnityPlugin
{
    // ...
}
```

### 2. 设置本地化

```csharp
// 创建本地化类（单例封装）
public class MyLocale : ModLocaleBase
{
    private static MyLocale _instance;

    public static void Initialize(ManualLogSource logger)
    {
        _instance = new MyLocale();
        _instance.Initialize(logger, Assembly.GetExecutingAssembly());
    }

    public static string Get(string key) => _instance?.GetString(key) ?? $"[{key}]";
    public static string GetFormat(string key, params object[] args) => _instance?.GetStringFormatted(key, args) ?? $"[{key}]";
}
```

### 3. 注册自定义指令

```csharp
[HarmonyPatch(typeof(ConsoleScript))]
public class MyCommand : ModCommandBase
{
    [HarmonyPatch("RegisterAllCommands")]
    [HarmonyPostfix]
    public static void RegisterCustomCommands(ConsoleScript __instance)
    {
        ConsoleScript.Commands.Add(new Command(
            "mycommand",
            "我的指令说明",
            _ => Log.Info("指令已执行！", Plugin.Logger),
            null
        ));
    }
}
```

---

## 本地化系统

本地化系统从插件目录下的 `Lang/` 文件夹加载 JSON 语言文件。

### 文件结构

```
BepInEx/plugins/YourPlugin/
├── Lang/
│   ├── EN.json        # 英语（回退语言）
│   ├── zh-CN.json     # 简体中文
│   └── zh-TW.json     # 繁体中文
└── YourPlugin.dll
```

### JSON 格式

语言文件使用嵌套 JSON 键，以点号分隔进行组织：

```json
{
    "welcome": "欢迎！",
    "command": {
        "mycommand": {
            "description": "我的自定义指令",
            "text": "你好 {0}！"
        }
    },
    "tool": {
        "player": {
            "bodynull": "玩家身体对象为空"
        }
    }
}
```

### API 方法

| 方法 | 说明 |
|---|---|
| [`GetString(key)`](Base/ModLocaleBase.cs:116) | 根据键获取本地化字符串；依次回退到英语和 `[key]` 占位符 |
| [`GetStringFormatted(key, args...)`](Base/ModLocaleBase.cs:163) | 获取本地化字符串并用 `string.Format()` 格式化参数 |
| [`GetStringArray(key)`](Base/ModLocaleBase.cs:149) | 获取本地化字符串数组（JSON 数组） |
| [`GetStringDictionary(key)`](Base/ModLocaleBase.cs:156) | 获取本地化字典（JSON 对象） |
| [`HasKey(key)`](Base/ModLocaleBase.cs:245) | 检查翻译键是否存在 |

### 示例

完整实现参见 [`Example/ModLocale.cs`](Example/ModLocale.cs)。

```csharp
// 在插件的 Awake() 中初始化
MyLocale.Initialize(Logger);

// 使用
string welcome = MyLocale.Get("welcome");                              // "欢迎！"
string message = MyLocale.GetFormat("command.mycommand.text", "世界");  // "你好世界！"
```

---

## 指令系统

指令系统允许你通过 Harmony 补丁注册自定义游戏控制台指令。

### 基类：[`ModCommandBase`](Base/ModCommandBase.cs)

| 成员 | 说明 |
|---|---|
| [`Initialize(logger, assembly, harmony?)`](Base/ModCommandBase.cs:18) | 使用日志器和插件程序集初始化；可指定 Harmony 实例 |
| [`LogToConsole(text)`](Base/ModCommandBase.cs:52) | 向游戏内控制台输出文本 |
| [`ApplicationLogCallback(condition, stackTrace, type)`](Base/ModCommandBase.cs:60) | Unity 日志回调，带颜色标记的控制台输出 |
| [`Logger`](Base/ModCommandBase.cs:95) | 受保护的 `ManualLogSource` 属性，供子类使用 |

### 示例

```csharp
[HarmonyPatch(typeof(ConsoleScript))]
public class MyCommand : ModCommandBase
{
    [HarmonyPatch("RegisterAllCommands")]
    [HarmonyPostfix]
    public static void RegisterCustomCommands(ConsoleScript __instance)
    {
        ConsoleScript.Commands.Add(new Command(
            "hello",
            "打个招呼",
            args => {
                string name = args.Length > 1 ? args[1] : "世界";
                LogToConsole($"你好，{name}！");
            },
            null
        ));
    }
}
```

完整示例参见 [`Example/ModCommand.cs`](Example/ModCommand.cs)。

---

## 语言生成器

语言生成器可以从 C# 代码自动创建 JSON 语言文件，省去手动编写和维护本地化 JSON 文件的工作。

### 基类：[`ModLangGenBase`](Base/ModLangGenBase.cs)

| 成员 | 说明 |
|---|---|
| [`LanguageCode`](Base/ModLangGenBase.cs:13) | 抽象属性；返回语言代码（如 `"EN"`、`"zh-CN"`） |
| [`BuildLocaleData()`](Base/ModLangGenBase.cs:32) | 抽象方法；调用 `Add()` 注册翻译条目 |
| [`Add(key, value)`](Base/ModLangGenBase.cs:34) | 添加一条翻译条目 |
| [`Generate(outputDirectory?)`](Base/ModLangGenBase.cs:50) | 生成 JSON 文件；与已有文件合并（不会覆盖已有条目） |
| [`Count`](Base/ModLangGenBase.cs:206) | 已注册的翻译条目数量 |

### 创建生成器

```csharp
public class EnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "EN";

    protected override void BuildLocaleData()
    {
        Add("welcome", "欢迎！");
        Add("command.hello.description", "打个招呼");
        Add("command.hello.text", "你好，{0}！");
    }
}
```

### 注册与生成

使用 [`LocaleGenerator`](Tool/LocaleGenerator.cs) 注册并生成所有语言文件：

| 方法 | 说明 |
|---|---|
| [`SetLogger(logger)`](Tool/LocaleGenerator.cs:15) | 设置日志器 |
| [`Register(generator, logger)`](Tool/LocaleGenerator.cs:20) | 注册一个语言生成器 |
| [`GenerateAll(outputDirectory?)`](Tool/LocaleGenerator.cs:35) | 生成所有已注册的语言文件 |
| [`GenerateSingle(languageCode, outputDirectory?)`](Tool/LocaleGenerator.cs:77) | 生成单个语言文件 |
| [`PrintInfo()`](Tool/LocaleGenerator.cs:102) | 打印所有已注册生成器的信息 |

```csharp
// 在插件的 Awake() 中
LocaleGenerator.SetLogger(Logger);
LocaleGenerator.Register(new EnLangGenerator(), Logger);
LocaleGenerator.Register(new ZhCnLangGenerator(), Logger);
LocaleGenerator.GenerateAll(); // 在 Lang/ 文件夹中创建 EN.json、zh-CN.json
```

> **注意：** 生成器会**合并**已有 JSON 文件——仅添加**新**条目，保留用户对现有翻译的任何修改。

---

## 工具参考

### Log - 日志

[`Log`](Tool/Log.cs) — 增强的游戏内控制台日志，集成 BepInEx 日志系统。

| 方法 | 说明 |
|---|---|
| [`LogToConsole(text)`](Tool/Log.cs:22) | 向游戏内控制台写入带时间戳的消息 |
| [`Info(text, logger)`](Tool/Log.cs:53) | 同时输出到游戏控制台和 BepInEx 日志 |
| [`Debug(text, logger)`](Tool/Log.cs:59) | 输出调试消息，带 `[DEBUG]` 前缀 |
| [`Error(text, logger)`](Tool/Log.cs:65) | 输出错误消息，带 `[ERROR]` 前缀 |
| [`Warning(text, logger)`](Tool/Log.cs:71) | 输出警告消息，带 `[WARNING]` 前缀 |
| [`Alert(text, logger, important, delay?)`](Tool/Log.cs:77) | 输出日志并显示玩家屏幕提示 |
| [`NewLine()`](Tool/Log.cs:42) | 在控制台中插入空行 |
| [`Divider(char?, length?)`](Tool/Log.cs:47) | 插入分隔线 |

### GameConsole - 游戏控制台

[`GameConsole`](Tool/Console.cs) — 编程式执行游戏控制台命令。

```csharp
// 执行任意游戏内控制台指令
GameConsole.RunCommand("some_command arg1 arg2");
```

### World - 世界操作

[`World`](Tool/World.cs) — 世界操作工具。

| 方法 | 说明 |
|---|---|
| [`PlaceBlock(x, y, block)`](Tool/World.cs:21) | 在指定方块坐标放置方块 |
| [`PlaceBlock(vector2, block)`](Tool/World.cs:26) | 在 Vector2 位置放置方块 |
| [`PlaceItem(x, y, item)`](Tool/World.cs:39) | 在指定方块坐标生成物品 |
| [`PlaceItem(vector2, item)`](Tool/World.cs:44) | 在 Vector2 位置生成物品 |
| [`PlaceBackground(pos, backgroundId)`](Tool/World.cs:61) | 放置背景图块 |
| [`CreateTileMesh(pos)`](Tool/World.cs:113) | 为背景渲染创建平铺网格 |
| [`CheckForWorld()`](Tool/World.cs:179) | 检查世界是否已加载，未加载则抛出异常 |
| [`ClearCache()`](Tool/World.cs:185) | 清除精灵/网格/材质缓存 |

### Player - 玩家操作

[`Player`](Tool/Player.cs) — 玩家操作工具。

| 方法 | 说明 |
|---|---|
| [`Alert(text, important)`](Tool/Player.cs:12) | 向玩家显示屏幕提示 |
| [`Alert(text, important, delay)`](Tool/Player.cs:22) | 延迟显示屏幕提示 |
| [`Tp(vector2)`](Tool/Player.cs:36) | 传送玩家（支持单人/多人模式） |
| [`Tp(x, y)`](Tool/Player.cs:54) | 传送至浮点数坐标 |
| [`PickItem(item, slot, force?)`](Tool/Player.cs:59) | 将物品放入指定物品栏槽位 |

```csharp
// 示例
Player.Alert("小心！", true);                // 重要提示
Player.Tp(100.5f, 200.3f);                   // 传送
Player.PickItem("rifle", 0, true);           // 将步枪放入槽位 0
```

### Multiplayer - 多人游戏

[`Multiplayer`](Tool/Multiplayer.cs) — 通过 KrokoshaCasualtiesMP 实现多人游戏集成。

| 成员 | 说明 |
|---|---|
| [`IsNetworkRunning`](Tool/Multiplayer.cs:25) | 检查多人游戏网络是否运行 |
| [`IsClient`](Tool/Multiplayer.cs:38) | 检查当前实例是否为客户端 |
| [`Tp(vector2)`](Tool/Multiplayer.cs:51) | 在多人游戏中传送本地玩家 |
| [`Tp(x, y)`](Tool/Multiplayer.cs:68) | 传送至浮点数坐标 |
| [`Tp(playerName, vector2)`](Tool/Multiplayer.cs:73) | 传送指定玩家（使用 `"@a"` 传送所有玩家） |
| [`Tp(playerName, x, y)`](Tool/Multiplayer.cs:111) | 传送指定玩家至浮点数坐标 |
| [`GetPlayerName(player)`](Tool/Multiplayer.cs:249) | 获取玩家对象的名称 |

```csharp
// 传送所有玩家到指定位置
if (Multiplayer.IsNetworkRunning)
{
    Multiplayer.Tp("@a", new Vector2(50f, 100f));
}
```

### Config - 配置

[`Config`](Tool/Config.cs) — BepInEx 配置项操作辅助。

| 方法 | 说明 |
|---|---|
| [`ChangeConfig(entry, value)`](Tool/Config.cs:11) | 设置配置项的值并保存 |
| [`SwitchType(configEntry, configName)`](Tool/Config.cs:20) | 切换布尔配置项的值 |
| [`SwitchType(configEntry, configName, logger, important)`](Tool/Config.cs:29) | 切换配置项并可选显示玩家提示 |

```csharp
// 切换配置项
ConfigEntry<bool> mySetting = Config.Bind("General", "MySetting", true, "说明");
Config.SwitchType(mySetting, "我的设置");
```

### Tools - 工具函数

[`Tools`](Tool/Tools.cs) — 参数解析与验证工具函数。

| 方法 | 说明 |
|---|---|
| [`CheckArgumentCount(args, desired)`](Tool/Tools.cs:14) | 确保参数数组至少有 `desired + 1` 个元素 |
| [`ParseFloat(s)`](Tool/Tools.cs:24) | 使用固定区域设置解析浮点数；失败时抛出异常 |
| [`ParseInt(s)`](Tool/Tools.cs:36) | 使用固定区域设置解析整数；失败时抛出异常 |
| [`TryParseFloat(s, out result)`](Tool/Tools.cs:46) | 尝试解析浮点数，不抛出异常 |

```csharp
// 在指令处理器中
Tools.CheckArgumentCount(args, 2);          // 需要至少 2 个参数
float x = Tools.ParseFloat(args[1]);        // 解析浮点数参数
int count = Tools.ParseInt(args[2]);        // 解析整数参数
```

---

## 项目结构

```
MossLib/
├── Plugin.cs                    # 插件主入口
├── Base/
│   ├── ModCommandBase.cs        # 指令注册基类
│   ├── ModLangGenBase.cs        # 语言生成器基类
│   └── ModLocaleBase.cs         # 本地化基类
├── Example/
│   ├── ModCommand.cs            # 指令实现示例
│   ├── ModLocale.cs             # 本地化实现示例
│   └── Lang/
│       ├── EnLangGenerator.cs   # 英语语言生成器
│       ├── ZhCnLangGenerator.cs # 简体中文生成器
│       └── ZhTwLangGenerator.cs # 繁体中文生成器
└── Tool/
    ├── Config.cs                # 配置操作工具
    ├── Console.cs               # 游戏控制台封装
    ├── LocaleGenerator.cs       # 语言文件生成器管理器
    ├── Log.cs                   # 控制台日志工具
    ├── Multiplayer.cs           # 多人游戏集成
    ├── Player.cs                # 玩家操作工具
    ├── Tools.cs                 # 参数解析工具
    └── World.cs                 # 世界操作工具
```
