using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using MergaMusicReplacer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace MergaMusicReplacer
{
    [BepInPlugin("com.kuborro.plugins.fp2.mergamusic", "MergaMusicReplacer", "1.2.3")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<string> audioMerga1;
        public static ConfigEntry<string> audioMerga2;
        public static ConfigEntry<string> audioMerga3;

        public static string texPath = Path.Combine(Path.GetFullPath("."), "mod_overrides");

        public static string FilePathToFileUrl(string filePath)
        {
            StringBuilder uri = new();
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
        public static AudioType GetAudioType(string extension)
        {
            if (extension == ".wav") return AudioType.WAV;
            if (extension == ".ogg") return AudioType.OGGVORBIS;
            if (extension == ".s3m") return AudioType.S3M;
            if (extension == ".mod") return AudioType.MOD;
            if (extension == ".it") return AudioType.IT;
            if (extension == ".xm") return AudioType.XM;
            if (extension == ".aiff" || extension == ".aif") return AudioType.AIFF;
            return AudioType.UNKNOWN;
        }
        private void Awake()
        {
            var harmony = new Harmony("com.kuborro.plugins.fp2.mergamusic");

            audioMerga1 = Config.Bind("General", "Phase 1", "M_Boss_Merga1.wav", "Filename of the track for phase 1 (Blue/Blood/Super Moon).");
            audioMerga2 = Config.Bind("General", "Phase 2", "M_Boss_Merga2.wav", "Filename of the track for phase 2 (Eclipse/Lilith).");
            audioMerga3 = Config.Bind("General", "Phase 3", "M_Boss_Merga3.wav", "Filename of the track for phase 3 (Final).");

            harmony.PatchAll(typeof(Patch));
            harmony.PatchAll(typeof(Patch2));

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
            string trackPath = Plugin.texPath + "\\" + Plugin.audioMerga1.Value;
            string ext = Path.GetExtension(trackPath);
            bool stream = true;
            if (ext == "")
            {
                ext = ".wav";
                trackPath += ext;
            }
            if (ext is ".mod" or ".s3m" or ".it" or ".xm")
            {
                stream = false;
            }
            if (File.Exists(trackPath) && Plugin.GetAudioType(ext) != AudioType.UNKNOWN)
            {
                WWW audioLoader = new(Plugin.FilePathToFileUrl(trackPath));
                AudioClip selectedClip = audioLoader.GetAudioClip(false, stream, Plugin.GetAudioType(ext));
                while (!(selectedClip.loadState == AudioDataLoadState.Loaded))
                {
                    int i = 1; //we gotta wait for the file to load this is why this is here lol.
                }
                selectedClip.name = bgmMusic.name;
                bgmMusic = selectedClip;
            }
        }
        if (bgmMusic.name == "M_Boss_Merga2")
        {
            string trackPath = Plugin.texPath + "\\" + Plugin.audioMerga2.Value;
            string ext = Path.GetExtension(trackPath);
            bool stream = true;
            if (ext == "")
            {
                ext = ".wav";
                trackPath += ext;
            }
            if (ext is ".mod" or ".s3m" or ".it" or ".xm")
            {
                stream = false;
            }
            if (File.Exists(trackPath) && Plugin.GetAudioType(ext) != AudioType.UNKNOWN)
            {
                WWW audioLoader = new(Plugin.FilePathToFileUrl(trackPath));
                AudioClip selectedClip = audioLoader.GetAudioClip(false, stream, Plugin.GetAudioType(ext));
                while (!(selectedClip.loadState == AudioDataLoadState.Loaded))
                {
                    int i = 1;
                }
                selectedClip.name = bgmMusic.name;
                bgmMusic = selectedClip;
            }
        }
        if (bgmMusic.name == "M_Boss_Merga3")
        {
            string trackPath = Plugin.texPath + "\\" + Plugin.audioMerga3.Value;
            string ext = Path.GetExtension(trackPath);
            bool stream = true;
            if (ext == "")
            {
                ext = ".wav";
                trackPath += ext;
            }
            if (ext is ".mod" or ".s3m" or ".it" or ".xm")
            {
                stream = false;
            }
            if (File.Exists(trackPath) && Plugin.GetAudioType(ext) != AudioType.UNKNOWN)
            {
                WWW audioLoader = new(Plugin.FilePathToFileUrl(trackPath));
                AudioClip selectedClip = audioLoader.GetAudioClip(false, stream, Plugin.GetAudioType(ext));
                while (!(selectedClip.loadState == AudioDataLoadState.Loaded))
                {
                    int i = 1;
                }
                selectedClip.name = bgmMusic.name;
                bgmMusic = selectedClip;
            }
        }
    }
}

class Patch2
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(MergaBlueMoon), "Activate", MethodType.Normal)]
    [HarmonyPatch(typeof(MergaBloodMoon), "Activate", MethodType.Normal)]
    [HarmonyPatch(typeof(MergaSupermoon), "Activate", MethodType.Normal)]
    [HarmonyPatch(typeof(MergaLilith), "Activate", MethodType.Normal)]
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = new(instructions);
        for (int i = 5; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Brfalse && codes[i - 1].opcode == OpCodes.Call && codes[i - 2].opcode == OpCodes.Ldfld && codes[i - 3].opcode == OpCodes.Ldarg_0 && codes[i - 4].opcode == OpCodes.Call)
            {
                codes[i - 3].opcode = OpCodes.Nop; //Ldarg.0
                codes[i - 2].opcode = OpCodes.Nop; //Ldfld
                codes[i - 1].opcode = OpCodes.Nop; //Call
                codes[i].opcode = OpCodes.Brtrue; //BrFalse
                break;
            };
        }
        return codes;
    }
}