using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(nameof(CartItemTargetCategorySubtotalAmountOffAction))]
    public class CartItemTargetCategorySubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction
    {
        public IRuleValue<string> TargetTag { get; set; }

        protected override string NameOfBlock()
        {
            return nameof(CartItemTargetCategorySubtotalAmountOffAction);
        }

        protected override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetTag.YieldCartLinesWithCategory(context);
        }
    }
}