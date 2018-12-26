using Foundation.Carts.Engine;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Promotions.Engine
{
    [EntityIdentifier(PromotionsConstants.Actions.CartItemTargetItemsCollectionSubtotalPercentOffAction)]
    public class CartItemTargetItemsCollectionSubtotalPercentOffAction : BaseCartItemSubtotalPercentOffAction
    {
        public override IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            return context.YieldCartLinesWithItemsCollection();
        }
    }
}
