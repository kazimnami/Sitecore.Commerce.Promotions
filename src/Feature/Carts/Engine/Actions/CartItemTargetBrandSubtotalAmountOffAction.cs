using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(Constants.CartItemTargetBrandSubtotalAmountOffAction)]
    public class CartItemTargetBrandSubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction
    {
        public IRuleValue<string> TargetBrand { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetBrand.YieldCartLinesWithBrand(context);
        }
    }
}