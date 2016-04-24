using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.ProffixInvoiceService.Conditions;

namespace FLS.Server.ProffixInvoiceService.Rules
{
    internal class AdditionalInfoRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;

        internal AdditionalInfoRule(Flight flight)
        {
            _flight = flight;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new Equals<bool>(_flight.FlightType.InstructorRequired, true));
        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            flightInvoiceDetails.AdditionalInfo = "1";

            return base.Apply(flightInvoiceDetails);
        }

        public override ProffixFlightInvoiceDetails ElseApply(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            flightInvoiceDetails.AdditionalInfo = "0";

            return base.Apply(flightInvoiceDetails);
        }
    }
}
