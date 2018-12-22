using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(Constants.CartItemTargetCategorySubtotalAmountOffAction)]
    public class CartItemTargetCategorySubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction, ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IRuleValue<string> TargetCategorySitecoreId { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetCategorySitecoreId.YieldCartLinesWithCategory(context);
        }
    }
}