using RoR2;
using System;
using System.Text;

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

            /* cachedName
             * Actual Name
             * Author
             * Comment
             * URL
             * States
             * Used as main track
             * Used as boss track
             */
            foreach (var track in RoR2.MusicTrackCatalog.musicTrackDefs)
            {
                sb.Append($"{Environment.NewLine}\"{track.cachedName}\""); //cachedName
                if (Data.trackLists.ContainsKey(track.cachedName))
                {
                    sb.Append($"TAB\"{Data.trackLists[track.cachedName][0]}\""); //real name
                } else
                {
                    sb.Append($"TAB\"songname\""); //fallback real name
                }
                if (Data.trackLists.ContainsKey(track.cachedName))
                {
                    sb.Append($"TAB\"{Data.trackLists[track.cachedName][1]}\""); //author
                }
                else
                {
                    sb.Append($"TAB\"author\""); //author
                }
                sb.Append($"TAB\"{track.comment}\""); //comment
                if (SpreadsheetDataHelper.list.ContainsKey(track.cachedName))
                {
                    sb.Append($"TAB\"{SpreadsheetDataHelper.list[track.cachedName]}\""); //url
                } else
                {
                    sb.Append($"TAB\"url\""); //url
                }
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
