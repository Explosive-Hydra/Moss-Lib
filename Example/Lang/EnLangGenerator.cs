using MossLib.Base;

namespace MossLib.Example.Lang
{
    public class EnLangGenerator : ModLangGenBase
    {
        protected override string LanguageCode => "EN";

        protected override void BuildLocaleData()
        {
            Add("welcome", "Welcome!");

            // Command - TestHello
            Add("command.testhello.description", "Test Hello.");
            Add("command.testhello.text", "Hello World! by Moss Lib! {0}!");

            // Tool - Console
            Add("tool.console.nullorempty", "Command cannot be null or empty");
            Add("tool.console.notinitialized", "ConsoleScript not initialized");

            // Tool - World
            Add("tool.world.checkforworld", "No world is loaded. Try starting a game?");
            Add("tool.world.setblock", "Failed to spawn block {1} at {0}: {2}");
            Add("tool.world.setitem", "Failed to spawn item {1} at {0}: {2}");
            Add("tool.world.setbackground", "Failed to spawn background object {1} at {0}: {2}");
            Add("tool.world.item.nullorempty", "Item cannot be null or empty");
            Add("tool.world.background.nullorempty", "Background object ID cannot be null or empty");

            // Tool - Player
            Add("tool.player.bodynull", "Player body is null");

            // Tool - Multiplayer
            Add("tool.multiplayer.playername.nullorempty", "Player name cannot be null or empty");

            // Tool - Config
            Add("tool.config.switchtype", "{0} has been set to {1}!");
            Add("tool.config.change.isnullorempty", "Unable to get assembly location!");
            Add("tool.config.change.filenotfoundexception", "Configuration file \"{0}\" not found");

            // Tool - Utils
            Add("tool.utils.checkargumentcount", "Expected at least {0} argument {1}, but got {2}.");
            Add("tool.utils.parse.float.invalid", "\"{0}\" is not a valid float value! (2, 0.7, 14.1, etc)");
            Add("tool.utils.parse.int.invalid", "\"{0}\" is not a valid integer value!");
            Add("tool.utils.string.nullorempty", "Input string cannot be null or empty");
        }
    }
}
