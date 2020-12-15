﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using SMLHelper.V2.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace FCS_HomeSolutions.Configuration
{
    internal static class AuxPatchers
    {
        private const string ModKey = "AHS";

        private static readonly Dictionary<string, string> LanguageDictionary = new Dictionary<string, string>
        {
            { $"{ModKey}_Max","MAX"},
            { $"{ModKey}_High","HIGH"},
            { $"{ModKey}_Min","MIN"},
            { $"{ModKey}_Low","LOW"},
            { $"{ModKey}_GatesOpenMessage","Gates are open cannot move."},
            { $"{ModKey}_Save","SAVE"},
            { $"{ModKey}_SaveLevel","Save Current Level"},
            { $"{ModKey}_GoingToLevelFormat","Going to floor {0}"},
            { $"{ModKey}_FloorSelect","Selecting Floor: {0}"},
            { $"{ModKey}_HoverPadInOperation","Hover Lift Pad is in operation please unlock any prawn suit or yourself."},
            { $"{ModKey}_UnDockPrawnSuitToEnter","Undock prawn suit to enter."},
            { $"{ModKey}_ClickToOpen","Click to open {0}."},
            { $"{ModKey}_NoRecyclerConnected","No Recycler connected to the base"},
            { $"{ModKey}_TrashReceptacleDumpLabel","Trash Receptacle Bin"},
            { $"{ModKey}_TrashRecyclerDumpLabel","Trash Recycler Bin"},
            { $"{ModKey}_TrashRecyclerStatusFormat","Recycling {0} Status:"},
            { $"{ModKey}_Waiting","WAITING"},
            { $"{ModKey}_TrashRecyclerStorageFormat","Storage: ({0}/{1})"},
            { $"{ModKey}_Submit","SUBMIT"},
            { $"{ModKey}_ToBaseNotEnoughPower","Destination base does not have enough power to teleport"},
            { $"{ModKey}_BaseNotEnoughPower","This base does not have enough power to teleport"},
            { $"{ModKey}_HomeNetworkToggle","Home Network"},
            { $"{ModKey}_HomeNetworkToggleDesc","Shows all available teleporters at this base."},
            { $"{ModKey}_GlobalNetworkToggle","Global Network"},
            { $"{ModKey}_GlobalNetworkToggleDesc","Shows all available teleporters that have been set to the global network  at other bases."},
            { $"{ModKey}_Rename","Rename"},
            { $"{ModKey}_RenameDesc","Renames this teleporter."},
            { $"{ModKey}_AddToGlobalNetworkToggle","Add To Global Network"},
            { $"{ModKey}_AddToGlobalNetworkToggleDesc","Allows this teleporter to be visible in the global network."},
            { $"{ModKey}_LocationFormat","Location: {0}"},
            { $"{ModKey}_GlobalNetwork","Global Network"},
            { $"{ModKey}_LocalNetwork","Local Network"},
            { $"{ModKey}_RequirementsFormat","Teleport Power Requirement: Local ({0}) / Global ({1})"},
            { $"{ModKey}_Confirm","Confirm"},
            { $"{ModKey}_Cancel","Cancel"},
            { $"{ModKey}_PowerAvailable","Power Available"},
            { $"{ModKey}_TeleportConfirmMessage","Initiate Teleportation?"},
            { $"{ModKey}_Coordinates","Coordinates"},
            { $"{ModKey}_CurtainInteractionFormat","Press {0} to change curtain pattern."},
            { $"{ModKey}_TeleportCanceledMessage","Something went wrong with the teleport sequence canceled operation for your safety."},
        };

        internal static void AdditionalPatching()
        {
            foreach (KeyValuePair<string, string> languageEntry in LanguageDictionary)
            {
                LanguageHandler.SetLanguageLine(languageEntry.Key, languageEntry.Value);
            }
        }

        private static string GetLanguage(string key)
        {
            return LanguageDictionary.ContainsKey(key) ? Language.main.Get(key) : "N/A";
        }

        internal static string Max()
        {
            return GetLanguage($"{ModKey}_Max");
        }

        internal static string High()
        {
            return GetLanguage($"{ModKey}_High");
        }

        internal static string Min()
        {
            return GetLanguage($"{ModKey}_Min");
        }

        internal static string Low()
        {
            return GetLanguage($"{ModKey}_Low");
        }

        public static string GatesOpenMessage()
        {
            return GetLanguage($"{ModKey}_GatesOpenMessage");
        }

        public static string Save()
        {
            return GetLanguage($"{ModKey}_Save");
        }

        public static string SaveLevel()
        {
            return GetLanguage($"{ModKey}_SaveLevel");
        }

        public static string GoingToLevelFormat(string levelName)
        {
            return string.Format(GetLanguage($"{ModKey}_GoingToLevelFormat"),levelName);
        }

        public static string FloorSelect(string levelName)
        {
            return string.Format(GetLanguage($"{ModKey}_FloorSelect"),levelName);
        }

        public static string HoverPadInOperation()
        {
            return GetLanguage($"{ModKey}_HoverPadInOperation");
        }

        public static string UnDockPrawnSuitToEnter()
        {
            return GetLanguage($"{ModKey}_UnDockPrawnSuitToEnter");
        }

        public static string ClickToOpenRecycle(string name)
        {
            return string.Format(GetLanguage($"{ModKey}_ClickToOpen"),name);
        }

        public static string NoRecyclerConnected()
        {
            return GetLanguage($"{ModKey}_NoRecyclerConnected");
        }

        public static string TrashReceptacleDumpLabel()
        {
            return GetLanguage($"{ModKey}_TrashReceptacleDumpLabel");
        }

        public static string TrashRecyclerDumpLabel()
        {
            return GetLanguage($"{ModKey}_TrashRecyclerDumpLabel");
        }

        public static string TrashRecyclerStatusFormat(string item)
        {
            return string.Format(GetLanguage($"{ModKey}_TrashRecyclerStatusFormat"),item);
        }

        public static string Waiting()
        {
            return GetLanguage($"{ModKey}_Waiting");
        }

        public static string TrashRecyclerStorageFormat(int count, int max)
        {
            return string.Format(GetLanguage($"{ModKey}_TrashRecyclerStorageFormat"), count, max);
        }

        public static string Submit()
        {
            return GetLanguage($"{ModKey}_Submit");
        }

        public static string ToBaseNotEnoughPower()
        {
            return GetLanguage($"{ModKey}_ToBaseNotEnoughPower");
        }

        public static string BaseNotEnoughPower()
        {
            return GetLanguage($"{ModKey}_BaseNotEnoughPower");
        }

        public static string HomeNetworkToggle()
        {
            return GetLanguage($"{ModKey}_HomeNetworkToggle");
        }

        public static string HomeNetworkToggleDesc()
        {
            return GetLanguage($"{ModKey}_HomeNetworkToggleDesc");
        }

        public static string GlobalNetworkToggle()
        {
            return GetLanguage($"{ModKey}_GlobalNetworkToggle");
        }

        public static string GlobalNetworkToggleDesc()
        {
            return GetLanguage($"{ModKey}_GlobalNetworkToggleDesc");
        }

        public static string Rename()
        {
            return GetLanguage($"{ModKey}_Rename");
        }

        public static string RenameDesc()
        {
            return GetLanguage($"{ModKey}_RenameDesc");
        }

        public static string AddToGlobalNetworkToggle()
        {
            return GetLanguage($"{ModKey}_AddToGlobalNetworkToggle");
        }

        public static string AddToGlobalNetworkToggleDesc()
        {
            return GetLanguage($"{ModKey}_AddToGlobalNetworkToggleDesc");
        }

        public static string LocationFormat(Vector3 position)
        {
            return string.Format(GetLanguage($"{ModKey}_LocationFormat"), position);
        }

        public static string LocalNetwork()
        {
            return GetLanguage($"{ModKey}_LocalNetwork");
        }

        public static string GlobalNetwork()
        {
            return GetLanguage($"{ModKey}_GlobalNetwork");
        }

        public static string Requirements(float internalTeleportPowerUsage, float globalTeleportPowerUsage)
        {
            return string.Format(GetLanguage($"{ModKey}_RequirementsFormat"), internalTeleportPowerUsage, globalTeleportPowerUsage);
        }

        public static string Confirm()
        {
            return GetLanguage($"{ModKey}_Confirm");
        }

        public static string Cancel()
        {
            return GetLanguage($"{ModKey}_Cancel");
        }

        public static string TeleportCanceledMessage()
        {
            return GetLanguage($"{ModKey}_TeleportCanceledMessage");
        }

        public static string TeleporterConfirmMessage()
        {
            return GetLanguage($"{ModKey}_TeleportConfirmMessage");
        }

        public static string Coordinate()
        {
            return GetLanguage($"{ModKey}_Coordinates");
        }

        public static string PowerAvailable()
        {
            return GetLanguage($"{ModKey}_PowerAvailable");
        }

        public static string CurtainInteractionFormat(string key)
        {
            return string.Format(GetLanguage($"{ModKey}_CurtainInteractionFormat"), key);

        }
    }
}

