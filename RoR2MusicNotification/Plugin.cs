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
    [BepInPlugin("com.DestroyedClone.MusicDisplay", "Music Display", "1.1.0")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static MusicController musicController;
        public static GameObject notif;
        public static HGTextMeshProUGUI notifTextMesh;
        public static GameObject ReferenceGameObject;

        public static bool canLoad = false;
        public static string currentlyPlaying = string.Empty;
        public static bool shouldUseOverride = false;
        public static BepInEx.Logging.ManualLogSource _logger;

        public void Awake()
        {
            _logger = Logger;
            RoR2MusicNotification.ModConfig.Initialize(Config);

            On.RoR2.MusicTrackDef.Play += MusicTrackDef_Play;
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/CreditsPanel/HGCreditNameLabel.prefab").Completed += (obj) => { ReferenceGameObject = obj.Result; };
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
                MusicNotification_TextFadeOutController component = notif.AddComponent<MusicNotification_TextFadeOutController>();
                MusicNotification_TextFadeOutController.instance = component;
            }
            //notif.AddComponent<FadeOutText>().textMesh = notif.GetComponent<HGTextMeshProUGUI>();

            notifTextMesh = notif.GetComponent<HGTextMeshProUGUI>();
            SetOverrides();
            UpdateText();
            SetNotifAlignment();
            SetNotifPosition();
            SetNotifScale();
            SetFade();
        }

        public static void SetNotifAlignment()
        {
            if (!notifTextMesh) return;
            if (!canLoad) return;
            switch (ModConfig.cfgTextAlignment.Value)
            {
                case Data.TextAlignment.Left:
                    notifTextMesh.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Left;
                    break;
                case Data.TextAlignment.Center:
                    notifTextMesh.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                    break;
                case Data.TextAlignment.Right:
                    notifTextMesh.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Right;
                    break;
            }
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
            if (shouldUseOverride)
            {
                if (Data.trackListOverrides.TryGetValue(trackDefName, out var trackInfoOverride))
                {
                    return $"{trackInfoOverride[0]}\n{trackInfoOverride[1]}";
                }
            }
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

        public static void SetOverrides()
        {            // ID::NAME::AUTHOR|-|
            Data.trackListOverrides.Clear();
            int addedTrackCount = 0;
            // this might be a bit more prone to user error so I'm wrapping a try catch block here
            if (ModConfig.cfgTrackInfoOverrides.Value.IsNullOrWhiteSpace())
                goto LeaveEarly;
            try
            {
                var trackInfoList = ModConfig.cfgTrackInfoOverrides.Value.Split(ModConfig.cfgTrackInfoOverridesSplitString.Value);
                if (trackInfoList.Length == 0)
                {
                    _logger.LogWarning($"No entries found for track overrides.");
                    goto LeaveEarly;
                }
                foreach (var trackInfo in trackInfoList)
                {
                    if (trackInfo.IsNullOrWhiteSpace()) continue;
                    var trackComponents = trackInfo.Split(ModConfig.cfgTrackInfoOverridesInnerSplitString.Value);
                    switch (trackComponents.Length)
                    {
                        case 0:
                            _logger.LogWarning($"Track information for an entry is empty");
                            _logger.LogWarning($"Provided value: \"{ModConfig.cfgTrackInfoOverridesInnerSplitString.Value}\"");
                            break;
                        case 1:
                            _logger.LogWarning($"Track information for \"{trackComponents[0]}\" is missing a track name and author.");
                            _logger.LogWarning($"Provided values: trackId \"{trackComponents[0]}\"");
                            break;
                        case 2:
                            _logger.LogWarning($"Track information for \"{trackComponents[0]}\" is missing an author.");
                            _logger.LogWarning($"Provided values: trackId \"{trackComponents[0]}\", trackName:\"{trackComponents[1]}\"");
                            break;
                        default:
                            if (Data.trackListOverrides.ContainsKey(trackComponents[0]))
                            {
                                _logger.LogWarning($"Track information already exists for \"{trackComponents[0]}\" (\"{trackComponents[1]}\" by \"{trackComponents[2]}\")");
                                _logger.LogWarning($"Skipping new entry \"{trackComponents[1]}\" and author \"{trackComponents[2]}\".");
                                continue;
                            }
                            Data.trackListOverrides.Add(trackComponents[0], new string[] { trackComponents[1], trackComponents[2] });
                            _logger.LogMessage($"Added \"{trackComponents[0]}\" with trackname \"{trackComponents[1]}\" and author \"{trackComponents[2]}\"");
                            addedTrackCount++;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong while parsing the override strings.");
                _logger.LogError(ex);
            }
        LeaveEarly:
            if (addedTrackCount < 1)
            {
                Data.trackListOverrides.Clear();
                _logger.LogMessage("No overrides found, skipping.");
            } else
            {
                _logger.LogMessage($"Added {addedTrackCount} track overrides.");
            }
            Plugin.shouldUseOverride = Data.trackListOverrides.Count > 0;
        }

        internal static void UpdateText()
        {
            notifTextMesh.text = GetTrackInfo(currentlyPlaying);
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
                notifTextMesh.alpha = Mathf.Lerp(1, 0, fadeOutStopwatch / FadeDuration);
                if (fadeOutStopwatch > FadeDuration)
                    this.enabled = false;
            }
        }
    }
}