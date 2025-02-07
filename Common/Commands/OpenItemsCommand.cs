using SquidTestingMod.Helpers;
using SquidTestingMod.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace SquidTestingMod.Common.Commands
{
    public class OpenItemsCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "items";
        public override string Description => "Toggles the items panel";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Main.NewText("Toggling items panel...");

            // get button state
            MainSystem sys = ModContent.GetInstance<MainSystem>();

            // toggle the items panel
            sys?.myState?.itemBrowserButton?.ToggleItemsPanel();
        }
    }
}