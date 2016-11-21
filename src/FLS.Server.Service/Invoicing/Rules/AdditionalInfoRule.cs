using FLS.Server.Data.DbEntities;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal class AdditionalInfoRule : BaseRule<RuleBasedFlightInvoiceDetails>
    {
        private readonly Flight _flight;

        internal AdditionalInfoRule(Flight flight)
        {
            _flight = flight;
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new Equals<bool>(_flight.FlightType.InstructorRequired, true));
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            flightInvoiceDetails.AdditionalInfo = "1";

            return base.Apply(flightInvoiceDetails);
        }

        public override RuleBasedFlightInvoiceDetails ElseApply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            flightInvoiceDetails.AdditionalInfo = "0";

            return base.Apply(flightInvoiceDetails);
        }
    }
}
