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
        private readonly List<BaseRuleFilter> _invoiceLineBaseRuleFilters;
        private readonly IPersonService _personService;

        public InvoiceLineRulesEngine(RuleBasedFlightInvoiceDetails flightInvoiceDetails, Flight flight, 
            IPersonService personService, List<BaseRuleFilter> invoiceLineBaseRuleFilters)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flight = flight;
            _invoiceLineBaseRuleFilters = invoiceLineBaseRuleFilters;
            _personService = personService;
        }

        public RuleBasedFlightInvoiceDetails Run()
        {
            Logger.Trace($"Start of InvoiceLineRulesEngine.Run()");
            var rules = new List<IRule<RuleBasedFlightInvoiceDetails>>();

            //No landing tax rule must be one of the first rule
            #region NO Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(NoLandingTaxRuleFilter)).ToList())
            {
                var rule = new NoLandingTaxRule(_flight, (NoLandingTaxRuleFilter)filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of NoLandingTaxRuleFilter");
            Logger.Trace($"Before apply rules: {_invoiceLineBaseRuleFilters.Count} and {_invoiceLineBaseRuleFilters.Count(x => x.GetType() == typeof(AircraftRuleFilter))} AircraftRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            Logger.Trace($"After apply rules: {_invoiceLineBaseRuleFilters.Count} and {_invoiceLineBaseRuleFilters.Count(x => x.GetType() == typeof(AircraftRuleFilter))} AircraftRuleFilter"); 
            #endregion NO Landing taxes

            #region Aircraft in MasterFlight (Glider- or Motor-Flight)
            rules.Clear();
            Logger.Trace($"After clearing rules: {_invoiceLineBaseRuleFilters.Count} and {_invoiceLineBaseRuleFilters.Count(x => x.GetType() == typeof(AircraftRuleFilter))} AircraftRuleFilter");

            foreach (var filter in _invoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(AircraftRuleFilter)).ToList())
            {
                var rule = new AircraftFlightTimeRule(_flight, (AircraftRuleFilter)filter);
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
            Logger.Trace($"After clearing rules of AircraftRuleFilter: {_invoiceLineBaseRuleFilters.Count} and {_invoiceLineBaseRuleFilters.Count(x => x.GetType() == typeof(AircraftRuleFilter))} AircraftRuleFilter");
            foreach (var filter in _invoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(InstructorFeeRuleFilter)).ToList())
            {
                var rule = new InstructorFeeRule(_flight, (InstructorFeeRuleFilter)filter);
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
                    _personService, _invoiceLineBaseRuleFilters);
                invoiceLineRulesEngine.Run();
            }
            #endregion Aircraft in TowFlight

            #region Additional Fuel Fee
            rules.Clear();

            foreach (var filter in _invoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(AdditionalFuelFeeRuleFilter)).ToList())
            {
                var rule = new AdditionalFuelFeeRule(_flight, (AdditionalFuelFeeRuleFilter)filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of AdditionalFuelFeeRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Additional Fuel Fee

            #region Landing taxes
            rules.Clear();

            foreach (var filter in _invoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(LandingTaxRuleFilter)).ToList())
            {
                var rule = new LandingTaxRule(_flight, (LandingTaxRuleFilter)filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of LandingTaxRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion Landing taxes

            #region VSF fee
            rules.Clear();

            foreach (var filter in _invoiceLineBaseRuleFilters.Where(x => x.GetType() == typeof(VsfFeeRuleFilter)).ToList())
            {
                var rule = new VsfFeeRule(_flight, (VsfFeeRuleFilter)filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of VsfFeeRuleFilter");
            _flightInvoiceDetails.ApplyRules(rules);
            #endregion VSF fee

            return _flightInvoiceDetails;
        }
    }
}
