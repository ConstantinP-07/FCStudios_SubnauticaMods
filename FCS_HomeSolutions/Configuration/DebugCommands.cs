﻿using System;
using System.Collections.Generic;
using FCS_AlterraHub.Mono;
using FCS_AlterraHub.Registration;
using FCS_HomeSolutions.Curtains.Mono;
using FCS_HomeSolutions.QuantumTeleporter.Mono;
using FCS_HomeSolutions.SeaBreeze.Mono;
using FCSCommon.Utilities;
using SMLHelper.V2.Commands;
using UnityEngine;

namespace FCS_HomeSolutions.Configuration
{
    internal class DebugCommands
    {
        [ConsoleCommand("clearseabreeze")]
        public static string ClearSeabreezeCommand(int unitID)
        {
            var unitName = $"{Mod.SeaBreezeTabID}{unitID:D3}";

            QuickLogger.Debug($"Trying to find device: {unitName} || Count of Devices: {FCSAlterraHubService.PublicAPI.GetRegisteredDevices()?.Count}",true);
            foreach (KeyValuePair<string, FcsDevice> device in FCSAlterraHubService.PublicAPI.GetRegisteredDevices())
            {
                var compareResult = device.Key.Equals(unitName, StringComparison.OrdinalIgnoreCase);
                QuickLogger.Debug($"Compare Returned: {compareResult}", true);
                if (compareResult)
                {
                    var controller = device.Value.gameObject.GetComponent<SeaBreezeController>();
                    controller.ClearSeaBreeze();
                }
            }
            return $"Parameters: {unitID}";
        }

        [ConsoleCommand("setallglobal")]
        public static string SetAllGlobal(bool setGlobal)
        {
            foreach (KeyValuePair<string, FcsDevice> device in FCSAlterraHubService.PublicAPI.GetRegisteredDevicesOfId(Mod.QuantumTeleporterTabID))
            {
                var controller = device.Value.gameObject.GetComponent<QuantumTeleporterController>();
                controller.IsGlobal = setGlobal;
            }
            return $"Parameters: {setGlobal}";
        }

    }
}
