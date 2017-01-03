using System;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Service.Accounting.Rules
{
    internal class DeliveryRecipientRule : BaseAccountingRule
    {
        internal DeliveryRecipientRule(Flight flight, RuleBasedAccountingRuleFilterDetails accountingRecipientRuleFilter)
            : base(flight, accountingRecipientRuleFilter)
        {
        }
        
        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (AccountingRuleFilter != null && AccountingRuleFilter.RecipientTarget != null)
            {
                ruleBasedDelivery.RecipientDetails.RecipientName = AccountingRuleFilter.RecipientTarget.RecipientName;
                ruleBasedDelivery.RecipientDetails.PersonClubMemberNumber = AccountingRuleFilter.RecipientTarget.PersonClubMemberNumber;

                //accounting is created for club internal (customer already paid)
                ruleBasedDelivery.IsChargedToClubInternal = AccountingRuleFilter.IsChargedToClubInternal;
            }
            else
            {
                throw new Exception($"Recipient target is null. Can not create delivery for flight with Id: {ruleBasedDelivery.FlightInformation.FlightId}");
            }

            AccountingRuleFilter.HasMatched = true;
            return base.Apply(ruleBasedDelivery);
        }
    }
}
