using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Customers.Engine
{
    [EntityIdentifier(CustomersConstants.Conditions.CurrentCustomerCampaignConditionName)]
    public class CurrentCustomerCampaignCondition : ICustomerCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> CampaignId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var targetCampaignId = CampaignId?.Yield(context);
            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetCampaignId))
                return false;

            var component = cart.GetComponent<CartContactBehaviourComponent>();
            return component.CampaignIds.Contains(targetCampaignId.ToLower());
        }
    }
}
