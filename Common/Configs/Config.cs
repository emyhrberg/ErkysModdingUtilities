﻿using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Xna.Framework;
using ModHelper.Helpers;
using ModHelper.UI;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ModHelper.Common.Configs
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Reload")]

        [DefaultValue("")]
        public string ModToReload;

        [DefaultValue(false)]
        public bool SaveWorldBeforeReloading;

        [Header("Game")]

        [Range(50f, 80f)]
        [Increment(5f)]
        [DefaultValue(70)]
        public float ButtonSize;

        [DefaultValue(true)]
        public bool ShowDebugText;

        [Header("Improvements")]
        [DefaultValue(false)]
        public bool ImproveMainMenu;

        [DefaultValue(false)]
        public bool ImproveExceptionMenu;

        [Expand(false, false)]
        [Header("ShowButtons")]
        public ShowButtons ShowButtons = new();

        public override void OnChanged()
        {
            base.OnChanged();

            // remove and re-create entire MainState.
            MainSystem sys = ModContent.GetInstance<MainSystem>();
            if (sys != null && sys.mainState != null)
            {
                // Create a new MainState (reset everything)
                sys.OnWorldLoad();
            }
        }
    }

    public class ShowButtons
    {
        [DefaultValue(true)]
        public bool ShowOptionsButton = true;

        [DefaultValue(true)]
        public bool ShowUIButton = true;

        [DefaultValue(true)]
        public bool ShowModsButton = true;

        [DefaultValue(true)]
        public bool ShowReloadButton = true;

        [DefaultValue(true)]
        public bool ShowReloadMPButton = true;
    }

    public static class Conf
    {
        /// <summary>
        /// There is no current way to manually save a mod configuration file in tModLoader.
        // / The method which saves mod config files is private in ConfigManager, so reflection is used to invoke it.
        /// CalamityMod does this great
        /// Reference: 
        // https://github.com/CalamityTeam/CalamityModPublic/blob/1.4.4/CalamityMod.cs#L550
        /// </summary>
        public static void Save()
        {
            try
            {
                MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
                if (saveMethodInfo is not null)
                    saveMethodInfo.Invoke(null, [Conf.C]);
            }
            catch
            {
                Log.Error("An error occurred while manually saving ModConfig!.");
            }
        }

        // Instance of the Config class
        // Use it like Conf.C for easy access to the config values
        public static Config C => ModContent.GetInstance<Config>();
        public static ShowButtons Show => C.ShowButtons;
    }
}
