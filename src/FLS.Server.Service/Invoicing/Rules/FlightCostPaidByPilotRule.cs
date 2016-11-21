using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal class FlightCostPaidByPilotRule : BaseRule<RuleBasedFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly IPersonService _personService;

        internal FlightCostPaidByPilotRule(Flight flight, IPersonService personService)
        {
            _flight = flight;
            _personService = personService;
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            ICondition condition = new Equals<int>(_flight.FlightCostBalanceTypeId.GetValueOrDefault(), (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.PilotPaysAllCosts);

            condition = new Or(condition,
                    new Equals<int>(_flight.FlightCostBalanceTypeId.GetValueOrDefault(), (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.NoInstructorFee));

            Conditions.Add(condition);
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            var personDetails = _personService.GetPilotPersonDetails(_flight.Pilot.PersonId, flightInvoiceDetails.ClubId);

            if (personDetails != null)
            {
                flightInvoiceDetails.RecipientDetails.RecipientName = personDetails.Lastname + " " +
                                                                      personDetails.Firstname;
                flightInvoiceDetails.RecipientDetails.Lastname = personDetails.Lastname;
                flightInvoiceDetails.RecipientDetails.Firstname = personDetails.Firstname;
                flightInvoiceDetails.RecipientDetails.AddressLine1 = personDetails.AddressLine1;
                flightInvoiceDetails.RecipientDetails.AddressLine2 = personDetails.AddressLine2;
                flightInvoiceDetails.RecipientDetails.ZipCode = personDetails.ZipCode;
                flightInvoiceDetails.RecipientDetails.City = personDetails.City;
                flightInvoiceDetails.RecipientDetails.PersonClubMemberNumber = personDetails.ClubRelatedPersonDetails.MemberNumber;
            }

            return base.Apply(flightInvoiceDetails);
        }
    }
}
