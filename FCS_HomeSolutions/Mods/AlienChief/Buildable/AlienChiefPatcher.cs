﻿using System;
using System.Collections.Generic;
using System.IO;
using FCS_AlterraHub.Enumerators;
using FCS_AlterraHub.Registration;
using FCS_AlterraHub.Spawnables;
using FCS_HomeSolutions.Buildables;
using FCS_HomeSolutions.Configuration;
using FCS_HomeSolutions.ModManagers;
using FCS_HomeSolutions.Mods.AlienChief.Mono;
using FCSCommon.Extensions;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace FCS_HomeSolutions.Mods.AlienChief.Buildables
{
    internal partial class AlienChiefBuildable : Buildable
    {
        public AlienChiefBuildable() : base(Mod.AlienChiefClassID, Mod.AlienChiefFriendly, Mod.AlienChiefDescription)
        {

            OnStartedPatching += () =>
            {
                var miniFountainKit = new FCSKit(Mod.AlienChiefKitClassID, FriendlyName, Path.Combine(AssetsFolder, $"{ClassID}.png"));
                miniFountainKit.Patch();
            };
            OnFinishedPatching += () =>
            {
                FCSAlterraHubService.PublicAPI.CreateStoreEntry(TechType, Mod.AlienChiefKitClassID.ToTechType(), 30000, StoreCategory.Home);
                FCSAlterraHubService.PublicAPI.RegisterPatchedMod(ClassID);
            };
        }

        public override GameObject GetGameObject()
        {
            try
            {
                var prefab = GameObject.Instantiate(ModelPrefab.AlienChiefPrefab);

                prefab.name = this.PrefabFileName;
                
                var center = new Vector3(0f, 1.467808f, 0f);
                var size = new Vector3(2.108935f, 2.653688f, 1f);

                GameObjectHelpers.AddConstructableBounds(prefab,size,center);

                var model = prefab.FindChild("model");

                SkyApplier skyApplier = prefab.AddComponent<SkyApplier>();
                skyApplier.renderers = model.GetComponentsInChildren<MeshRenderer>();
                skyApplier.anchorSky = Skies.Auto;

                //========== Allows the building animation and material colors ==========// 

                QuickLogger.Debug("Adding Constructible");

                // Add constructible
                var constructable = prefab.AddComponent<Constructable>();
                constructable.allowedOnWall = false;
                constructable.allowedOnGround = true;
                constructable.allowedInSub = true;
                constructable.allowedInBase = true;
                constructable.allowedOnCeiling = false;
                constructable.allowedOutside = false;
                constructable.model = model;
                constructable.techType = TechType;

                PrefabIdentifier prefabID = prefab.AddComponent<PrefabIdentifier>();
                prefabID.ClassId = ClassID;

                prefab.AddComponent<TechTag>().type = TechType;
                prefab.AddComponent<AlienChiefController>();

                //Apply the glass shader here because of autosort lockers for some reason doesnt like it.
                MaterialHelpers.ApplyGlassShaderTemplate(prefab, "_glass", Mod.ModName);

                return prefab;
            }
            catch (Exception e)
            {
                QuickLogger.Error(e.Message);
                return null;
            }
        }

        public override string AssetsFolder { get; } = Mod.GetAssetPath();

#if SUBNAUTICA
        protected override TechData GetBlueprintRecipe()
        {
            QuickLogger.Debug($"Creating recipe...");
            // Create and associate recipe to the new TechType
            var customFabRecipe = new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(Mod.AlienChiefKitClassID.ToTechType(), 1)
                }
            };
            return customFabRecipe;
        }
#elif BELOWZERO

        protected override RecipeData GetBlueprintRecipe()
        {
            QuickLogger.Debug($"Creating recipe...");
            // Create and associate recipe to the new TechType
            var customFabRecipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient(Mod.AlienChiefKitClassID.ToTechType(), 1)
                }
            };
            return customFabRecipe;
        }
#endif

        public override TechGroup GroupForPDA { get; } = TechGroup.InteriorModules;
        public override TechCategory CategoryForPDA { get; } = TechCategory.InteriorModule;
    }
}
