﻿using System;
using System.Collections.Generic;
using FCS_AlterraHub.Enumerators;
using FCS_AlterraHub.Objects;
using FCS_ProductionSolutions.HydroponicHarvester.Enumerators;
using FCS_ProductionSolutions.HydroponicHarvester.Mono;
using Oculus.Newtonsoft.Json;

namespace FCS_ProductionSolutions.Configuration
{
    [Serializable]
    internal class HydroponicHarvesterDataEntry
    {
        [JsonProperty] internal string ID { get; set; }
        [JsonProperty] internal string SaveVersion { get; set; } = "1.0";
        [JsonProperty] internal Vec4 Body { get; set; }
        [JsonProperty] internal Dictionary<TechType, int> Storage { get; set; }
        [JsonProperty] internal bool IsVisible { get; set; }
        [JsonProperty] internal bool IsInBase { get; set; }
        [JsonProperty] internal SpeedModes SpeedMode { get; set; }
        [JsonProperty] internal List<SlotsData> SlotData { get; set; }
        [JsonProperty] internal bool SetBreaker { get; set; }
    }

    internal class ReplicatorDataEntry
    {
        [JsonProperty] internal string ID { get; set; }
        [JsonProperty] internal string SaveVersion { get; set; } = "1.0";
        [JsonProperty] internal Vec4 Body { get; set; }
        [JsonProperty] internal bool IsVisible { get; set; }
        [JsonProperty] internal TechType TargetItem { get; set; }
        [JsonProperty] internal float Progress { get; set; }
        [JsonProperty] internal int ItemCount { get; set; }
        [JsonProperty] internal SpeedModes Speed { get; set; }
    }

    internal class MatterAnalyzerDataEntry
    {
        [JsonProperty] internal string ID { get; set; }
        [JsonProperty] internal string SaveVersion { get; set; } = "1.0";
        [JsonProperty] internal Vec4 Body { get; set; }
        [JsonProperty] internal Dictionary<TechType, int> Storage { get; set; }
        [JsonProperty] internal bool IsVisible { get; set; }
        [JsonProperty] internal TechType CurrentTechType { get; set; }
        [JsonProperty] internal float CurrentScanTime { get; set; }
        [JsonProperty] internal float CurrentMaxScanTime { get; set; }
        [JsonProperty] internal TechType PickTechType { get; set; }
        [JsonProperty] internal bool IsLandPlant { get; set; }
        [JsonProperty] internal bool Reset { get; set; }
    }



    [Serializable]
    internal class SaveData
    {
        [JsonProperty] internal List<HydroponicHarvesterDataEntry> HydroponicHarvesterEntries = new List<HydroponicHarvesterDataEntry>();
        [JsonProperty] internal List<MatterAnalyzerDataEntry> MatterAnalyzerEntries = new List<MatterAnalyzerDataEntry>();
        [JsonProperty] internal List<ReplicatorDataEntry> ReplicatorEntries = new List<ReplicatorDataEntry>();
        [JsonProperty] internal List<DNASampleData> HydroponicHarvesterKnownTech { get; set; } = new List<DNASampleData>();
    }
}
