using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Accounting.Rules
{
    internal class FlightCostPaidByPilotRule : BaseRule<RuleBasedDeliveryDetails>
    {
        private readonly Flight _flight;
        private readonly IPersonService _personService;

        internal FlightCostPaidByPilotRule(Flight flight, IPersonService personService)
        {
            _flight = flight;
            _personService = personService;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            ICondition condition = new Equals<int>(_flight.FlightCostBalanceTypeId.GetValueOrDefault(), (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.PilotPaysAllCosts);

            condition = new Or(condition,
                    new Equals<int>(_flight.FlightCostBalanceTypeId.GetValueOrDefault(), (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.NoInstructorFee));

            Conditions.Add(condition);
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            var personDetails = _personService.GetPersonDetails(_flight.Pilot.PersonId, ruleBasedDelivery.ClubId);

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

            return base.Apply(ruleBasedDelivery);
        }
    }
}
