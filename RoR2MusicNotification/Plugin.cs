using BepInEx;
using RoR2;
using RoR2.UI;
using System;
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
    [BepInPlugin("com.DestroyedClone.MusicDisplay", "Music Display", "1.0.2")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static MusicController musicController;
        public static GameObject notif;
        public static HGTextMeshProUGUI notifTextMesh;
        public static GameObject ReferenceGameObject;

        public static bool canLoad = false;
        public static string currentlyPlaying = string.Empty;

        public void Awake()
        {
            RoR2MusicNotification.ModConfig.Initialize(Config);

            On.RoR2.MusicTrackDef.Play += MusicTrackDef_Play;
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/HGCreditNameLabel.prefab").Completed += (obj) => { ReferenceGameObject = obj.Result; };
            //MusicCatalogDumper.Hook();
            RoR2Application.onLoad += OnLoad;
        }

        private void OnLoad()
        {
            canLoad = true;
            CreateDisplay();
        }

        private void MusicTrackDef_Play(On.RoR2.MusicTrackDef.orig_Play orig, MusicTrackDef self)
        {
            orig(self);
            currentlyPlaying = self.cachedName;
            CreateDisplay();
        }

        private static void CreateDisplay()
        {
            if (!canLoad) return;
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
            notifTextMesh.text = GetTrackInfo(currentlyPlaying);
            SetNotifAlignment();
            SetNotifPosition();
            SetNotifScale();
        }

        public static void SetNotifAlignment()
        {
            if (!notifTextMesh) return;
            if (!canLoad) return;
            notifTextMesh.horizontalAlignment = ModConfig.cfgTextAlignment.Value;
        }

        public static void SetNotifPosition()
        {
            if (!Plugin.notif) return;
            if (!canLoad) return;
            notif.transform.localPosition = new Vector3(ModConfig.cfgXHeight.Value, ModConfig.cfgYHeight.Value);
        }

        public static void SetNotifScale()
        {
            if (!Plugin.notif) return;
            if (!canLoad) return;
            notif.transform.localScale = Vector3.one * ModConfig.cfgScale.Value;
        }

        public static string GetTrackInfo(string trackDefName)
        {
            if (Data.trackNames.TryGetValue(trackDefName, out var trackName))
            {
                return trackDefName switch
                {
                    "muNone" => string.Empty,
                    "muSong_LemurianTemple" => trackName + "\nStavros Markonis",
                    _ => trackName + "\nChris Christodoulou",
                };
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