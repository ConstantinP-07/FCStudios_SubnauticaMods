﻿using ARS_SeaBreezeFCS32.Buildables;
using ARS_SeaBreezeFCS32.Configuration;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using Harmony;
using Oculus.Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ARS_SeaBreezeFCS32
{
    public static class QPatch
    {
        //public static AssetBundle Bundle { get; private set; }
        public static void Patch()
        {
            Console.WriteLine("This is a test");

            // == Load Configuration == //
            string configJson = File.ReadAllText(Mod.ConfigurationFile().Trim());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;

            Configuration = JsonConvert.DeserializeObject<ModConfiguration>(configJson, settings);

            var assembly = Assembly.GetExecutingAssembly();
            QuickLogger.Info("Started patching. Version: " + QuickLogger.GetAssemblyVersion(assembly));

#if DEBUG
            QuickLogger.DebugLogsEnabled = true;
            QuickLogger.Debug("Debug logs enabled");
#endif

            try
            {
                LoadAssetBundle();

                GlobalBundle = FCSTechFabricator.QPatch.Bundle;

                ARSSeaBreezeFCS32Buildable.PatchHelper();

                var harmony = HarmonyInstance.Create("com.arsseabreezefcs32.fcstudios");

                harmony.PatchAll(assembly);

                QuickLogger.Info("Finished patching");
            }
            catch (Exception ex)
            {
                QuickLogger.Error(ex);
            }
        }

        private static void LoadAssetBundle()
        {
            QuickLogger.Debug("GetPrefabs");
            AssetBundle assetBundle = AssetHelper.Asset(Mod.ModName, Mod.BundleName);

            //If the result is null return false.
            if (assetBundle == null)
            {
                QuickLogger.Error($"AssetBundle is Null!");
                throw new FileLoadException();
            }

            Bundle = assetBundle;
        }

        public static AssetBundle Bundle { get; set; }

        public static AssetBundle GlobalBundle { get; set; }
        public static ModConfiguration Configuration { get; private set; }
    }
}

