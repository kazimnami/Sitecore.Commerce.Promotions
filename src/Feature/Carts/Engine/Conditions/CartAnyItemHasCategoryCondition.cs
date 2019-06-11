using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Carts.Engine.Conditions
{
    [EntityIdentifier(CartsConstants.Conditions.CartAnyItemHasCategoryCondition)]
    public class CartAnyItemHasCategoryCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> TargetCategorySitecoreId { get; set; }

        public IRuleValue<string> CategoryId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            return TargetCategorySitecoreId.YieldCartLinesWithCategory(context).Any();
        }
    }

}
