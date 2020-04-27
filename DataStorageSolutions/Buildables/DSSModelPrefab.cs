﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStorageSolutions.Configuration;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using FCSTechFabricator.Configuration;
using UnityEngine;

namespace DataStorageSolutions.Buildables
{
    internal class DSSModelPrefab
    {
        private static bool _initialized;

        public static GameObject AntennaPrefab { get; private set; }
        internal static GameObject ColorItemPrefab { get; set; }
        internal static GameObject ItemPrefab { get; set; }
        internal static string BodyMaterial => $"{Mod.ModName}_COL";
        internal static string DecalMaterial => $"{Mod.ModName}_COL_Decals";
        internal static string SpecTexture => $"{Mod.ModName}_COL_SPEC";
        internal static string LUMTexture => $"{Mod.ModName}_COL_LUM";
        internal static string ColorIDTexture => $"{Mod.ModName}_COL_ID";
        internal static GameObject TerminalPrefab { get; private set; }
        internal static GameObject FloorMountRackPrefab { get; private set; }
        internal static GameObject ServerFormatStationPrefab { get; private set; }
        internal static GameObject WallMountRackPrefab { get; private set; }
        internal static AssetBundle Bundle { get; set; }
        internal static GameObject ServerPrefab { get; set; }
        internal static GameObject BaseItemPrefab { get; set; }
        internal static GameObject FilterItemPrefab { get; set; }
        public static bool GetPrefabs()
        {
            try
            {
                if (!_initialized)
                {
                    QuickLogger.Debug($"AssetBundle Set");

                    QuickLogger.Debug("GetPrefabs");
                    AssetBundle assetBundle = AssetHelper.Asset(Mod.ModFolderName, Mod.BundleName);

                    Bundle = assetBundle;

                    //We have found the asset bundle and now we are going to continue by looking for the model.
                    GameObject wallMountRackPrefab = assetBundle.LoadAsset<GameObject>(Mod.WallMountedRackPrefabName);
                    GameObject floorMountRackPrefab = assetBundle.LoadAsset<GameObject>(Mod.FloorMountedRackPrefabName);
                    GameObject terminalPrefab = assetBundle.LoadAsset<GameObject>(Mod.TerminalPrefabName);
                    GameObject colorItem = QPatch.GlobalBundle.LoadAsset<GameObject>("ColorItem");
                    GameObject serverModel = assetBundle.LoadAsset<GameObject>(Mod.ServerPrefabName);
                    GameObject baseItem = assetBundle.LoadAsset<GameObject>("DSS_Base_Item");
                    GameObject item = assetBundle.LoadAsset<GameObject>("DSS_Item");
                    GameObject antenna = assetBundle.LoadAsset<GameObject>(Mod.AntennaPrefabName);
                    GameObject formatMachine = assetBundle.LoadAsset<GameObject>(Mod.ServerFormattingStationPrefabName);
                    GameObject filterItemPrefab = assetBundle.LoadAsset<GameObject>("FilterItem");

                    //If the prefab isn't null lets add the shader to the materials
                    if (wallMountRackPrefab != null)
                    {
                        //Lets apply the material shader
                        ApplyShaders(wallMountRackPrefab, assetBundle);

                        WallMountRackPrefab = wallMountRackPrefab;
                        QuickLogger.Debug($"Wall Mount Rack Prefab Found!");
                    }
                    else
                    {
                        QuickLogger.Error($"Wall Mount Rack Prefab Not Found!");
                        return false;
                    }

                    if (floorMountRackPrefab != null)
                    {
                        //Lets apply the material shader
                        ApplyShaders(floorMountRackPrefab, assetBundle);

                        FloorMountRackPrefab = floorMountRackPrefab;

                        QuickLogger.Debug($"Floor Mount Rack Prefab Found!");
                    }
                    else
                    {
                        QuickLogger.Error($"Floor Mount Rack Prefab Not Found!");
                        return false;
                    }

                    if (terminalPrefab != null)
                    {
                        //Lets apply the material shader
                        ApplyShaders(terminalPrefab, assetBundle);

                        TerminalPrefab = terminalPrefab;


                        QuickLogger.Debug($"Terminal Prefab Found!");
                    }
                    else
                    {
                        QuickLogger.Error($"Terminal Prefab Not Found!");
                        return false;
                    }

                    if (colorItem != null)
                    {
                        ColorItemPrefab = colorItem;
                    }
                    else
                    {
                        QuickLogger.Error($"Color Item Not Found!");
                        return false;
                    }
                    
                    if (serverModel != null)
                    {
                        ApplyShaders(serverModel, assetBundle);
                        ServerPrefab = serverModel;
                    }
                    else
                    {
                        QuickLogger.Error($"HydroponicHarvesterBottleModel Not Found!");
                        return false;
                    }
                    
                    if (baseItem != null)
                    {
                        BaseItemPrefab = baseItem;
                    }
                    else
                    {
                        QuickLogger.Error($"DSS_Base_Item Not Found!");
                        return false;
                    }
                    
                    if (item != null)
                    {
                        ItemPrefab =item;
                    }
                    else
                    {
                        QuickLogger.Error($"DSS_Item Not Found!");
                        return false;
                    }

                    if (antenna != null)
                    {
                        ApplyShaders(antenna, assetBundle);
                        AntennaPrefab = antenna;
                    }
                    else
                    {
                        QuickLogger.Error($"Antenna Not Found!");
                        return false;
                    }

                    if (formatMachine != null)
                    {
                        ApplyShaders(formatMachine, assetBundle);
                        ServerFormatStationPrefab = formatMachine;
                    }
                    else
                    {
                        QuickLogger.Error($"Format Machine Not Found!");
                        return false;
                    }

                    if (filterItemPrefab != null)
                    {
                        FilterItemPrefab = filterItemPrefab;
                    }
                    else
                    {
                        QuickLogger.Error($"Filter Item Not Found!");
                        return false;
                    }

                    _initialized = true;
                }

                return true;
            }
            catch (Exception e)
            {
                QuickLogger.Error<DSSModelPrefab>(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Applies the shader to the materials of the reactor
        /// </summary>
        /// <param name="prefab">The prefab to apply shaders.</param>
        internal static void ApplyShaders(GameObject prefab, AssetBundle bundle)
        {
            #region BaseColor
            MaterialHelpers.ApplyColorMaskShader(BodyMaterial, ColorIDTexture, Color.white, DefaultConfigurations.DefaultColor, Color.white, prefab, bundle); //Use color2 
            MaterialHelpers.ApplySpecShader(BodyMaterial, SpecTexture, prefab, 1, 3f, bundle);
            MaterialHelpers.ApplyEmissionShader(BodyMaterial, LUMTexture, prefab, bundle, Color.white);
            MaterialHelpers.ApplyAlphaShader(DecalMaterial, prefab);
            #endregion
        }
    }
}
