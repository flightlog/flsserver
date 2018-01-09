using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Flight;

namespace FLS.Data.WebApi.Processing
{
    public class FlightProcessRequest
    {
        public Guid FlightId { get; set; }

        public FlightProcessState NewFlightProcessState { get; set; }
    }
}
