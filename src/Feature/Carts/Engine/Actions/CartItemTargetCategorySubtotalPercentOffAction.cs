using Foundation.Carts.Engine;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Collections.Generic;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetCategorySubtotalPercentOffAction)]
    public class CartItemTargetCategorySubtotalPercentOffAction : BaseCartItemSubtotalPercentOffAction, ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IRuleValue<string> TargetCategorySitecoreId { get; set; }

        public IRuleValue<string> CategoryId { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetCategorySitecoreId.YieldCartLinesWithCategory(context);
        }
    }
}
