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
    [BepInPlugin("com.DestroyedClone.MusicDisplay", "Music Display", "1.0.1")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static MusicController musicController;
        public static GameObject notif;
        public static HGTextMeshProUGUI notifTextMesh;
        public static GameObject ReferenceGameObject;

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
            if (Data.trackNames.TryGetValue(trackDefName, out var trackName))
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