using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLS.Server.Data.Objects.Aircraft
{
    public class OgnDevice
    {
        [JsonProperty(PropertyName = "device_type")]
        public string DeviceType { get; set; }

        [JsonProperty(PropertyName = "device_id")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "aircraft_model")]
        public string AircraftModel { get; set; }

        [JsonProperty(PropertyName = "registration")]
        public string Registration { get; set; }

        [JsonProperty(PropertyName = "cn")]
        public string Cn { get; set; }

        [JsonProperty(PropertyName = "tracked")]
        public bool IsTracked { get; set; }

        [JsonProperty(PropertyName = "identified")]
        public bool IsIdentified { get; set; }

    }
}
