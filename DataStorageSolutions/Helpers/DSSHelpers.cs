﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStorageSolutions.Configuration;
using DataStorageSolutions.Model;
using DataStorageSolutions.Mono;
using FCSCommon.Enums;
using FCSCommon.Utilities;

namespace DataStorageSolutions.Helpers
{
    internal class DSSHelpers
    {
        public static bool GivePlayerItem(TechType techType, ObjectDataTransferData itemData,Func<ObjectData,RackSlot> getServerWithObjectData)
        {
            QuickLogger.Debug($"Give Player Item: {techType}", true);

            bool isSuccessful = false;

#if SUBNAUTICA
            var itemSize = CraftData.GetItemSize(techType);
#elif BELOWZERO
            var itemSize = TechData.GetItemSize(techType);
#endif
            if (Inventory.main.HasRoomFor(itemSize.x, itemSize.y))
            {
                var pickup = CraftData.InstantiateFromPrefab(techType).GetComponent<Pickupable>();

                if (!itemData.IsServer)
                {
                    var data = (ObjectData)itemData.data;
                    switch (data.DataObjectType)
                    {
                        case SaveDataObjectType.PlayerTool:
                            QuickLogger.Debug("Making PlayerTool", true);
                            if (data.PlayToolData.HasBattery)
                            {
                                var tempBattery = CraftData.GetPrefabForTechType(data.PlayToolData.BatteryInfo.TechType);
                                var capacity = tempBattery?.gameObject.GetComponent<IBattery>()?.capacity;

                                QuickLogger.Debug($"Checking Capacity: {capacity}", true);
                                if (data.PlayToolData.HasBattery && capacity != null && capacity > 0)
                                {
                                    QuickLogger.Debug("Passed Capacity Check", true);
                                    var energyMixin = pickup.gameObject.GetComponent<EnergyMixin>();
                                    var normalizedCharge = data.PlayToolData.BatteryInfo.BatteryCharge / capacity;
                                    energyMixin.SetBattery(data.PlayToolData.BatteryInfo.TechType, (float)normalizedCharge);
                                    QuickLogger.Debug("Setting PlayerTool Battery", true);
                                }
                                else
                                {
                                    QuickLogger.Error<DSSServerController>("While trying to get the batter capacity of the battery it returned null or 0.");
                                }
                            }
                            break;

                        case SaveDataObjectType.Eatable:
                            var eatable = pickup.gameObject.GetComponent<Eatable>();
                            eatable.waterValue = data.EatableEntity.GetWaterValue();
                            eatable.foodValue = data.EatableEntity.GetFoodValue();
                            break;

                        case SaveDataObjectType.Server:
                            var server = pickup.gameObject.GetComponent<DSSServerController>();
                            server.Items = new List<ObjectData>(data.ServerData);
                            server.Initialize();
                            server.DisplayManager.UpdateDisplay();
                            break;
                    }

                    var result = getServerWithObjectData?.Invoke(data);
                    result?.Remove(data);
                    isSuccessful = true;
                }
                else
                {
                    var data = (List<ObjectData>)itemData.data;
                    var controller = pickup.gameObject.GetComponent<DSSServerController>();
                    controller.Initialize();
                    controller.Items = data;
                    controller.DisplayManager.UpdateDisplay();
                    isSuccessful = true;
                }
                Inventory.main.Pickup(pickup);
            }
            Mod.OnBaseUpdate?.Invoke();
            return isSuccessful;
        }
    }
}
