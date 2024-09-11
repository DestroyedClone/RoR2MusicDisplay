using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RoR2MusicNotification
{
    internal class MusicCatalogDumper
    {
        public static void Hook()
        {
            RoR2Application.onLoad += OnLoad2;
            //RoR2Application.onLoad += ItemCatalogDumper.Parse;
        }

        private static void OnLoad2()
        {
            StringBuilder sb = HG.StringBuilderPool.RentStringBuilder();

            foreach (var track in RoR2.MusicTrackCatalog.musicTrackDefs)
            {
                sb.Append($"{Environment.NewLine}\"{track.cachedName}\""); //name
                sb.Append($"TAB\"{track.comment}\""); //comment
                sb.Append($"TAB\"url\""); //url
                sb.Append($"TAB\"author\""); //author
                var scenesWhereThisTrackIsMainTrack = " ";
                var scenesWhereThisTrackIsBossTrack = " ";
                //states
                var states = " ";
                foreach (var state in track.states)
                {
                    states += $"{state} ";
                }
                sb.Append($"TAB\"{states}\"");
                foreach (var scene in SceneCatalog.allSceneDefs)
                {
                    if (scene.mainTrack && scene.mainTrack == track)
                    {
                        scenesWhereThisTrackIsMainTrack += $"[{Language.GetString(scene.nameToken)}] ({scene.cachedName}), ";
                    }
                    if (scene.bossTrack && scene.bossTrack == track)
                    {
                        scenesWhereThisTrackIsBossTrack += $"[{Language.GetString(scene.nameToken)}] ({scene.cachedName}), ";
                    }
                }
                sb.Append($"TAB\"{scenesWhereThisTrackIsMainTrack}\"");
                sb.Append($"TAB\"{scenesWhereThisTrackIsBossTrack}\"");
            }
            UnityEngine.Debug.Log(sb.ToString());
            HG.StringBuilderPool.ReturnStringBuilder(sb);
        }
    }
}
