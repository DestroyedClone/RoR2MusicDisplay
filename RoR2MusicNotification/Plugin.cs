using BepInEx;
using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace RoR2MusicNotification
{
    [BepInPlugin("com.DestroyedClone.MusicDisplay", "Music Display", "1.0.0")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static MusicController musicController;
        public static GameObject notif;
        public static HGTextMeshProUGUI notifTextMesh;
        public static GameObject ReferenceGameObject;

        public static Dictionary<string, string> trackNames = new Dictionary<string, string>()
        {
            {"muBossfightDLC1_10", "Having Fallen, It Was Blood"},
            {"muBossfightDLC1_12", "A Boat Made from a Sheet of Newspaper"},
            {"muEscape", "Escape (Unnamed Track)"},
            {"muFULLSong02", "Into the Doldrums"},
            {"muFULLSong06", "Nocturnal Emission"},
            {"muFULLSong07", "Evapotranspiration"},
            {"muFULLSong18", "Disdrometer"},
            {"muFULLSong19", "Terra Pluviam"},
            {"muGameplayBase_09", "They Might As Well Be Dead"},
            {"muGameplayDLC1_01", "Once in a Lullaby"},
            {"muGameplayDLC1_03", "Out of Whose Womb Came the Ice?"},
            {"muGameplayDLC1_06", "Who Can Fathom the Soundless Depths?"},
            {"muGameplayDLC1_08", "A Placid Island of Ignorance"},
            {"muGameplayDLC2_01", "It Can't Rain All the Time"},
            {"muGameplayDLC2_02", "A Tempestuous Noise of Thunder and Lightning Heard"},
            {"muIntroCutscene", "Through a Cloud, Darkly"},
            {"muLogbook", "The Dehydration of Risk of Rain 2"},
            {"muMainEndingFull", "Lacrimosum"},
            {"muMainEndingOutroA", "Lacrimosum"},
            {"muMainEndingOutroB", "Lacrimosum"},
            {"muMenu", "Risk of Rain 2"},
            {"muMenuDLC1", "Prelude in D flat major"},
            {"muNone", ""},
            {"muRaidfightDLC1_07", "The Face of the Deep"},
            {"muSong04", "Parjanya"},
            {"muSong05", "Thermodynamic Equilibrium"},
            {"muSong08", "A Glacier Eventually Farts (And Don't You Listen to the Song of Life)"},
            {"muSong13", "The Raindrop that Fell to the Sky"},
            {"muSong14", "The Rain Formerly KNown as Purple"},
            {"muSong16", "Hydrophobia"},
            {"muSong21", "Petrichor V"},
            {"muSong22", "Köppen As Fuck"},
            {"muSong23", "Antarctic Oscillation"},
            {"muSong24", "...con lentitud poderosa"},
            {"muSong25", "You're Gonna Need A Bigger Ukulele"},
        };

        public void Awake()
        {
            RoR2MusicNotification.ModConfig.Initialize(Config);

            On.RoR2.MusicTrackDef.Play += MusicTrackDef_Play;
            ReferenceGameObject = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HGCreditNameLabel.prefab").WaitForCompletion();
        }

        private void MusicTrackDef_Play(On.RoR2.MusicTrackDef.orig_Play orig, MusicTrackDef self)
        {
            orig(self);
            //Logger.LogMessage($"Now Playing: {self.cachedName} = {GetTrackInfo(self.cachedName)}");
            if (!notif)
            {
                var userCanvas = AchievementNotificationPanel.GetUserCanvas(LocalUserManager.GetFirstLocalUser());
                if (!userCanvas) return;
                notif = UnityEngine.Object.Instantiate(ReferenceGameObject);
                notif.transform.parent = userCanvas.transform;
                notif.name = "MusicNameDisplay";
            }
            //notif.AddComponent<FadeOutText>().textMesh = notif.GetComponent<HGTextMeshProUGUI>();

            notifTextMesh = notif.GetComponent<HGTextMeshProUGUI>();
            notifTextMesh.text = GetTrackInfo(self.cachedName);
            SetNotifAlignment();
            SetNotifPosition();
            SetNotifScale();
        }

        public static void SetNotifAlignment()
        {
            if (!notifTextMesh) return;
            notifTextMesh.horizontalAlignment = ModConfig.cfgTextAlignment.Value;
        }

        public static void SetNotifPosition()
        {
            if (!Plugin.notif) return;
            notif.transform.localPosition = new Vector3(ModConfig.cfgXHeight.Value, ModConfig.cfgYHeight.Value);
        }

        public static void SetNotifScale()
        {
            if (!Plugin.notif) return;
            notif.transform.localScale = Vector3.one * ModConfig.cfgScale.Value;
        }

        public static string GetTrackInfo(string trackDefName)
        {
            if (trackNames.TryGetValue(trackDefName, out var trackName))
            {
                if (trackName == "muNone")
                    return string.Empty;
                return trackName + "\nChris Christodoulou";
            }
            return trackDefName;
        }

        private class FadeOutText : MonoBehaviour
        {
            public float stopwatch = 0;
            public float fadeDuration = 0;

            public HGTextMeshProUGUI textMesh;

            public void Update()
            {
                stopwatch += Time.deltaTime;
                if (stopwatch > fadeDuration)
                {
                    //Destroy(gameObject);
                }
            }
        }
    }
}