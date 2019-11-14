﻿using FCSCommon.Utilities.Enums;
using Oculus.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FCSCommon.Objects;

namespace FCS_DeepDriller.Configuration
{
    [Serializable]
    internal class SaveDataEntry
    {
        [JsonProperty] internal string Id { get; set; }
        public int SCA { get; set; }
        public float TTS { get; set; }
        public ColorVec4 BodyColor { get; set; }

    }

    [Serializable]
    internal class SaveData
    {
        [JsonProperty] internal List<SaveDataEntry> Entries = new List<SaveDataEntry>();
    }
}