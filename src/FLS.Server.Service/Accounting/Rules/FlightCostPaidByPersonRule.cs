using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Accounting.Rules
{
    internal class FlightCostPaidByPersonRule : BaseRule<RuleBasedDeliveryDetails>
    {
        private readonly Flight _flight;
        private readonly IPersonService _personService;

        internal FlightCostPaidByPersonRule(Flight flight, IPersonService personService)
        {
            _flight = flight;
            _personService = personService;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            Conditions.Add(new Equals<int>(_flight.FlightCostBalanceTypeId.GetValueOrDefault(), (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.CostsPaidByPerson));
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            var invoiceRecipient =
                            _flight.FlightCrews.FirstOrDefault(
                                fc => fc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightCostInvoiceRecipient);

            if (invoiceRecipient != null)
            {
                var personDetails = _personService.GetPilotPersonDetails(invoiceRecipient.PersonId, ruleBasedDelivery.ClubId);

                if (personDetails != null)
                {
                    ruleBasedDelivery.RecipientDetails.RecipientName = personDetails.Lastname + " " +
                                                                          personDetails.Firstname;
                    ruleBasedDelivery.RecipientDetails.Lastname = personDetails.Lastname;
                    ruleBasedDelivery.RecipientDetails.Firstname = personDetails.Firstname;
                    ruleBasedDelivery.RecipientDetails.AddressLine1 = personDetails.AddressLine1;
                    ruleBasedDelivery.RecipientDetails.AddressLine2 = personDetails.AddressLine2;
                    ruleBasedDelivery.RecipientDetails.ZipCode = personDetails.ZipCode;
                    ruleBasedDelivery.RecipientDetails.City = personDetails.City;
                    ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber = personDetails.ClubRelatedPersonDetails.MemberNumber;
                }
            }

            return base.Apply(ruleBasedDelivery);
        }
    }
}
