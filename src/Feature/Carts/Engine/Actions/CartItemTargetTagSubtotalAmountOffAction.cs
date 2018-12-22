using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(Constants.CartItemTargetTagSubtotalAmountOffAction)]
    public class CartItemTargetTagSubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction
    {
        public IRuleValue<string> TargetTag { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetTag.YieldCartLinesWithTag(context);
        }
    }
}