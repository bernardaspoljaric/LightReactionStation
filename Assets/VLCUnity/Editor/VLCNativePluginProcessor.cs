#if UNITY_2018_1_OR_NEWER
#define UNITY_SUPPORTS_BUILD_REPORT
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditor.Build;
#if UNITY_SUPPORTS_BUILD_REPORT
using UnityEditor.Build.Reporting;
#endif

namespace Videolabs.VLCUnity.Editor
{
    public class VLCNativePluginProcessor :
#if UNITY_SUPPORTS_BUILD_REPORT
        IPreprocessBuildWithReport
#else
        IPreprocessBuild
#endif
    {
        public int callbackOrder { get { return 0; } }

        const string SDK = "sdk";
        string[] UWP_ARCH = { "x86_64", "ARM64" };

        const string UWP_PATH = "VLCUnity/Plugins/WSA/UWP";
        const string WINDOWS_PATH = "VLCUnity/Plugins/Windows/x86_64";
        const string ANDROID_PATH = "VLCUnity/Plugins/Android/libs";

#if UNITY_SUPPORTS_BUILD_REPORT
        public void OnPreprocessBuild(BuildReport report)
        {
            OnPreprocessBuild(report.summary.platform, report.summary.outputPath);
        }
#endif

        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            switch(target)
            {
                case BuildTarget.WSAPlayer:
                    ConfigureUWPNativePlugins();
                    break;
                case BuildTarget.StandaloneWindows64:
                    ConfigureWindowsNativePlugins();
                    break;
                case BuildTarget.Android:
                    ConfigureAndroidNativePlugins();
                    break;
            }
        }

        void ConfigureWindowsNativePlugins()
        {
            PluginImporter[] importers = PluginImporter.GetAllImporters();
            foreach (PluginImporter pi in importers)
            {
                if(!pi.isNativePlugin) continue;

                if(!pi.assetPath.Contains(WINDOWS_PATH)) continue;
                // pi.ClearSettings();

                var dirty = false;

                if(pi.GetCompatibleWithAnyPlatform() || !pi.GetCompatibleWithEditor() || !pi.GetCompatibleWithPlatform(BuildTarget.StandaloneWindows64))
                {
                    pi.SetCompatibleWithAnyPlatform(false);
                    pi.SetCompatibleWithEditor(true);
                    pi.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);

                    dirty = true;
                }
                
                var cpu = pi.GetPlatformData(BuildTarget.StandaloneWindows64, "CPU");
                if(cpu != "x86_64")
                {
                    pi.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "x86_64");
                    dirty = true;
                }

                if(dirty)
                {
                    pi.SaveAndReimport();
                }
            }
        }

        void ConfigureUWPNativePlugins()
        {
            PluginImporter[] importers = PluginImporter.GetAllImporters();
            foreach (PluginImporter pi in importers)
            {
                if(!pi.isNativePlugin) continue;

                if(!pi.assetPath.Contains(UWP_PATH)) continue;

                var isX64 = pi.assetPath.Contains($"{UWP_PATH}/{UWP_ARCH[0]}");
                var sdkPath = isX64 ? $"{UWP_PATH}/{UWP_ARCH[0]}/{SDK}" : $"{UWP_PATH}/{UWP_ARCH[1]}/{SDK}";

                if(pi.assetPath.Contains(sdkPath)) continue;

                // pi.ClearSettings();
                var dirty = false;
                if(pi.GetCompatibleWithAnyPlatform() || pi.GetCompatibleWithEditor() || !pi.GetCompatibleWithPlatform(BuildTarget.WSAPlayer))
                {
                    pi.SetCompatibleWithAnyPlatform(false);
                    pi.SetCompatibleWithEditor(false);
                    pi.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, true);

                    dirty = true;
                }

                var cpu = pi.GetPlatformData(BuildTarget.WSAPlayer, "CPU");

                if(isX64)
                {
                    if(cpu != "X64")
                    {
                        pi.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "X64");
                        dirty = true;
                    }
                }
                else
                {
                    if(cpu != "ARM64")
                    {
                        pi.SetPlatformData(BuildTarget.WSAPlayer, "CPU", "ARM64");
                        dirty = true;
                    }
                }

                if(dirty)
                {
                    pi.SaveAndReimport();
                }
            }
        }

        void ConfigureAndroidNativePlugins()
        {
            PluginImporter[] importers = PluginImporter.GetAllImporters();
            foreach (PluginImporter pi in importers)
            {
                if(!pi.isNativePlugin) continue;

                if(!pi.assetPath.Contains(ANDROID_PATH)) continue;

                var dirty = false;
                if(pi.GetCompatibleWithAnyPlatform() || pi.GetCompatibleWithEditor() || !pi.GetCompatibleWithPlatform(BuildTarget.Android))
                {
                    pi.SetCompatibleWithAnyPlatform(false);
                    pi.SetCompatibleWithEditor(false);
                    pi.SetCompatibleWithPlatform(BuildTarget.Android, true);

                    dirty = true;
                }

                if(pi.assetPath.Contains($"{ANDROID_PATH}/armeabi-v7a/"))
                {
                    if(pi.GetPlatformData(BuildTarget.Android, "CPU") != "ARMv7")
                    {
                        pi.SetPlatformData(BuildTarget.Android, "CPU", "ARMv7");
                        dirty = true;
                    }
                }
                else if(pi.assetPath.Contains($"{ANDROID_PATH}/arm64-v8a/"))
                {
                    if(pi.GetPlatformData(BuildTarget.Android, "CPU") != "ARM64")
                    {
                        pi.SetPlatformData(BuildTarget.Android, "CPU", "ARM64");
                        dirty = true;
                    }
                }
                else if(pi.assetPath.Contains($"{ANDROID_PATH}/x86/"))
                {
                    if(pi.GetPlatformData(BuildTarget.Android, "CPU") != "X86")
                    {
                        pi.SetPlatformData(BuildTarget.Android, "CPU", "X86");
                        dirty = true;
                    }
                }
                else if(pi.assetPath.Contains($"{ANDROID_PATH}/x86_64/"))
                {
                    if(pi.GetPlatformData(BuildTarget.Android, "CPU") != "X86_64")
                    {
                        pi.SetPlatformData(BuildTarget.Android, "CPU", "X86_64");
                        dirty = true;
                    }
                }

                if(dirty)
                {
                    pi.SaveAndReimport();
                }
            }
        }
    }
}