﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace wakaru
{
    public class Configuration
    {
        public static readonly Configuration Instance = File.Exists(@".\config.json") ? JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(@".\config.json")) ?? new Configuration() : new Configuration();
        public string? HardwareID { get; set; }
        public string LocalAPIUrl { get; set; } = "http://127.0.0.1:8000";
        public string OnlineAPIUrl { get; set; } = "https://wakatta.nogu.dev/api";
        public bool UseLocal { get; set; } = false;
        public static void Save()
        {
            File.WriteAllText(@"./config.json", JsonConvert.SerializeObject(Instance));
        }
    }
}
