using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Customers.Engine
{
    [EntityIdentifier(CustomersConstants.Conditions.CurrentCustomerOutcomeConditionName)]
    public class CurrentCustomerOutcomeCondition : ICustomerCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> OutcomeId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var targetOutcomeId = OutcomeId?.Yield(context);
            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetOutcomeId))
                return false;

            var component = cart.GetComponent<CartContactBehaviourComponent>();
            return component.Outcomes.Contains(targetOutcomeId);
        }
    }
}
