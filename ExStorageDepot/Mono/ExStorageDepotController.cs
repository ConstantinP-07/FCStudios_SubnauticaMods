﻿using ExStorageDepot.Buildable;
using ExStorageDepot.Configuration;
using ExStorageDepot.Enumerators;
using ExStorageDepot.Mono.Managers;
using FCSCommon.Utilities;
using System;
using FCSTechFabricator.Abstract;
using FCSTechFabricator.Components;

namespace ExStorageDepot.Mono
{
    public class ExStorageDepotController : FCSController
    {
        internal ExStorageDepotDisplayManager Display { get; private set; }
        private ExStorageDepotSaveDataEntry _saveData;
        private bool _initialized;
        private bool _runStartUpOnEnable;
        private bool _fromSave;
        internal ExStorageDepotNameManager NameController { get; private set; }
        internal ExStorageDepotAnimationManager AnimationManager { get; private set; }
        internal ExStorageDepotStorageManager Storage { get; private set; }
        internal BulkMultipliers BulkMultiplier { get; set; }
        public FCSConnectableDevice FCSConnectableDevice { get; private set; }

        private void OnEnable()
        {
            if (!_runStartUpOnEnable) return;

            if (!_initialized)
            {
                Initialize();
            }

            if (_fromSave)
            {
                QuickLogger.Debug("In OnProtoDeserialize");
                var prefabIdentifier = GetComponent<PrefabIdentifier>() ?? GetComponentInParent<PrefabIdentifier>();
                var id = prefabIdentifier?.Id ?? string.Empty;
                var data = Mod.GetExStorageDepotSaveData(id);
                NameController.SetCurrentName(data.UnitName);
                
                if (data.StorageItems != null)
                {
                    Storage.LoadFromSave(data.StorageItems);
                }

                BulkMultiplier = data.Multiplier;
                Display.UpdateMultiplier();
            }

            _runStartUpOnEnable = false;
        }

        public override void OnProtoSerialize(ProtobufSerializer serializer)
        {
            if (!_initialized) return;

            if (!Mod.IsSaving())
            {
                QuickLogger.Info("Saving Ex Storage Depot");
                Mod.SaveExStorageDepot();
            }
        }

        public override void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            _fromSave = true;
        }

        public override bool CanDeconstruct(out string reason)
        {
            reason = string.Empty;

            if (Storage == null || !Storage.IsFull) return true;
            reason = "Please empty the Ex-Storage";
            return false;
        }

        public override void OnConstructedChanged(bool constructed)
        {
            if (constructed)
            {
                if (isActiveAndEnabled)
                {
                    if (!_initialized)
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

        public override void Initialize()
        {
            
            if (NameController == null)
            {
                NameController = new ExStorageDepotNameManager();
                NameController.Initialize(this);
            }
            
            if (AnimationManager == null)
            {
                AnimationManager = gameObject.AddComponent<ExStorageDepotAnimationManager>();
            }

            if (Storage == null)
            {
                Storage = gameObject.AddComponent<ExStorageDepotStorageManager>();
                Storage.Initialize(this);
            }
            
            if (DumpContainer == null)
            {
                DumpContainer = gameObject.AddComponent<DumpContainer>();
                DumpContainer.Initialize(transform, ExStorageDepotBuildable.DumpContainerLabel(), ExStorageDepotBuildable.FoodNotAllowed(), ExStorageDepotBuildable.ContainerFullMessage(), Storage);
                DumpContainer.OnDumpContainerClosed += Storage.OnDumpContainerClosed;
            }

            if (Display == null)
            {
                Display = gameObject.AddComponent<ExStorageDepotDisplayManager>();
                Display.Initialize(this);
            }

            if (FCSConnectableDevice == null)
            {
                FCSConnectableDevice = gameObject.AddComponent<FCSConnectableDevice>();
                FCSConnectableDevice.Initialize(this, Storage);
            }

            _initialized = true;
        }

        internal void Save(ExStorageDepotSaveData saveDataList)
        {
            var prefabIdentifier = GetComponent<PrefabIdentifier>();
            var id = prefabIdentifier.Id;

            if (_saveData == null)
            {
                _saveData = new ExStorageDepotSaveDataEntry();
            }
            _saveData.Id = id;
            _saveData.UnitName = NameController.GetCurrentName();
            _saveData.StorageItems = Storage.ContainerItems;
            _saveData.Multiplier = BulkMultiplier;
            saveDataList.Entries.Add(_saveData);
        }
    }
}
