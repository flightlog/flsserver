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
            var rules = new List<IRule<ProffixFlightInvoiceDetails>>();

            //No landing tax rule must be one of the first rule
            #region NO Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceMapping.NoLandingTaxRules)
            {
                var rule = new NoLandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion NO Landing taxes

            #region Aircraft in MasterFlight (Glider- or Motor-Flight)
            foreach (var aircraftMapping in _invoiceMapping.AircraftERPArticleMapping)
            {
                var rule = new AircraftFlightTimeRule(_flight, aircraftMapping);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ActiveFlightTime = _flight.Duration.TotalMinutes;

            while (_flightInvoiceDetails.ActiveFlightTime > 0)
            {
                _flightInvoiceDetails.ApplyRules(rules);

                if (rules.Any(x => x.RuleApplied) == false)
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
            //run rule engine again for towflight, before other rules were applied (because of order of invoice lines)
            if (_flight.TowFlight != null)
            {
                var invoiceLineRulesEngine = new InvoiceLineRulesEngine(_flightInvoiceDetails, _flight.TowFlight,
                    _invoiceMapping, _personService);
                invoiceLineRulesEngine.Run();
            }
            #endregion Aircraft in TowFlight

            #region Additional Fuel Fee
            rules.Clear();

            foreach (var filter in _invoiceMapping.AdditionalFuelFeeRules)
            {
                var rule = new AdditionalFuelFeeRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Additional Fuel Fee

            #region Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceMapping.LandingTaxRules)
            {
                var rule = new LandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Landing taxes

            #region VSF fee
            _flightInvoiceDetails.ApplyRule(new VsfFeeRule(_flight, _invoiceMapping.VsfFee));
            #endregion VSF fee

            return _flightInvoiceDetails;
        }
    }
}
