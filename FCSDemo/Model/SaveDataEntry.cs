﻿using System;
using System.Collections.Generic;
using FCS_AlterraHub.Objects;
using Oculus.Newtonsoft.Json;

namespace Model
{
    [Serializable]
    internal class SaveDataEntry
    {
        [JsonProperty] internal string ID { get; set; }
        [JsonProperty] internal Vec4 Body { get; set; }
    }

    [Serializable]
    internal class SaveData
    {
        [JsonProperty] internal List<SaveDataEntry> Entries = new List<SaveDataEntry>();
    }
}
