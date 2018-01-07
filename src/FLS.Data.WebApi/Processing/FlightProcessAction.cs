using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Flight;

namespace FLS.Data.WebApi.Processing
{
    public class FlightProcessAction
    {
        public FlightProcessAction()
        {
            PossibleFlightProcessActions = new List<FlightProcessActionDetails>();
        }

        public FlightProcessState CurrentFlightProcessState { get; set; }

        public List<FlightProcessActionDetails> PossibleFlightProcessActions { get; set; }
    }
}
