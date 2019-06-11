using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Customers.Engine
{
    [EntityIdentifier(CustomersConstants.Conditions.CurrentCustomerGoalConditionName)]
    public class CurrentCustomerGoalCondition : ICustomerCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> GoalId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var targetGoalId = GoalId?.Yield(context);
            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetGoalId))
                return false;

            var component = cart.GetComponent<CartContactBehaviourComponent>();
            return component.Goals.Contains(targetGoalId);
        }
    }
}
