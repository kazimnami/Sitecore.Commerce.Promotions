using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamplePromotions.Feature.Promotions.Engine
{
    [EntityIdentifier(PromotionsConstants.Conditions.ItemsCollectionCondition)]
    public class ItemsCollectionCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public bool Evaluate(IRuleExecutionContext context)
        {
            return context.YieldCartLinesWithItemsCollection().Any();
        }
    }
}
