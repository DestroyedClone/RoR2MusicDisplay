using BepInEx;
using RoR2;
using RoR2.UI;
using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[module: UnverifiableCode]
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace RoR2MusicNotification
{
    [BepInPlugin("com.DestroyedClone.MusicDisplay", "Music Display", "1.0.3")]
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
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/CreditsPanel/HGCreditNameLabel.prefab").Completed += (obj) => { ReferenceGameObject = obj.Result; };
            MusicCatalogDumper.Hook();
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
                MusicNotification_TextFadeOutController component = notif.AddComponent<MusicNotification_TextFadeOutController>();
                MusicNotification_TextFadeOutController.instance = component;
            }
            //notif.AddComponent<FadeOutText>().textMesh = notif.GetComponent<HGTextMeshProUGUI>();

            notifTextMesh = notif.GetComponent<HGTextMeshProUGUI>();
            notifTextMesh.text = GetTrackInfo(currentlyPlaying);
            SetNotifAlignment();
            SetNotifPosition();
            SetNotifScale();
            SetFade();
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
            if (Data.trackLists.TryGetValue(trackDefName, out string[] trackInfo))
            {
                return trackInfo[0] switch
                {
                    "muNone" => string.Empty,
                    _ => $"{trackInfo[0]}\n{trackInfo[1]}"
                };
            }
            return trackDefName;
        }

        internal static void SetFade()
        {
            if (!MusicNotification_TextFadeOutController.instance) return;
            var instance = MusicNotification_TextFadeOutController.instance;
            instance.holdStopwatch = 0;
            instance.fadeOutStopwatch = 0;
            instance.HoldDuration = ModConfig.cfgFadeHoldDuration.Value;
            instance.FadeDuration = ModConfig.cfgFadeOutDuration.Value;
            notifTextMesh.alpha = 1;
            instance.enabled = ModConfig.cfgFadeToggle.Value;
        }

        private class MusicNotification_TextFadeOutController : MonoBehaviour
        {
            public float holdStopwatch = 0;
            public float fadeOutStopwatch = 0;
            public float HoldDuration;
            public float FadeDuration;

            public static MusicNotification_TextFadeOutController instance;

            public void Update()
            {
                holdStopwatch += Time.deltaTime;
                if (holdStopwatch < HoldDuration)
                    return;
                fadeOutStopwatch += Time.deltaTime;
                if (fadeOutStopwatch > FadeDuration)
                    return;
                notifTextMesh.alpha = Mathf.Lerp(1, 0, fadeOutStopwatch / FadeDuration);
            }
        }
    }
}