using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Service.Accounting.Rules.ItemRules;
using FLS.Server.Service.RulesEngine;
using NLog;
using AccountingRuleFilterType = FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType;

namespace FLS.Server.Service.Accounting.RuleEngines
{
    internal class DeliveryItemRulesEngine
    {
        private Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private readonly RuleBasedDeliveryDetails _ruleBasedDelivery;
        private readonly Flight _flight;
        private readonly List<RuleBasedAccountingRuleFilterDetails> _accountingRuleFilters;
        private readonly IPersonService _personService;

        public DeliveryItemRulesEngine(RuleBasedDeliveryDetails ruleBasedDelivery, Flight flight, 
            IPersonService personService, List<RuleBasedAccountingRuleFilterDetails> accountingRuleFilters)
        {
            _ruleBasedDelivery = ruleBasedDelivery;
            _flight = flight;
            _accountingRuleFilters = accountingRuleFilters;
            _personService = personService;
        }

        public RuleBasedDeliveryDetails Run()
        {
            Logger.Trace($"Start of DeliveryItemRulesEngine.Run()");
            var rules = new List<IRule<RuleBasedDeliveryDetails>>();

            //No landing tax rule must be one of the first rule
            #region NO Landing taxes
            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.NoLandingTaxAccountingRuleFilter))
            {
                var rule = new NoLandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of NoLandingTaxRuleFilter");
            _ruleBasedDelivery.ApplyRules(rules);
            #endregion NO Landing taxes

            #region Aircraft in MasterFlight (Glider- or Motor-Flight)
            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.AircraftAccountingRuleFilter))
            {
                var rule = new AircraftFlightTimeRule(_flight, filter);
                rules.Add(rule);
            }

            _ruleBasedDelivery.ActiveFlightTime = _flight.FlightDurationZeroBased.TotalMinutes;

            while (_ruleBasedDelivery.ActiveFlightTime > 0)
            {
                Logger.Trace($"Run {rules.Count} rules of AircraftRuleFilter, active flight time = {_ruleBasedDelivery.ActiveFlightTime}");
                _ruleBasedDelivery.ApplyRules(rules);

                if (rules.Any(x => x.RuleApplied) == false)
                {
                    Logger.Warn($"No aircraft mapping rule found for flight: {_flight}.");
                    break;
                }
            }
            #endregion Aircraft in MasterFlight (Glider- or Motor-Flight)

            #region Instructor fee
            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.InstructorFeeAccountingRuleFilter))
            {
                var rule = new InstructorFeeRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of InstructorFeeRuleFilter");
            _ruleBasedDelivery.ApplyRules(rules);
            #endregion Instructor fee

            #region Aircraft in TowFlight
            //run rule engine again for towflight, before other rules were applied (because of order of delivery lines)
            if (_flight.TowFlight != null)
            {
                var deliveryItemRulesEngine = new DeliveryItemRulesEngine(_ruleBasedDelivery, _flight.TowFlight,
                    _personService, _accountingRuleFilters);
                deliveryItemRulesEngine.Run();
            }
            #endregion Aircraft in TowFlight

            #region Additional Fuel Fee
            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.AdditionalFuelFeeAccountingRuleFilter))
            {
                var rule = new AdditionalFuelFeeRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of AdditionalFuelFeeRuleFilter");
            _ruleBasedDelivery.ApplyRules(rules);
            #endregion Additional Fuel Fee

            #region Landing taxes
            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.LandingTaxAccountingRuleFilter))
            {
                var rule = new LandingTaxRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of LandingTaxRuleFilter");
            _ruleBasedDelivery.ApplyRules(rules);
            #endregion Landing taxes

            #region VSF fee
            rules.Clear();

            foreach (var filter in _accountingRuleFilters.Where(x => x.AccountingRuleFilterTypeId == (int)AccountingRuleFilterType.VsfFeeAccountingRuleFilter))
            {
                var rule = new VsfFeeRule(_flight, filter);
                rules.Add(rule);
            }

            Logger.Trace($"Run {rules.Count} rules of VsfFeeRuleFilter");
            _ruleBasedDelivery.ApplyRules(rules);
            #endregion VSF fee

            return _ruleBasedDelivery;
        }
    }
}
