﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NzbDrone.Core.Download.Clients.Deluge
{
    public class DelugeTorrent
    {
        public String Hash { get; set; }
        public String Name { get; set; }
        public String State { get; set; }
        public Double Progress { get; set; }
        public Int32 Eta { get; set; }
        public String Message { get; set; }

        [JsonProperty(PropertyName = "is_finished")]
        public Boolean IsFinished { get; set; }
        
        // TODO: Also 'move_on_completed_path'. What is the difference?
        [JsonProperty(PropertyName = "move_completed_path")] 
        public String DownloadPath { get; set; }

        [JsonProperty(PropertyName = "total_size")]
        public Int64 Size { get; set; }

        [JsonProperty(PropertyName = "total_done")]
        public Int64 BytesDownloaded { get; set; }

        [JsonProperty(PropertyName = "time_added")]
        public Double DateAdded { get; set; }

        [JsonProperty(PropertyName = "active_time")]
        public Int32 SecondsDownloading { get; set; }
    }
}