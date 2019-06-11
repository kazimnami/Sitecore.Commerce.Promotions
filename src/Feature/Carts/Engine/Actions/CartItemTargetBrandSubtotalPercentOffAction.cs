using SamplePromotions.Foundation.Carts.Engine;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Collections.Generic;

namespace SamplePromotions.Feature.Carts.Engine
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetBrandSubtotalPercentOffAction)]
    public class CartItemTargetBrandSubtotalPercentOffAction : BaseCartItemSubtotalPercentOffAction
    {
        public IRuleValue<string> TargetBrand { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetBrand.YieldCartLinesWithBrand(context);
        }
    }
}
