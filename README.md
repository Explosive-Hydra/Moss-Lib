![alt text](Covor.png)

[中文指南](README_ZH.md)

# Moss Lib

[GitHub](https://github.com/Black-Moss/Moss-Lib) / [Nexus Mods](https://www.nexusmods.com/scavprototype/mods/8)

_Just a simple library for [Black_Moss](https://github.com/Black-Moss). :)_

---

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Localization System](#localization-system)
- [Command System](#command-system)
- [Language Generator](#language-generator)
- [Tools Reference](#tools-reference)
  - [Log](#log)
  - [GameConsole](#gameconsole)
  - [World](#world)
  - [Player](#player)
  - [Key](#key)
  - [Multiplayer](#multiplayer)
  - [Config](#config)
  - [RichText](#richtext)
  - [Tools (Utils)](#tools-utils)
- [Constants Reference](#constants-reference)
  - [Blocks](#blocks)
  - [Items](#items)
  - [Backgrounds](#backgrounds)
  - [Keys](#keys-1)
- [UI Toolkit (WIP)](#ui-toolkit-wip)

---

## Overview

**Moss Lib** is a BepInEx plugin library for **Casualties Unknown** (and its demo), providing a set of reusable base classes and utility tools to simplify mod development. It includes:

| Module | Description |
|---|---|
| [`ModLocaleBase`](Base/ModLocaleBase.cs) | Multi-language localization system with JSON-based language files |
| [`ModCommandBase`](Base/ModCommandBase.cs) | Base class for registering custom in-game console commands |
| [`ModLangGenBase`](Base/ModLangGenBase.cs) | Language file generator that produces JSON locale files from code |
| [`Log`](Tool/Log.cs) | Advanced in-game console logging utilities |
| [`GameConsole`](Tool/Console.cs) | Wrapper to execute game console commands programmatically |
| [`World`](Tool/World.cs) | World manipulation: place blocks, items, and background tiles |
| [`Player`](Tool/Player.cs) | Player manipulation: teleport, alerts, inventory management |
| [`Key`](Tool/Key.cs) | Input handling: key binding checks, mouse click waiting, world-space coordinates |
| [`Multiplayer`](Tool/Multiplayer.cs) | Multiplayer support with reflection-based KrokoshaCasualtiesMP integration |
| [`Config`](Tool/Config.cs) | BepInEx configuration entry toggling helpers |
| [`RichText`](Tool/RichText.cs) | Unity rich text formatting: color, alpha, bold, italic, size |
| [`Tools`](Tool/Tools.cs) | Argument validation, float/int parsing utilities |
| [`Blocks`](Constant/Blocks.cs) | Strongly-typed block definitions with properties |
| [`Items`](Constant/Items.cs) | Strongly-typed item definitions with properties |
| [`Backgrounds`](Constant/Backgrounds.cs) | Background ID string constants |
| [`Keys`](Constant/Keys.cs) | Strongly-typed key action constants |

---

## Installation

1. Install [BepInEx 5.x](https://github.com/BepInEx/BepInEx) for Casualties Unknown.
2. Download the latest [`MossLib.dll`](https://github.com/Black-Moss/Moss-Lib/releases) from the Releases page (or [Nexus Mods](https://www.nexusmods.com/scavprototype/mods/8)).
3. Place `MossLib.dll` into your `BepInEx/plugins/` folder.
4. (Optional) For multiplayer features, install **KrokoshaCasualtiesMP** as a soft dependency.

> **For mod developers:** Add a reference to `MossLib.dll` in your project, and add `BepInDependency("blackmoss.mosslib")` to your plugin class.

---

## Quick Start

### 1. Add BepInEx Dependency

```csharp
[BepInPlugin(Guid, Name, Version)]
[BepInDependency("blackmoss.mosslib")]  // Add this line
public class MyPlugin : BaseUnityPlugin
{
    // ...
}
```

### 2. Set Up Localization

```csharp
// Create a locale class (singleton wrapper)
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

### 3. Register a Custom Command

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
            "Description of my command",
            _ => Log.Info("Command executed!", Plugin.Logger),
            null
        ));
    }
}
```

---

## Localization System

The localization system loads JSON language files from the `Lang/` folder inside your plugin directory.

### File Structure

```
BepInEx/plugins/YourPlugin/
├── Lang/
│   ├── EN.json        # English (fallback)
│   ├── zh-CN.json     # Simplified Chinese
│   └── zh-TW.json     # Traditional Chinese
└── YourPlugin.dll
```

### JSON Format

Language files use nested JSON keys with dot notation for organization:

```json
{
    "welcome": "Welcome!",
    "command": {
        "mycommand": {
            "description": "My custom command",
            "text": "Hello {0}!"
        }
    },
    "tool": {
        "player": {
            "bodynull": "Player body is null"
        }
    }
}
```

### API Methods

| Method | Description |
|---|---|
| [`GetString(key)`](Base/ModLocaleBase.cs:116) | Get localized string by key; falls back to English, then returns `[key]` |
| [`GetStringFormatted(key, args...)`](Base/ModLocaleBase.cs:163) | Get localized string and apply `string.Format()` with arguments |
| [`GetStringArray(key)`](Base/ModLocaleBase.cs:149) | Get a localized string array (JSON array) |
| [`GetStringDictionary(key)`](Base/ModLocaleBase.cs:156) | Get a localized dictionary (JSON object) |
| [`HasKey(key)`](Base/ModLocaleBase.cs:245) | Check if a translation key exists |

### Example

See [`Example/ModLocale.cs`](Example/ModLocale.cs) for a complete singleton implementation.

```csharp
// Initialize in plugin Awake()
MyLocale.Initialize(Logger);

// Usage
string welcome = MyLocale.Get("welcome");                          // "Welcome!"
string message = MyLocale.GetFormat("command.mycommand.text", "World"); // "Hello World!"
```

---

## Command System

The command system allows you to register custom console commands via Harmony patching.

### Base Class: [`ModCommandBase`](Base/ModCommandBase.cs)

| Member | Description |
|---|---|
| [`Initialize(logger, assembly, harmony?)`](Base/ModCommandBase.cs:18) | Initialize with logger and plugin assembly; optionally provide Harmony instance |
| [`LogToConsole(text)`](Base/ModCommandBase.cs:52) | Log text to the in-game console |
| [`ApplicationLogCallback(condition, stackTrace, type)`](Base/ModCommandBase.cs:60) | Callback for Unity log messages with color-coded console output |
| [`Logger`](Base/ModCommandBase.cs:95) | Protected `ManualLogSource` property for subclasses |

### Example

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
            "Says hello",
            args => {
                string name = args.Length > 1 ? args[1] : "World";
                LogToConsole($"Hello, {name}!");
            },
            null
        ));
    }
}
```

See [`Example/ModCommand.cs`](Example/ModCommand.cs) for a complete example.

---

## Language Generator

The language generator auto-creates JSON language files from C# code, eliminating the need to manually write and maintain locale JSON files.

### Base Class: [`ModLangGenBase`](Base/ModLangGenBase.cs)

| Member | Description |
|---|---|
| [`LanguageCode`](Base/ModLangGenBase.cs:13) | Abstract property; return the language code (e.g., `"EN"`, `"zh-CN"`) |
| [`BuildLocaleData()`](Base/ModLangGenBase.cs:32) | Abstract method; call `Add()` to register translations |
| [`Add(key, value)`](Base/ModLangGenBase.cs:34) | Add a translation entry |
| [`Generate(outputDirectory?)`](Base/ModLangGenBase.cs:50) | Generate the JSON file; merges with existing files (doesn't overwrite existing entries) |
| [`Count`](Base/ModLangGenBase.cs:206) | Number of registered translation entries |

### Creating a Generator

```csharp
public class EnLangGenerator : ModLangGenBase
{
    protected override string LanguageCode => "EN";

    protected override void BuildLocaleData()
    {
        Add("welcome", "Welcome!");
        Add("command.hello.description", "Says hello");
        Add("command.hello.text", "Hello, {0}!");
    }
}
```

### Registration & Generation

Use [`LocaleGenerator`](Tool/LocaleGenerator.cs) to register and generate all language files:

| Method | Description |
|---|---|
| [`SetLogger(logger)`](Tool/LocaleGenerator.cs:15) | Set the logger |
| [`Register(generator, logger)`](Tool/LocaleGenerator.cs:20) | Register a language generator |
| [`GenerateAll(outputDirectory?)`](Tool/LocaleGenerator.cs:35) | Generate all registered language files |
| [`GenerateSingle(languageCode, outputDirectory?)`](Tool/LocaleGenerator.cs:77) | Generate a single language file |
| [`PrintInfo()`](Tool/LocaleGenerator.cs:102) | Print info about all registered generators |

```csharp
// In plugin Awake()
LocaleGenerator.SetLogger(Logger);
LocaleGenerator.Register(new EnLangGenerator(), Logger);
LocaleGenerator.Register(new ZhCnLangGenerator(), Logger);
LocaleGenerator.GenerateAll(); // Creates EN.json, zh-CN.json in Lang/ folder
```

> **Note:** The generator **merges** with existing JSON files — it only adds **new** entries, preserving any user modifications to existing translations.

---

## Tools Reference

### Log

[`Log`](Tool/Log.cs) — Enhanced in-game console logging with BepInEx integration.

| Method | Description |
|---|---|
| [`LogToConsole(text)`](Tool/Log.cs:22) | Write a message to the in-game console with timestamp |
| [`Info(text, logger)`](Tool/Log.cs:53) | Log to both in-game console and BepInEx log |
| [`Debug(text, logger)`](Tool/Log.cs:59) | Log debug message with `[DEBUG]` prefix |
| [`Error(text, logger)`](Tool/Log.cs:65) | Log error message with `[ERROR]` prefix |
| [`Warning(text, logger)`](Tool/Log.cs:71) | Log warning message with `[WARNING]` prefix |
| [`Alert(text, logger, important, delay?)`](Tool/Log.cs:77) | Log and show a player alert (screen notification) |
| [`NewLine()`](Tool/Log.cs:42) | Insert a blank line in the console |
| [`Divider(char?, length?)`](Tool/Log.cs:47) | Insert a divider line |

### GameConsole

[`GameConsole`](Tool/Console.cs) — Execute game console commands programmatically.

```csharp
// Execute any in-game console command
GameConsole.RunCommand("some_command arg1 arg2");
```

### World

[`World`](Tool/World.cs) — World manipulation tools.

| Method | Description |
|---|---|
| [`PlaceBlock(x, y, block)`](Tool/World.cs:24) | Place a block at tile coordinates |
| [`PlaceBlock(vector2, block)`](Tool/World.cs:29) | Place a block at a Vector2 position |
| [`PlaceItem(x, y, item)`](Tool/World.cs:42) | Spawn an item at tile coordinates |
| [`PlaceItem(vector2, item)`](Tool/World.cs:47) | Spawn an item at a Vector2 position |
| [`PlaceBackground(pos, backgroundId)`](Tool/World.cs:64) | Place a background tile (accepts Vector2) |
| [`PlaceBackground(pos, backgroundId)`](Tool/World.cs:71) | Place a background tile (accepts Vector2Int) |
| [`CreateTileMesh(pos)`](Tool/World.cs:123) | Create a tiled mesh for background rendering |
| [`CheckForWorld()`](Tool/World.cs:189) | Throw if no world is loaded |
| [`ClearCache()`](Tool/World.cs:195) | Clear sprite/mesh/material caches |

### Player

[`Player`](Tool/Player.cs) — Player manipulation tools.

| Method | Description |
|---|---|
| [`Alert(text, important)`](Tool/Player.cs:12) | Show a screen alert to the player |
| [`Alert(text, important, delay)`](Tool/Player.cs:22) | Show a delayed screen alert |
| [`Tp(vector2)`](Tool/Player.cs:36) | Teleport the player (works in singleplayer and multiplayer) |
| [`Tp(x, y)`](Tool/Player.cs:54) | Teleport to float coordinates |
| [`PickItem(item, slot, force?)`](Tool/Player.cs:59) | Give an item to a specific inventory slot |

```csharp
// Examples
Player.Alert("Watch out!", true);           // Important alert
Player.Tp(100.5f, 200.3f);                  // Teleport
Player.PickItem("rifle", 0, true);          // Give rifle to slot 0
```

### Key

[`Key`](Tool/Key.cs) — Input handling utilities.

| Method | Description |
|---|---|
| [`HasKey(action)`](Tool/Key.cs:9) | Check if a key binding action name exists |
| [`IsKey(action)`](Tool/Key.cs:25) | Check if a key is currently held down |
| [`IsKeyDown(action)`](Tool/Key.cs:31) | Check if a key was pressed this frame |
| [`IsKeyUp(action)`](Tool/Key.cs:37) | Check if a key was released this frame |
| [`MouseWorldPosition()`](Tool/Key.cs:46) | Get the mouse position in world coordinates |
| [`LeftClickPosition()`](Tool/Key.cs:57) | Get left click world position (only valid on click frame) |
| [`RightClickPosition()`](Tool/Key.cs:67) | Get right click world position (only valid on click frame) |

**Coroutine-based waiting methods** — These wait until the player clicks, then return the world position:

| Method | Description |
|---|---|
| [`WaitForLeftClick(Action<Vector2>)`](Tool/Key.cs:79) | Coroutine: wait for left click, invoke callback with position |
| [`WaitForRightClick(Action<Vector2>)`](Tool/Key.cs:90) | Coroutine: wait for right click, invoke callback with position |
| [`WaitForLeftClick()`](Tool/Key.cs:107) | Coroutine: wait for left click, returns `WaitForClickResult` |
| [`WaitForRightClick()`](Tool/Key.cs:123) | Coroutine: wait for right click, returns `WaitForClickResult` |

**`WaitForClickResult`** — A [`CustomYieldInstruction`](https://docs.unity3d.com/ScriptReference/CustomYieldInstruction.html) that yields until the specified click is detected.

**Key action constants** are defined in [`Key.InputAction`](Tool/Key.cs:172):

| Constant | Value | Description |
|---|---|---|
| `InputAction.LeftClick` | `"attack"` | Left mouse click action |
| `InputAction.RightClick` | `"iteminteract"` | Right mouse click action |

```csharp
// Check if left click is pressed this frame
if (Key.IsKeyDown(Key.InputAction.LeftClick))
{
    Vector2 pos = Key.MouseWorldPosition();
    // Do something...
}

// ── Coroutine: wait for player to click ──
IEnumerator MyCoroutine()
{
    // Pattern 1: Callback-based
    yield return Key.WaitForLeftClick(pos => {
        Player.Tp(pos);
    });

    // Pattern 2: Result-based (cleaner)
    var waiter = Key.WaitForRightClick();
    yield return waiter;
    Player.Tp(waiter.Result);
}
```

For a full list of key action names, see [`Constant/Keys`](Constant/Keys.cs).

### Multiplayer

[`Multiplayer`](Tool/Multiplayer.cs) — Multiplayer integration via KrokoshaCasualtiesMP.

| Member | Description |
|---|---|
| [`IsNetworkRunning`](Tool/Multiplayer.cs:25) | Check if the multiplayer network is active |
| [`IsClient`](Tool/Multiplayer.cs:38) | Check if this instance is a client |
| [`Tp(vector2)`](Tool/Multiplayer.cs:51) | Teleport local player in multiplayer |
| [`Tp(x, y)`](Tool/Multiplayer.cs:68) | Teleport to float coordinates |
| [`Tp(playerName, vector2)`](Tool/Multiplayer.cs:73) | Teleport a specific player (use `"@a"` for all players) |
| [`Tp(playerName, x, y)`](Tool/Multiplayer.cs:111) | Teleport a player to float coordinates |
| [`GetPlayerName(player)`](Tool/Multiplayer.cs:249) | Get the name of a player object |

```csharp
// Teleport all players to a location
if (Multiplayer.IsNetworkRunning)
{
    Multiplayer.Tp("@a", new Vector2(50f, 100f));
}
```

### Config

[`Config`](Tool/Config.cs) — BepInEx configuration helpers.

| Method | Description |
|---|---|
| [`ChangeConfig(entry, value)`](Tool/Config.cs:11) | Set a config entry value and save |
| [`SwitchType(configEntry, configName)`](Tool/Config.cs:20) | Toggle a boolean config entry |
| [`SwitchType(configEntry, configName, logger, important)`](Tool/Config.cs:29) | Toggle with optional player alert |

```csharp
// Toggle a config value
ConfigEntry<bool> mySetting = Config.Bind("General", "MySetting", true, "Description");
Config.SwitchType(mySetting, "My Setting");
```

### RichText

[`RichText`](Tool/RichText.cs) — Unity rich text formatting utilities for in-game console messages.

| Method | Description |
|---|---|
| [`Color(text, color)`](Tool/RichText.cs:10) | Wrap text in `<color>` tag by name |
| [`Color(text, color)`](Tool/RichText.cs:18) | Wrap text in `<color>` tag by Unity `Color` |
| [`Hex(text, hex)`](Tool/RichText.cs:23) | Wrap text in `<color>` tag by hex value |
| [`Alpha(text, alphaHex)`](Tool/RichText.cs:44) | Set text alpha by hex value |
| [`Alpha(text, alpha)`](Tool/RichText.cs:54) | Set text alpha by float (0–1) |
| [`Alpha(text, alpha)`](Tool/RichText.cs:61) | Set text alpha by byte (0–255) |
| [`Alpha(text, alpha)`](Tool/RichText.cs:66) | Set text alpha by int (0–255) |
| [`Bold(text)`](Tool/RichText.cs:71) | Wrap text in `<b>` tag |
| [`Italic(text)`](Tool/RichText.cs:77) | Wrap text in `<i>` tag |
| [`Size(text, size)`](Tool/RichText.cs:83) | Wrap text in `<size>` tag |

**Shorthand color methods:**

| Method | Color |
|---|---|
| [`Blue(text)`](Tool/RichText.cs:30) | `blue` |
| [`Red(text)`](Tool/RichText.cs:31) | `red` |
| [`Green(text)`](Tool/RichText.cs:32) | `green` |
| [`Yellow(text)`](Tool/RichText.cs:33) | `yellow` |
| [`White(text)`](Tool/RichText.cs:34) | `white` |
| [`Black(text)`](Tool/RichText.cs:35) | `black` |
| [`Cyan(text)`](Tool/RichText.cs:36) | `cyan` |
| [`Magenta(text)`](Tool/RichText.cs:37) | `magenta` |
| [`Gray(text)`](Tool/RichText.cs:38) | `gray` |
| [`Orange(text)`](Tool/RichText.cs:39) | `orange` |
| [`Purple(text)`](Tool/RichText.cs:40) | `purple` |
| [`Pink(text)`](Tool/RichText.cs:41) | `pink` |
| [`Brown(text)`](Tool/RichText.cs:42) | `brown` |

```csharp
// Format colorful console messages
string msg = RichText.Color("Warning: ", "red") + RichText.Bold("Low health!");
Log.Info(msg, Plugin.Logger);

// With hex color
string fancy = RichText.Hex("Hello", "#FF8800") + " " + RichText.Italic("World");
```

### Tools (Utils)

[`Tools`](Tool/Tools.cs) — Utility methods for argument parsing and validation.

| Method | Description |
|---|---|
| [`CheckArgumentCount(args, desired)`](Tool/Tools.cs:14) | Ensure args array has at least `desired + 1` elements |
| [`ParseFloat(s)`](Tool/Tools.cs:24) | Parse a float with invariant culture; throws on failure |
| [`ParseInt(s)`](Tool/Tools.cs:36) | Parse an int with invariant culture; throws on failure |
| [`TryParseFloat(s, out result)`](Tool/Tools.cs:46) | Try-parse a float without exception |

```csharp
// In a command handler
Tools.CheckArgumentCount(args, 2);          // Requires at least 2 args
float x = Tools.ParseFloat(args[1]);        // Parse float argument
int count = Tools.ParseInt(args[2]);        // Parse int argument
```

---

## Constants Reference

The [`Constant/`](Constant/) namespace provides strongly-typed constants for game objects, making it easier and safer to reference blocks, items, backgrounds, and key bindings without memorizing string IDs.

### Blocks

[`Blocks`](Constant/Blocks.cs) — Strongly-typed block definitions. Implicitly convertible to `ushort`.

| Property | Type | Description |
|---|---|---|
| `Id` | `ushort` | Block ID |
| `LocaleKey` | `string` | Localization key |
| `Health` | `float` | Block hit points |
| `HitSound` | `string` | Sound played when hit |
| `StepSound` | `string` | Sound played when walking on it |
| `SleepQuality` | `string` | Sleep quality rating |
| `IsMetallic` | `bool` | Whether the block is metallic |
| `NoVariation` | `bool` | Whether the block has no visual variation |
| `Toxicity` | `float` | Toxicity level |
| `IsSlippery` | `bool` | Whether the block is slippery |

**Available block constants** (selection):

| Constant | ID | Health |
|---|---|---|
| [`Blocks.Air`](Constant/Blocks.cs:50) | 0 | 0 |
| [`Blocks.LightRock`](Constant/Blocks.cs:51) | 1 | 100 |
| [`Blocks.Gravel`](Constant/Blocks.cs:52) | 2 | 25 |
| [`Blocks.ConcreteTile`](Constant/Blocks.cs:55) | 5 | 800 |
| [`Blocks.SteelTile`](Constant/Blocks.cs:56) | 6 | 5000 |
| [`Blocks.Wood`](Constant/Blocks.cs:61) | 11 | 150 |
| [`Blocks.Granite`](Constant/Blocks.cs:67) | 17 | 200 |
| [`Blocks.Copper`](Constant/Blocks.cs:84) | 34 | 2000 |
| [`Blocks.Ilmenite`](Constant/Blocks.cs:85) | 35 | 4000 |

Use [`Blocks.FromId(ushort)`](Constant/Blocks.cs:87) to resolve a block ID to its constant.

```csharp
// Place blocks using strongly-typed constants
World.PlaceBlock(10, 20, Blocks.Granite);
World.PlaceBlock(new Vector2(15, 25), Blocks.SteelTile);

// Implicit conversion to ushort
ushort blockId = Blocks.Copper; // 34
```

### Items

[`Items`](Constant/Items.cs) — Strongly-typed item definitions. Implicitly convertible to `string`.

| Property | Type | Description |
|---|---|---|
| `Id` | `string` | Item ID |
| `Category` | `string` | Item category (food, medical, tool, etc.) |
| `Weight` | `float` | Item weight |
| `Value` | `int` | Item value |
| `Rec` | `int` | Item rarity/recipe tier |

**Available item constants** (selection):

| Constant | Category |
|---|---|
| [`Items.Rifle`](Constant/Items.cs:228) | `tool` |
| [`Items.Shotgun`](Constant/Items.cs:246) | `tool` |
| [`Items.Bandage`](Constant/Items.cs:52) | `medical` |
| [`Items.Medkit`](Constant/Items.cs:182) | `container` |
| [`Items.Bread`](Constant/Items.cs:72) | `food` |
| [`Items.WaterBottle`](Constant/Items.cs:292) | `water` |
| [`Items.Backpack`](Constant/Items.cs:56) | `container` |
| [`Items.Jetpack`](Constant/Items.cs:152) | `utility` |

Use [`Items.FromId(string)`](Constant/Items.cs:308) to resolve an item ID to its constant.

```csharp
// Give items using strongly-typed constants
Player.PickItem(Items.Rifle, 0, true);
Player.PickItem(Items.Bandage, 1);

// Implicit conversion to string
string itemId = Items.Rifle; // "rifle"
```

### Backgrounds

[`Backgrounds`](Constant/Backgrounds.cs) — String constants for background tile IDs.

| Constant | Value |
|---|---|
| [`Backgrounds.Fungal`](Constant/Backgrounds.cs:5) | `"fungalBackground"` |
| [`Backgrounds.Grass`](Constant/Backgrounds.cs:6) | `"grassBackground"` |
| [`Backgrounds.Ice`](Constant/Backgrounds.cs:7) | `"iceBackground"` |
| [`Backgrounds.Rock`](Constant/Backgrounds.cs:8) | `"rockBackground"` |
| [`Backgrounds.Sand`](Constant/Backgrounds.cs:9) | `"sandBackground"` |
| [`Backgrounds.Soil`](Constant/Backgrounds.cs:10) | `"soilBackground"` |
| [`Backgrounds.Steel`](Constant/Backgrounds.cs:11) | `"steelBackground"` |
| [`Backgrounds.Vents`](Constant/Backgrounds.cs:12) | `"ventsBackground"` |
| [`Backgrounds.Wasteland`](Constant/Backgrounds.cs:13) | `"wastelandBackground"` |

```csharp
// Place backgrounds using constants
World.PlaceBackground(new Vector2Int(10, 20), Backgrounds.Grass);
World.PlaceBackground(new Vector2(15f, 25f), Backgrounds.Sand);
```

### Keys

[`Keys`](Constant/Keys.cs) — Strongly-typed key action constants. Implicitly convertible to `string`.

| Constant | Action Name |
|---|---|
| [`Keys.Jump`](Constant/Keys.cs:22) | `"jump"` |
| [`Keys.Up`](Constant/Keys.cs:23) | `"up"` |
| [`Keys.Left`](Constant/Keys.cs:24) | `"left"` |
| [`Keys.Right`](Constant/Keys.cs:25) | `"right"` |
| [`Keys.Down`](Constant/Keys.cs:26) | `"down"` |
| [`Keys.Attack`](Constant/Keys.cs:36) | `"attack"` |
| [`Keys.ItemInteract`](Constant/Keys.cs:37) | `"iteminteract"` |
| [`Keys.Throw`](Constant/Keys.cs:29) | `"throw"` |
| [`Keys.ToggleInventory`](Constant/Keys.cs:32) | `"toggleinventory"` |
| [`Keys.Pause`](Constant/Keys.cs:40) | `"pause"` |
| [`Keys.Console`](Constant/Keys.cs:41) | `"console"` |
| [`Keys.Craft`](Constant/Keys.cs:43) | `"craft"` |

Use [`Keys.FromAction(string)`](Constant/Keys.cs:45) to resolve an action name to its constant.

```csharp
// Check key bindings using constants
if (Key.IsKeyDown(Keys.Attack))
{
    // Player attacked
}

// Implicit conversion to string
string action = Keys.Jump; // "jump"
```

---

## UI Toolkit (WIP)

The [`Tool/UI/`](Tool/UI/) directory contains in-development UI creation utilities for building in-game mod interfaces. Currently **commented out** and under active development.

| File | Description |
|---|---|
| [`UILayout`](Tool/UI/UILayout.cs) | Canvas management, panel creation, RectTransform helpers |
| [`UIWidgets`](Tool/UI/UIWidgets.cs) | Button and text creation with game-style styling and localization support |

These will be enabled in a future release.

---

## Project Structure

```
MossLib/
├── Plugin.cs                    # Main plugin entry point
├── Base/
│   ├── ModCommandBase.cs        # Command registration base class
│   ├── ModLangGenBase.cs        # Language generator base class
│   └── ModLocaleBase.cs         # Localization base class
├── Constant/
│   ├── Backgrounds.cs           # Background ID constants
│   ├── Blocks.cs                # Strongly-typed block definitions
│   ├── Items.cs                 # Strongly-typed item definitions
│   └── Keys.cs                  # Key action name constants
├── Example/
│   ├── ModCommand.cs            # Example command implementation
│   ├── ModLocale.cs             # Example locale implementation
│   └── Lang/
│       ├── EnLangGenerator.cs   # English language generator
│       ├── ZhCnLangGenerator.cs # Simplified Chinese generator
│       └── ZhTwLangGenerator.cs # Traditional Chinese generator
└── Tool/
    ├── Config.cs                # Config toggling utilities
    ├── Console.cs               # Game console wrapper
    ├── Key.cs                   # Input handling (key binds, mouse click waiting)
    ├── LocaleGenerator.cs       # Language file generator manager
    ├── Log.cs                   # Console logging utilities
    ├── Multiplayer.cs           # Multiplayer integration
    ├── Player.cs                # Player manipulation tools
    ├── RichText.cs              # Rich text formatting utilities
    ├── Tools.cs                 # Argument/parsing utilities
    ├── World.cs                 # World manipulation tools
    └── UI/                      # UI Toolkit (WIP, commented out)
        ├── UILayout.cs          # Canvas and panel creation
        └── UIWidgets.cs         # Button and text creation
```
