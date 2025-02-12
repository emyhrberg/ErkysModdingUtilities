﻿using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SquidTestingMod.Common.Systems;
using SquidTestingMod.UI;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SquidTestingMod.Common.Configs
{
    public class Config : ModConfig
    {
        // CLIENT SIDE
        public override ConfigScope Mode => ConfigScope.ClientSide;

        // ACTUAL CONFIG
        [Header("Reload")]
        public ReloadConfig Reload = new();

        [Header("General")]
        public GeneralConfig General = new();

        [Header("Gameplay")]
        public GameplayConfig Gameplay = new();

        [Header("ItemBrowser")]
        public ItemBrowserConfig ItemBrowser = new();

        [Header("DisableButtons")]
        public DisableButtonConfig DisableButton = new();

        public class ReloadConfig
        {
            [OptionStrings(["Disabled", "Singleplayer", "Multiplayer"])]
            [DefaultValue("Disabled")]
            [DrawTicks]
            public string AutoloadWorld = "Disabled";

            [DefaultValue(true)]
            public bool SaveAndQuitWorldWithoutSaving;

            [DefaultValue(true)]
            public bool InvokeBuildAndReload;

            [DefaultValue("SquidTestingMod")]
            public string ModToReload;

            [DefaultValue(100)]
            [Range(100, 5000)]
            public int WaitingTimeBeforeNavigatingToModSources;

            [DefaultValue(100)]
            [Range(100, 5000)]
            public int WaitingTimeBeforeBuildAndReload;

            [DefaultValue(false)]
            public bool AttemptToKillServer;

            [DefaultValue("MyWorld")]
            public string WorldToLoad;
        }

        public class GeneralConfig
        {
            [DefaultValue(false)]
            public bool OnlyShowWhenInventoryOpen;

            [DefaultValue(true)]
            public bool HideButtonText;

            [DefaultValue(true)]
            public bool HideButtonTooltips;

            [Range(0.3f, 1f)]
            [Increment(0.1f)]
            [DrawTicks]
            [DefaultValue(0.7f)]
            public float ButtonSize;
        }

        public class GameplayConfig
        {
            [DefaultValue(false)]
            [ReloadRequired] // this is an IL hook (edited at loadtime), so it requires a reload
            public bool KeepGameRunningWhenFocusLost;

            [DefaultValue(false)]
            public bool AlwaysSpawnBossOnTopOfPlayer;

            [DefaultValue(false)]
            public bool StartInGodMode;

            [OptionStrings(["Disabled", "Small", "Big"])]
            [DefaultValue("Small")]
            [DrawTicks]
            public string GodModeOutlineSize = "Small";
        }

        public class ItemBrowserConfig
        {
            [DefaultValue(100)]
            [Range(0, 10000)]
            public int MaxItemsToDisplay = 1000;

            [DefaultValue(1)]
            [Range(1, 19)]
            public int ItemSlotStyle = 1;

            [DefaultValue(typeof(Color), "255, 0, 0, 255"), ColorHSLSlider(false), ColorNoAlpha]
            public Color ItemSlotColor = new(255, 0, 0, 255);
        }

        public class DisableButtonConfig
        {
            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableConfig;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableReload;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableItemBrowser;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableNPCBrowser;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableGod;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableFast;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableEnemies;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableTime;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableHitboxes;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableUIHitboxes;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableTeleport;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableLog;

            [DefaultValue(false)]
            [ReloadRequired]
            public bool DisableSecondClient;
        }

        public override void OnChanged()
        {
            ChangeGodModeOutline();
            ChangeButtonTextVisibility();
            ChangeButtonSizes();
            // ChangeFocusSystem();
        }

        // NOT WORKING TO EDIT IL HOOKS ON RUNTIME
        // private void ChangeFocusSystem()
        // {
        //     FocusSystem focusSystem = ModContent.GetInstance<FocusSystem>();
        //     if (focusSystem == null)
        //         return;
        //     IL_Main.DoUpdate -= focusSystem.GameUpdate;
        // }

        private static void ChangeButtonSizes()
        {
            MainSystem sys = ModContent.GetInstance<MainSystem>();
            sys?.mainState?.UpdateAllButtonsTexture();
        }

        private static void ChangeButtonTextVisibility()
        {
            MainSystem sys = ModContent.GetInstance<MainSystem>();
            sys?.mainState?.UpdateAllButtonsTexture();
        }

        private void ChangeGodModeOutline()
        {
            // add null check for the class itself in case it's called before the mod is loaded
            // idk
            if (ModContent.GetInstance<Config>() == null)
                return;

            int type = ModContent.ItemType<BorderShaderDye>();

            if (Gameplay.GodModeOutlineSize == "Small")
            {
                Asset<Effect> smallOutlineEffect = Mod.Assets.Request<Effect>("Effects/LessOutlineEffect");
                GameShaders.Armor.BindShader(type, new ArmorShaderData(smallOutlineEffect, "Pass0"));
            }
            else if (Gameplay.GodModeOutlineSize == "Big")
            {
                Asset<Effect> bigOutlineEffect = Mod.Assets.Request<Effect>("Effects/OutlineEffect");
                GameShaders.Armor.BindShader(type, new ArmorShaderData(bigOutlineEffect, "Pass0"));
            }
        }
    }
}
