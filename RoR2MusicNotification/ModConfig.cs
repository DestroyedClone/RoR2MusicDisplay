using BepInEx.Configuration;
using System;

namespace RoR2MusicNotification
{
    internal class ModConfig
    {
        public static ConfigEntry<int> cfgXHeight;
        public static ConfigEntry<int> cfgYHeight;
        public static ConfigEntry<float> cfgScale;
        public static ConfigEntry<TMPro.HorizontalAlignmentOptions> cfgTextAlignment;

        public static void Initialize(ConfigFile config)
        {
            cfgXHeight = config.Bind("Display", "Position X", 810, "");
            cfgYHeight = config.Bind("Display", "Position Y", -200, "");
            cfgScale = config.Bind("Display", "Scale Multiplier", 1f, "");
            cfgTextAlignment = config.Bind("Display", "Text Alignment", TMPro.HorizontalAlignmentOptions.Right, "");

            cfgXHeight.SettingChanged += CfgXHeight_SettingChanged;
            cfgYHeight.SettingChanged += CfgYHeight_SettingChanged;
            cfgScale.SettingChanged += CfgScale_SettingChanged;
            cfgTextAlignment.SettingChanged += CfgTextAlignment_SettingChanged;

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModSupport_RiskOfOptions.Initialize();
            }
        }

        private static void CfgTextAlignment_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifAlignment();
        }

        private static void CfgScale_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifScale();
        }

        private static void CfgYHeight_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifPosition();
        }

        private static void CfgXHeight_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifPosition();
        }
    }
}