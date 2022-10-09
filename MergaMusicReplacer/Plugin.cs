using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using MergaMusicReplacer;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace MergaMusicReplacer
{
    [BepInPlugin("com.kuborro.plugins.fp2.mergamusic", "MergaMusicReplacer", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<string> audioMerga1;
        public static ConfigEntry<string> audioMerga2;
        public static ConfigEntry<string> audioMerga3;

        public static string texPath = Path.Combine(Path.GetFullPath("."), "mod_overrides");

        public static string FilePathToFileUrl(string filePath)
        {
            StringBuilder uri = new StringBuilder();
            foreach (char v in filePath)
            {
                if ((v >= 'a' && v <= 'z') || (v >= 'A' && v <= 'Z') || (v >= '0' && v <= '9') ||
                  v == '+' || v == '/' || v == ':' || v == '.' || v == '-' || v == '_' || v == '~' ||
                  v > '\xFF')
                {
                    uri.Append(v);
                }
                else if (v == Path.DirectorySeparatorChar || v == Path.AltDirectorySeparatorChar)
                {
                    uri.Append('/');
                }
                else
                {
                    uri.Append(String.Format("%{0:X2}", (int)v));
                }
            }
            if (uri.Length >= 2 && uri[0] == '/' && uri[1] == '/') // UNC path
                uri.Insert(0, "file:");
            else
                uri.Insert(0, "file:///");
            return uri.ToString();
        }

        private void Awake()
        {
            var harmony = new Harmony("com.kuborro.plugins.fp2.mergamusic");

            audioMerga1 = Config.Bind("General", "Phase 1", "M_Boss_Merga1" , "Filename of the track for phase 1 (Blue Moon).");
            audioMerga2 = Config.Bind("General", "Phase 2", "M_Boss_Merga1", "Filename of the track for phase 2 (Blood Moon).");
            audioMerga3 = Config.Bind("General", "Phase 3", "M_Boss_Merga1", "Filename of the track for phase 3 (SuperMoon).");
            
            harmony.PatchAll(typeof(Patch));

        }
    }
}

class Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FPAudio), nameof(FPAudio.PlayMusic), new Type[] { typeof(AudioClip), typeof(float) })]
    static void Prefix(ref AudioClip bgmMusic)
    {
        if (bgmMusic == null) return;
        if (bgmMusic.name == "M_Boss_Merga1")
        {
            string trackPath = Plugin.texPath + "\\" + Plugin.audioMerga1.Value + ".wav";
            WWW audioLoader = new(Plugin.FilePathToFileUrl(trackPath));
            AudioClip selectedClip = audioLoader.GetAudioClip(false, true, AudioType.WAV);
            while (!(selectedClip.loadState == AudioDataLoadState.Loaded))
            {
                int i = 1;
            }
            selectedClip.name = Plugin.audioMerga1.Value + "_patched";
            bgmMusic = selectedClip;
        }
        if (bgmMusic.name == "M_Boss_Merga2")
        {
            string trackPath = Plugin.texPath + "\\" + Plugin.audioMerga2.Value + ".wav";
            WWW audioLoader = new(Plugin.FilePathToFileUrl(trackPath));
            AudioClip selectedClip = audioLoader.GetAudioClip(false, true, AudioType.WAV);
            while (!(selectedClip.loadState == AudioDataLoadState.Loaded))
            {
                int i = 1;
            }
            selectedClip.name = Plugin.audioMerga2.Value;
            bgmMusic = selectedClip;
        }
        if (bgmMusic.name == "M_Boss_Merga3")
        {
            string trackPath = Plugin.texPath + "\\" + Plugin.audioMerga3.Value + ".wav";
            WWW audioLoader = new(Plugin.FilePathToFileUrl(trackPath));
            AudioClip selectedClip = audioLoader.GetAudioClip(false, true, AudioType.WAV);
            while (!(selectedClip.loadState == AudioDataLoadState.Loaded))
            {
                int i = 1;
            }
            selectedClip.name = Plugin.audioMerga3.Value;
            bgmMusic = selectedClip;
        }
    }
}