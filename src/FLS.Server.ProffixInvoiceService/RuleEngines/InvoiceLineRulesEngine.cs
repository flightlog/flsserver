using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Interfaces;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService;
using FLS.Server.ProffixInvoiceService.RuleFilters;
using FLS.Server.ProffixInvoiceService.Rules;
using NLog;

namespace FLS.Server.ProffixInvoiceService.RuleEngines
{
    internal class InvoiceLineRulesEngine
    {
        private Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private readonly ProffixFlightInvoiceDetails _flightInvoiceDetails;
        private readonly Flight _flight;
        private readonly InvoiceMapping _invoiceMapping;
        private readonly IPersonService _personService;

        public InvoiceLineRulesEngine(ProffixFlightInvoiceDetails flightInvoiceDetails, Flight flight, InvoiceMapping invoiceMapping, IPersonService personService)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flight = flight;
            _invoiceMapping = invoiceMapping;
            _personService = personService;
        }

        public ProffixFlightInvoiceDetails Run()
        {
            var aircraftMappingRules = new List<IRule<ProffixFlightInvoiceDetails>>();

            #region Aircraft in MasterFlight (Glider- or Motor-Flight)
            foreach (var aircraftMapping in _invoiceMapping.AircraftERPArticleMapping)
            {
                var rule = new AircraftFlightTimeRule(_flight, aircraftMapping);
                aircraftMappingRules.Add(rule);
            }

            _flightInvoiceDetails.ActiveFlightTime = _flight.Duration.TotalMinutes;

            while (_flightInvoiceDetails.ActiveFlightTime > 0)
            {
                _flightInvoiceDetails.ApplyRules(aircraftMappingRules);

                if (aircraftMappingRules.Any(x => x.RuleApplied) == false)
                {
                    Logger.Warn($"No aircraft mapping rule found for flight: {_flight}.");
                    break;
                }
            }
            #endregion Aircraft in MasterFlight (Glider- or Motor-Flight)

            #region Instructor fee
            var instructorFeeRule = new InstructorFeeRule(_flight, _invoiceMapping.InstructorToERPArticleMapping,
                _personService);

            _flightInvoiceDetails.ApplyRule(instructorFeeRule);
            #endregion Instructor fee

            #region Aircraft in TowFlight
            aircraftMappingRules.Clear();

            if (_flight.TowFlight != null)
            {
                foreach (var aircraftMapping in _invoiceMapping.AircraftERPArticleMapping)
                {
                    var rule = new AircraftFlightTimeRule(_flight.TowFlight, aircraftMapping);
                    aircraftMappingRules.Add(rule);
                }

                _flightInvoiceDetails.ActiveFlightTime = _flight.TowFlight.Duration.TotalMinutes;

                while (_flightInvoiceDetails.ActiveFlightTime > 0)
                {
                    _flightInvoiceDetails.ApplyRules(aircraftMappingRules);

                    if (aircraftMappingRules.Any(x => x.RuleApplied) == false)
                    {
                        Logger.Warn($"No aircraft mapping rule found for flight: {_flight.TowFlight}.");
                        break;
                    }
                }
            }
            #endregion Aircraft in TowFlight

            #region Additional Fuel Fee
            #endregion Additional Fuel Fee

            #region Landing taxes
            #endregion Landing taxes

            #region VSF fee

            #endregion VSF fee

            return _flightInvoiceDetails;
        }
    }
}
