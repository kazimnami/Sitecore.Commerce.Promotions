using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(nameof(CartItemTargetBrandSubtotalPercentOffAction))]
    public class CartItemTargetBrandSubtotalPercentOffAction : BaseCartItemSubtotalPercentOffAction
    {
        public IRuleValue<string> TargetBrand { get; set; }

        protected override string NameOfBlock()
        {
            return nameof(CartItemTargetBrandSubtotalAmountOffAction);
        }

        protected override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetBrand.YieldCartLinesWithBrand(context);
        }
    }
}
