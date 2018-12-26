using Foundation.Carts.Engine;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Collections.Generic;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetTagSubtotalAmountOffAction)]
    public class CartItemTargetTagSubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction
    {
        public IRuleValue<string> TargetTag { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetTag.YieldCartLinesWithTag(context);
        }
    }
}