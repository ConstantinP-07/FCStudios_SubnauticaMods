﻿using System.Collections.Generic;
using SMLHelper.V2.Handlers;

namespace FCS_StorageSolutions.Configuration
{
    internal static class AuxPatchers
    {
        private const string ModKey = "ASS";

        private static readonly Dictionary<string, string> LanguageDictionary = new Dictionary<string, string>
        {
            { $"{ModKey}_OpenAlterraStorage","Open Alterra Storage"},
            { $"{ModKey}_AlterraStorageDumpContainerTitle","Alterra Storage"},
            { $"{ModKey}_TakeFormat","Take {0}"},
            { $"{ModKey}_StorageAmountFormat","{0}/{1} Items"},
            { $"{ModKey}_OpenWallServerRack","Open Wall Server Rack"},
            { $"{ModKey}_CloseWallServerRack","Close Wall Server Rack"},
            { $"{ModKey}_AddItemToItemDisplay","Add Item"},
            { $"{ModKey}_AddItemToItemDisplayDesc","Click to add an item to the display to should the quantity in the system"},
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
            return LanguageDictionary.ContainsKey(key) ? Language.main.Get(LanguageDictionary[key]) : string.Empty;
        }

        internal static string OpenAlterraStorage()
        {
            return GetLanguage($"{ModKey}_OpenAlterraStorage");
        }

        public static string AlterraStorageDumpContainerTitle()
        {
            return GetLanguage($"{ModKey}_AlterraStorageDumpContainerTitle");
        }

        public static string TakeFormatted(string techType)
        {
            return string.Format(GetLanguage($"{ModKey}_TakeFormat"),techType);
        }

        public static string AlterraStorageAmountFormat(int count, int maxStorage)
        {
            return string.Format(GetLanguage($"{ModKey}_StorageAmountFormat"), count,maxStorage);
        }

        public static string ContainerNotEmpty()
        {
            return Language.main.Get("DeconstructNonEmptyStorageContainerError");
        }

        public static string OpenWallServerRack()
        {
            return GetLanguage($"{ModKey}_OpenWallServerRack");
        }

        public static string CloseWallServerRack()
        {
            return GetLanguage($"{ModKey}_CloseWallServerRack");
        }

        public static string AddItemToItemDisplay()
        {
            return GetLanguage($"{ModKey}_AddItemToItemDisplay");
        }

        public static string AddItemToItemDisplayDesc()
        {
            return GetLanguage($"{ModKey}_AddItemToItemDisplayDesc");
        }
    }
}
