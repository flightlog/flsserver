using System;
using System.Collections.Generic;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.Interfaces;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal class InstructorFeeRule : BaseRule<RuleBasedFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly Dictionary<string, string> _instructorMapping;
        private readonly IPersonService _personService;
        private string MemberNumber = String.Empty;

        internal InstructorFeeRule(Flight flight, Dictionary<string, string> instructorMapping, IPersonService personService)
        {
            _flight = flight;
            _instructorMapping = instructorMapping;
            _personService = personService;
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            if (_flight.Instructor != null && _flight.Instructor.PersonId.IsValid())
            {
                var person = _personService.GetPilotPersonDetails(_flight.Instructor.PersonId, flightInvoiceDetails.ClubId);

                if (person != null && person.ClubRelatedPersonDetails != null)
                {
                    MemberNumber = person.ClubRelatedPersonDetails.MemberNumber;

                    if (string.IsNullOrWhiteSpace(MemberNumber))
                    {
                        Logger.Warn("MemberNumber is empty");
                    }
                    else
                    {
                        Conditions.Add(new ContainsKey<string, string>(_instructorMapping, MemberNumber));

                        return;
                    }
                }
            }

            //make Rule invalid, if no instructor with membernumber found
            Conditions.Add(new Equals<bool>(true, false));
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            var line = new FlightInvoiceLineItem();
            line.FlightId = _flight.FlightId;
            line.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
            line.ERPArticleNumber = _instructorMapping[MemberNumber];
            line.UnitType = CostCenterUnitType.PerFlightMinute.ToUnitTypeString();

            line.InvoiceLineText = $"Fluglehrer-Honorar {_flight.InstructorDisplayName}";

            if (_flight.FlightCostBalanceTypeId.HasValue &&
                                _flight.FlightCostBalanceTypeId.Value ==
                                (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.NoInstructorFee)
            {
                //no instructor fee for this flight, so set quantity to 0
                line.Quantity = 0;
            }
            else
            {
                line.Quantity = Convert.ToDecimal(_flight.FlightDurationZeroBased.TotalMinutes);
            }

            flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

            Logger.Debug($"Added new invoice item line to invoice. Line: {line}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
