using BepInEx;
using BepInEx.Configuration;
using System;

namespace RoR2MusicNotification
{
    internal class ModConfig
    {
        public static ConfigEntry<int> cfgXHeight;
        public static ConfigEntry<int> cfgYHeight;
        public static ConfigEntry<float> cfgScale;
        public static ConfigEntry<Data.TextAlignment> cfgTextAlignment;
        public static ConfigEntry<bool> cfgFadeToggle;
        public static ConfigEntry<float> cfgFadeHoldDuration;
        public static ConfigEntry<float> cfgFadeOutDuration;
        public static ConfigEntry<string> cfgTrackInfoOverrides;
        public static ConfigEntry<string> cfgTrackInfoOverridesSplitString;
        public static ConfigEntry<string> cfgTrackInfoOverridesInnerSplitString;
        public static void Initialize(ConfigFile config)
        {
            cfgXHeight = config.Bind("Display", "Position X", 810, "");
            cfgYHeight = config.Bind("Display", "Position Y", -200, "");
            cfgScale = config.Bind("Display", "Scale Multiplier", 1f, "");
            cfgTextAlignment = config.Bind("Display", "Text Alignment", Data.TextAlignment.Right, "");
            cfgFadeToggle = config.Bind("Fade", "Enable Fade", false, "");
            cfgFadeHoldDuration = config.Bind("Fade", "Text Hold Duration", 10f, "In seconds, the amount of time that the text is visible before fading out.");
            cfgFadeOutDuration = config.Bind("Fade", "Fade Out Duration", 10f, "In seconds, the duration that text fades out for. This takes place after Text Hold Time.");
            cfgTrackInfoOverrides = config.Bind("Extra", "Overrides", "", "Format:" +
                "\nID::NAME::AUTHOR|-|ID2::NAME::AUTHOR ..." +
                "\nExample: muGameplayBase_09::They Might As Well Be Fumo::Chocopuni|-|muBossfightDLC1_10::Eazy Breezy::chelmico");
            cfgTrackInfoOverridesSplitString = config.Bind("Extra", "Overrides String Delimiter", "|-|", "Only change this if it's failing to delimit.");
            cfgTrackInfoOverridesInnerSplitString = config.Bind("Extra", "Overrides Inner String Delimiter", "::", "Only change this if the inner strings are failing to delimit.");

            cfgXHeight.SettingChanged += CfgHeight_SettingChanged;
            cfgYHeight.SettingChanged += CfgHeight_SettingChanged;
            cfgScale.SettingChanged += CfgScale_SettingChanged;
            cfgTextAlignment.SettingChanged += CfgTextAlignment_SettingChanged;
            cfgFadeToggle.SettingChanged += CfgFadeConfig_SettingChanged;
            cfgFadeHoldDuration.SettingChanged += CfgFadeConfig_SettingChanged;
            cfgFadeOutDuration.SettingChanged += CfgFadeConfig_SettingChanged;

            cfgTrackInfoOverrides.SettingChanged += CfgTrackInfoOverrides_SettingChanged;
            cfgTrackInfoOverridesSplitString.SettingChanged += CfgTrackInfoOverrides_SettingChanged;
            cfgTrackInfoOverridesInnerSplitString.SettingChanged += CfgTrackInfoOverrides_SettingChanged;

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModSupport_RiskOfOptions.Initialize();
            }
        }

        private static void CfgTrackInfoOverrides_SettingChanged(object sender, EventArgs e)
        {
            Plugin.SetOverrides();
            Plugin.UpdateText();
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