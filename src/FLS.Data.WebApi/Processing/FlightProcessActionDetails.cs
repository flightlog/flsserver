using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Flight;

namespace FLS.Data.WebApi.Processing
{
    public class FlightProcessActionDetails
    {
        public FlightProcessState NewFlightProcessState { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        public bool WillDeleteDeliveries { get; set; }
    }
}
