using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Rules;
using System.Linq;

namespace Feature.Customers.Engine
{
    [EntityIdentifier(CustomersConstants.Conditions.CurrentCustomerEngagementValueConditionName)]
    public class CurrentCustomerEngagementValueCondition : ICustomerCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<int> EngagementValue { get; set; }
        public IBinaryOperator<int, int> Operator { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            if (context == null || Operator == null)
                return false;

            var targetEngagementValue = EngagementValue?.Yield(context);
            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || targetEngagementValue != null)
                return false;

            var component = cart.GetComponent<CartContactBehaviourComponent>();
            return Operator.Evaluate(component.EngagementValue, targetEngagementValue.Value);
        }
    }
}
