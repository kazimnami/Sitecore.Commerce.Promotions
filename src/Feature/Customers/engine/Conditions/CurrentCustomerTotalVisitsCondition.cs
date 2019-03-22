using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Customers;
using Sitecore.Framework.Rules;
using System.Linq;

namespace Feature.Customers.Engine
{
    [EntityIdentifier(CustomersConstants.Conditions.CurrentCustomerTotalVisitsConditionName)]
    public class CurrentCustomerTotalVisitsCondition : ICustomerCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<int> TotalVisits { get; set; }
        public IBinaryOperator<int, int> Operator { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            if (context == null || Operator == null)
                return false;

            var targetTotalVisits = TotalVisits?.Yield(context);
            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || targetTotalVisits != null)
                return false;

            var component = cart.GetComponent<CartContactBehaviourComponent>();
            return Operator.Evaluate(component.TotalVisits, targetTotalVisits.Value);
        }
    }
}
