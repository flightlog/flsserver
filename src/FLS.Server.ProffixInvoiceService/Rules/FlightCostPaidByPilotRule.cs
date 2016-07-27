﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.ProffixInvoiceService.Conditions;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;

namespace FLS.Server.ProffixInvoiceService.Rules
{
    internal class FlightCostPaidByPilotRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;
        private readonly IPersonService _personService;

        internal FlightCostPaidByPilotRule(Flight flight, IPersonService personService)
        {
            _flight = flight;
            _personService = personService;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new Equals<int>(_flight.FlightCostBalanceTypeId.GetValueOrDefault(), (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.PilotPaysAllCosts));
        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
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
