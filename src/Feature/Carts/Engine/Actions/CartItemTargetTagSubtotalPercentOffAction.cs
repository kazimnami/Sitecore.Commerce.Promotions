using SamplePromotions.Foundation.Carts.Engine;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Collections.Generic;

namespace SamplePromotions.Feature.Carts.Engine
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetTagSubtotalPercentOffAction)]
    public class CartItemTargetTagSubtotalPercentOffAction : BaseCartItemSubtotalPercentOffAction
    {
        public IRuleValue<string> TargetTag { get; set; }

        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return TargetTag.YieldCartLinesWithTag(context);
        }
    }
}
