using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLS.Server.Data.Objects.Aircraft
{
    public class OgnDevices
    {
        [JsonProperty(PropertyName = "devices")]
        public List<OgnDevice> Devices { get; set; }
    }
}
