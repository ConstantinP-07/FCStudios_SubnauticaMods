﻿using System;
using FCS_AlterraHomeSolutions.Mono.PaintTool;
using FCS_AlterraHub.Helpers;
using FCS_AlterraHub.Mono;
using FCS_AlterraHub.Objects;
using FCSCommon.Helpers;
using FCSCommon.Utilities;
using FCSDemo;
using FCSDemo.Buildables;
using FCSDemo.Configuration;
using Model;
using UnityEngine;

namespace Mono
{
    internal class FCSDemoController : FcsDevice,IHandTarget
    {
        private bool _runStartUpOnEnable;
        private FCSAquarium _fcsAquarium;

        public string Name => gameObject.name;
        public override bool IsInitialized { get; set; }

        private void OnEnable()
        {
            if (_runStartUpOnEnable)
            {
                if (!IsInitialized)
                {
                    Initialize();
                }
                _runStartUpOnEnable = false;
            }
        }
        
        public override void Initialize()
        {
            QuickLogger.Info("Initializing",true);
            
            if (_colorManager == null)
            {
                QuickLogger.Info($"Creating Color Component", true);
                _colorManager = gameObject.AddComponent<ColorManager>();
                _colorManager.Initialize(gameObject,FCSDemoModel.BodyMaterial,FCSDemoModel.SecondaryMaterial,FCSDemoModel.EmissiveControllerMaterial);

                if (QPatch.Configuration.ControlEmissionStrength)
                {
                    MaterialHelpers.ChangeEmissionStrength(FCSDemoModel.EmissiveControllerMaterial, gameObject, QPatch.Configuration.EmissionStrength);
                }
            }

            //if (_fcsAquarium == null && QPatch.Configuration.HasAquarium)
            //{
            //    var storageRoot = GameObjectHelpers.FindGameObject(gameObject, "StorageRoot");
            //    _fcsAquarium = gameObject.AddComponent<FCSAquarium>();
            //    _fcsAquarium.Initialize(storageRoot, new[] { TechType.Peeper, TechType.GarryFish, TechType.Bladderfish, TechType.HoleFish });
            //}

            //if (QPatch.Configuration.AddPlants)
            //{
            //    var plantSpawns = GameObjectHelpers.FindGameObject(gameObject, "spawn_pnt");

            //    if (plantSpawns != null)
            //    {
            //        foreach (Transform plantGo in plantSpawns.transform)
            //        {
            //            var name = plantGo.gameObject.name;

            //            if (SpawnHelper.ContainsPlant(name))
            //            {
            //                SpawnHelper.SpawnAtPoint(name, plantGo);
            //            }
            //        }
            //    }
            //}
            QuickLogger.Info("Initialized", true);
        }

        public void Save(SaveData newSaveData)
        {

        }

        public override void OnProtoSerialize(ProtobufSerializer serializer)
        {
            
        }

        public override void OnProtoDeserialize(ProtobufSerializer serializer)
        {

        }

        public override void OnConstructedChanged(bool constructed)
        {
            IsConstructed = constructed;

            if (constructed)
            {
                if (isActiveAndEnabled)
                {
                    if (!IsInitialized)
                    {
                        Initialize();
                    }
                }
                else
                {
                    _runStartUpOnEnable = true;
                }
            }
        }

        public override bool CanDeconstruct(out string reason)
        {
           reason = String.Empty;
           return true;
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle main = HandReticle.main;
            main.SetInteractTextRaw($"Item PrefabID: {GetPrefabID()}","");
        }

        public void OnHandClick(GUIHand hand)
        {

        }

        public override bool ChangeBodyColor(Color color, ColorTargetMode mode)
        {
            QuickLogger.Info($"Changing material {FCSDemoModel.BodyMaterial} color to {ColorList.GetName(color)}",true);
            
            var result = _colorManager.ChangeColor(color, mode);

            if(result && mode == ColorTargetMode.Emission)
            {
                var lights = gameObject.GetComponentsInChildren<Light>();
                if (lights != null)
                {
                    foreach (Light light in lights)
                    {
                        light.color = color;
                    }
                }
            }

            return result;

        }
    }
}
