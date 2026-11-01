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
        public static ConfigEntry<bool> cfgFadeToggle;
        public static ConfigEntry<float> cfgFadeHoldDuration;
        public static ConfigEntry<float> cfgFadeOutDuration;
        public static void Initialize(ConfigFile config)
        {
            cfgXHeight = config.Bind("Display", "Position X", 810, "");
            cfgYHeight = config.Bind("Display", "Position Y", -200, "");
            cfgScale = config.Bind("Display", "Scale Multiplier", 1f, "");
            cfgTextAlignment = config.Bind("Display", "Text Alignment", TMPro.HorizontalAlignmentOptions.Right, "");
            cfgFadeToggle = config.Bind("Fade", "Enable Fade", false, "");
            cfgFadeHoldDuration = config.Bind("Fade", "Text Hold Duration", 10f, "In seconds, the amount of time that the text is visible before fading out.");
            cfgFadeOutDuration = config.Bind("Fade", "Fade Out Duration", 10f, "In seconds, the duration that text fades out for. This takes place after Text Hold Time.");

            cfgXHeight.SettingChanged += CfgHeight_SettingChanged;
            cfgYHeight.SettingChanged += CfgHeight_SettingChanged;
            cfgScale.SettingChanged += CfgScale_SettingChanged;
            cfgTextAlignment.SettingChanged += CfgTextAlignment_SettingChanged;
            cfgFadeToggle.SettingChanged += CfgFadeConfig_SettingChanged;
            cfgFadeHoldDuration.SettingChanged += CfgFadeConfig_SettingChanged;
            cfgFadeOutDuration.SettingChanged += CfgFadeConfig_SettingChanged;

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModSupport_RiskOfOptions.Initialize();
            }
        }

        private static void CfgFadeConfig_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetFade();
        }

        private static void CfgTextAlignment_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifAlignment();
        }

        private static void CfgScale_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifScale();
        }

        private static void CfgHeight_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetNotifPosition();
        }
    }
}