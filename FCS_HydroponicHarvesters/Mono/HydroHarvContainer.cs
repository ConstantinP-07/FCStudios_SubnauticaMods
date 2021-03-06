﻿using System;
using System.Collections.Generic;
using System.Linq;
using FCS_HydroponicHarvesters.Buildables;
using FCS_HydroponicHarvesters.Enumerators;
using FCS_HydroponicHarvesters.Model;
using FCSCommon.Utilities;
using FCSTechFabricator.Interfaces;
using UnityEngine;

namespace FCS_HydroponicHarvesters.Mono
{
    internal class HydroHarvContainer : MonoBehaviour, IFCSStorage
    {
        private HydroHarvController _mono;
        internal int StorageLimit { get; private set; }
        public Action<int, int> OnContainerUpdate { get; set; }

        public int GetContainerFreeSpace => GetFreeSpace();
        public bool IsFull => CheckIfFull();
        internal Dictionary<TechType, int> Items = new Dictionary<TechType, int>();

        internal void Initialize(HydroHarvController mono)
        {
            _mono = mono;

            switch (mono.HydroHarvGrowBed.GetHydroHarvSize())
            {
                case HydroHarvSize.Unknown:
                    StorageLimit = 0;
                    break;
                case HydroHarvSize.Large:
                    StorageLimit = QPatch.Configuration.Config.LargeStorageLimit;
                    break;
                case HydroHarvSize.Medium:
                    StorageLimit = QPatch.Configuration.Config.MediumStorageLimit;
                    break;
                case HydroHarvSize.Small:
                    StorageLimit = QPatch.Configuration.Config.SmallStorageLimit;
                    break;
                default:
                    StorageLimit = 0;
                    break;
            }
        }

        private bool CheckIfFull()
        {
            return GetTotal() >= StorageLimit;
        }

        internal int GetTotal()
        {
            int amount = 0;
            foreach (KeyValuePair<TechType, int> item in Items)
            {
                amount += item.Value;
            }

            return amount;
        }

        private int GetFreeSpace()
        {
            int amount = 0;

            foreach (KeyValuePair<TechType, int> item in Items)
            {
                amount += item.Value;
            }

            return StorageLimit - amount;
        }

        public bool CanBeStored(int amount, TechType techType = TechType.None)
        {
            return true;
        }

        public bool AddItemToContainer(InventoryItem item)
        {
            //_mono.HydroHarvGrowBed.AddItemToContainer(item);
            return false;
        }

        internal void AddItemToContainer(TechType item, bool initializer = false)
        {
            QuickLogger.Debug("Adding to container", true);

            if (!IsFull)
            {
                QuickLogger.Debug($"Trying to add item to container {item}", true);

                if (Items.ContainsKey(item))
                {
                    Items[item] += 1;
                }
                else
                {
                    Items.Add(item, initializer ? 0 : 1);
                    QuickLogger.Debug($"Added item to container {item}", true);
                }
                OnContainerUpdate?.Invoke(GetTotal(), StorageLimit);
            }
        }

        internal void RemoveItemFromContainer(TechType item)
        {
            QuickLogger.Debug("Taking From Container", true);
            if (Items.ContainsKey(item))
            {
#if SUBNAUTICA
                var itemSize = CraftData.GetItemSize(item);
#elif BELOWZERO
            var itemSize = TechData.GetItemSize(item);
#endif
                if (!Inventory.main.HasRoomFor(itemSize.x, itemSize.y) || Items[item] < 1) return;

                Items[item] -= 1;
                var pickup = CraftData.InstantiateFromPrefab(item).GetComponent<Pickupable>();
                Inventory.main.Pickup(pickup);
                OnContainerUpdate?.Invoke(GetTotal(), StorageLimit);
                _mono?.Producer?.TryStartingNextClone();
            }
        }

        internal void RemoveItemFromContainerOnly(TechType item)
        {
            QuickLogger.Debug("Taking From Container", true);

            if (Items.ContainsKey(item) || Items[item] >= 0)
            {
                Items[item] -= 1;
                OnContainerUpdate?.Invoke(GetTotal(), StorageLimit);
                _mono?.Producer?.TryStartingNextClone();
            }

        }

        internal void DeleteItemFromContainer(TechType item)
        {
            if (!Items.ContainsKey(item)) return;

            //TODO Add Message to confirm Delete;

            if (Items[item] > 0)
            {
                QuickLogger.Message(HydroponicHarvestersBuildable.CannotDeleteDNAItem(Language.main.Get(item)), true);
                return;
            }

            if (_mono.HydroHarvGrowBed.GetDnaCount(item) <= 1)
            {
                Items.Remove(item);
            }
            _mono.HydroHarvGrowBed.RemoveDNA(item);
            OnContainerUpdate?.Invoke(0, 0);
        }

        public bool IsAllowedToAdd(Pickupable pickupable, bool verbose)
        {
            return true;
        }

        public bool IsAllowedToRemoveItems()
        {
            return true;
        }

        public Pickupable RemoveItemFromContainer(TechType techType, int amount)
        {
            Items[techType] -= 1;
            var go = GameObject.Instantiate(CraftData.GetPrefabForTechType(techType));
            var pickup = go.GetComponent<Pickupable>();
            OnContainerUpdate?.Invoke(0, 0);
            _mono?.Producer?.TryStartingNextClone();
            return pickup;
        }

        public Dictionary<TechType, int> GetItemsWithin()
        {
            return Items.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool ContainsItem(TechType techType)
        {
            return Items.Any(x => x.Key == techType);
        }

        internal void SpawnClone()
        {
            var samples = _mono.HydroHarvGrowBed.GetDNASamples();

            foreach (KeyValuePair<TechType, StoredDNAData> sample in samples)
            {
                for (int i = 0; i < sample.Value.Amount; i++)
                {
                    if (IsFull) break;
                    AddItemToContainer(sample.Key);
                }
            }
        }

        internal Dictionary<TechType, int> Save()
        {
            return Items;
        }

        internal void Load(Dictionary<TechType, int> savedDataContainer)
        {
            if (savedDataContainer == null) return;

            Items = savedDataContainer;

            OnContainerUpdate?.Invoke(GetTotal(), StorageLimit);
        }

        public bool HasItems()
        {
            return Items.Any(x => x.Value > 0);
        }

        public int GetItemCount(TechType techType)
        {
            int amount = 0;

            foreach (var item in Items)
            {
                if (item.Key == techType)
                {
                    amount += item.Value;
                }
            }

            return amount;
        }
    }
}
