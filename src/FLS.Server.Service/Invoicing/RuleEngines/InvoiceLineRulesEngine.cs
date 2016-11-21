using System.Collections.Generic;
using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.Invoicing.Rules;
using FLS.Server.Service.Invoicing.Rules.InvoiceLineRules;
using FLS.Server.Service.RulesEngine;
using NLog;

namespace FLS.Server.Service.Invoicing.RuleEngines
{
    internal class InvoiceLineRulesEngine
    {
        private Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private readonly RuleBasedFlightInvoiceDetails _flightInvoiceDetails;
        private readonly Flight _flight;
        private readonly InvoiceLineRuleFilterContainer _invoiceLineRuleFilterContainer;
        private readonly IPersonService _personService;

        public InvoiceLineRulesEngine(RuleBasedFlightInvoiceDetails flightInvoiceDetails, Flight flight, 
            IPersonService personService, InvoiceLineRuleFilterContainer invoiceLineRuleFilterContainer)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flight = flight;
            _invoiceLineRuleFilterContainer = invoiceLineRuleFilterContainer;
            _personService = personService;
        }

        public RuleBasedFlightInvoiceDetails Run()
        {
            var rules = new List<IRule<RuleBasedFlightInvoiceDetails>>();

            //No landing tax rule must be one of the first rule
            #region NO Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceLineRuleFilterContainer.NoLandingTaxRuleFilters)
            {
                var rule = new NoLandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion NO Landing taxes

            #region Aircraft in MasterFlight (Glider- or Motor-Flight)
            rules.Clear();

            foreach (var aircraftMapping in _invoiceLineRuleFilterContainer.AircraftRuleFilters)
            {
                var rule = new AircraftFlightTimeRule(_flight, aircraftMapping);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ActiveFlightTime = _flight.FlightDurationZeroBased.TotalMinutes;

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
            var instructorFeeRule = new InstructorFeeRule(_flight, _invoiceLineRuleFilterContainer.InstructorToArticleMapping,
                _personService);

            _flightInvoiceDetails.ApplyRule(instructorFeeRule);
            #endregion Instructor fee

            #region Aircraft in TowFlight
            //run rule engine again for towflight, before other rules were applied (because of order of invoice lines)
            if (_flight.TowFlight != null)
            {
                //set IncludesTowFlightId otherwise the flight state of towflight will not be set correctly during invoicing
                _flightInvoiceDetails.IncludesTowFlightId = _flight.TowFlightId;

                var invoiceLineRulesEngine = new InvoiceLineRulesEngine(_flightInvoiceDetails, _flight.TowFlight,
                    _personService, _invoiceLineRuleFilterContainer);
                invoiceLineRulesEngine.Run();
            }
            #endregion Aircraft in TowFlight

            #region Additional Fuel Fee
            rules.Clear();

            foreach (var filter in _invoiceLineRuleFilterContainer.AdditionalFuelFeeRuleFilters)
            {
                var rule = new AdditionalFuelFeeRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Additional Fuel Fee

            #region Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceLineRuleFilterContainer.LandingTaxRuleFilters)
            {
                var rule = new LandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Landing taxes

            #region VSF fee
            rules.Clear();

            foreach (var filter in _invoiceLineRuleFilterContainer.VsfFeeRuleFilters)
            {
                var rule = new VsfFeeRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ApplyRules(rules);
            #endregion VSF fee

            return _flightInvoiceDetails;
        }
    }
}
