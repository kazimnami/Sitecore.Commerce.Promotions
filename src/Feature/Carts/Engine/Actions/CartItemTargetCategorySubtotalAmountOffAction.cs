using Foundation.Carts.Engine;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Collections.Generic;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetCategorySubtotalAmountOffAction)]
    public class CartItemTargetCategorySubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction, ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IRuleValue<string> TargetCategorySitecoreId { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetCategorySitecoreId.YieldCartLinesWithCategory(context);
        }
    }
}