using Foundation.Carts.Engine;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Collections.Generic;

namespace Feature.Carts.Engine
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetBrandSubtotalAmountOffAction)]
    public class CartItemTargetBrandSubtotalAmountOffAction : BaseCartItemSubtotalAmountOffAction
    {
        public IRuleValue<string> TargetBrand { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetBrand.YieldCartLinesWithBrand(context);
        }
    }
}