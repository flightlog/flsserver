using System.Collections.Generic;
using System.Linq;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.Invoicing.Rules;
using FLS.Server.Service.Invoicing.Rules.InvoiceLineRules;
using FLS.Server.Service.RulesEngine;
using NLog;
using FLS.Data.WebApi.Invoicing.RuleFilters;

namespace FLS.Server.Service.Invoicing.RuleEngines
{
    internal class InvoiceLineRulesEngine
    {
        private Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private readonly RuleBasedFlightInvoiceDetails _flightInvoiceDetails;
        private readonly Flight _flight;
        private readonly List<InvoiceRuleFilterDetails> _invoiceRuleFilters;
        private readonly IPersonService _personService;

        public InvoiceLineRulesEngine(RuleBasedFlightInvoiceDetails flightInvoiceDetails, Flight flight, 
            IPersonService personService, List<InvoiceRuleFilterDetails> invoiceRuleFilters)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flight = flight;
            _invoiceRuleFilters = invoiceRuleFilters;
            _personService = personService;
        }

        public RuleBasedFlightInvoiceDetails Run()
        {
            Logger.Trace($"Start of InvoiceLineRulesEngine.Run()");
            var rules = new List<IRule<RuleBasedFlightInvoiceDetails>>();

            //No landing tax rule must be one of the first rule
            #region NO Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.NoLandingTaxInvoiceRuleFilter))
            {
                var rule = new NoLandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of NoLandingTaxRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion NO Landing taxes

            #region Aircraft in MasterFlight (Glider- or Motor-Flight)
            rules.Clear();

            foreach (var filter in _invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AircraftInvoiceRuleFilter))
            {
                var rule = new AircraftFlightTimeRule(_flight, filter);
                rules.Add(rule);
            }

            _flightInvoiceDetails.ActiveFlightTime = _flight.FlightDurationZeroBased.TotalMinutes;

            while (_flightInvoiceDetails.ActiveFlightTime > 0)
            {
                Logger.Trace($"Run {rules.Count} rules of AircraftRuleFilter, active flight time = {_flightInvoiceDetails.ActiveFlightTime}");
                _flightInvoiceDetails.ApplyRules(rules);

                if (rules.Any(x => x.RuleApplied) == false)
                {
                    Logger.Warn($"No aircraft mapping rule found for flight: {_flight}.");
                    break;
                }
            }
            #endregion Aircraft in MasterFlight (Glider- or Motor-Flight)

            #region Instructor fee
            rules.Clear();

            foreach (var filter in _invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.InstructorFeeInvoiceRuleFilter))
            {
                var rule = new InstructorFeeRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of InstructorFeeRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Instructor fee

            #region Aircraft in TowFlight
            //run rule engine again for towflight, before other rules were applied (because of order of invoice lines)
            if (_flight.TowFlight != null)
            {
                //set IncludesTowFlightId otherwise the flight state of towflight will not be set correctly during invoicing
                _flightInvoiceDetails.IncludesTowFlightId = _flight.TowFlightId;

                var invoiceLineRulesEngine = new InvoiceLineRulesEngine(_flightInvoiceDetails, _flight.TowFlight,
                    _personService, _invoiceRuleFilters);
                invoiceLineRulesEngine.Run();
            }
            #endregion Aircraft in TowFlight

            #region Additional Fuel Fee
            rules.Clear();

            foreach (var filter in _invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.AdditionalFuelFeeInvoiceRuleFilter))
            {
                var rule = new AdditionalFuelFeeRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of AdditionalFuelFeeRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Additional Fuel Fee

            #region Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.LandingTaxInvoiceRuleFilter))
            {
                var rule = new LandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of LandingTaxRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Landing taxes

            #region VSF fee
            rules.Clear();

            foreach (var filter in _invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.VsfFeeInvoiceRuleFilter))
            {
                var rule = new VsfFeeRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of VsfFeeRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion VSF fee

            return _flightInvoiceDetails;
        }
    }
}
