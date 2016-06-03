using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Extensions;
using FLS.Server.Interfaces;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.Conditions;

namespace FLS.Server.ProffixInvoiceService.Rules
{
    internal class InstructorFeeRule : BaseRule<ProffixFlightInvoiceDetails>
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

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
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

            //make Rule invalid, if no instructor with memberkey found
            Conditions.Add(new Equals<bool>(true, false));
        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
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
                line.Quantity = Convert.ToDecimal(_flight.Duration.TotalMinutes);
            }

            flightInvoiceDetails.FlightInvoiceLineItems.Add(line);

            Logger.Debug($"Added new invoice item line to invoice. Line: {line}");

            return base.Apply(flightInvoiceDetails);
        }
    }
}
