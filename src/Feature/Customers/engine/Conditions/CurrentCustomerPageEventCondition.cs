using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Customers.Engine
{
    [EntityIdentifier(CustomersConstants.Conditions.CurrentCustomerPageEventConditionName)]
    public class CurrentCustomerPageEventCondition : ICustomerCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> PageEventId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var targetPageEventId = PageEventId?.Yield(context);
            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetPageEventId))
                return false;

            var component = cart.GetComponent<CartContactBehaviourComponent>();
            return component.PageEvents.Contains(targetPageEventId);
        }
    }
}
