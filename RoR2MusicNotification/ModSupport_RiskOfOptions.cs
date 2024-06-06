using RiskOfOptions;
using RiskOfOptions.Options;
using System.Runtime.CompilerServices;

namespace RoR2MusicNotification
{
    internal class ModSupport_RiskOfOptions
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void Initialize()
        {
            RiskOfOptions.ModSettingsManager.SetModDescription("Adds a persistent music notification.", "com.DestroyedClone.MusicDisplay", "Music Display");

            ModSettingsManager.AddOption(new IntFieldOption(ModConfig.cfgXHeight, false));
            ModSettingsManager.AddOption(new IntFieldOption(ModConfig.cfgYHeight, false));
            ModSettingsManager.AddOption(new FloatFieldOption(ModConfig.cfgScale, false));
            ModSettingsManager.AddOption(new ChoiceOption(ModConfig.cfgTextAlignment, false));
        }
    }
}