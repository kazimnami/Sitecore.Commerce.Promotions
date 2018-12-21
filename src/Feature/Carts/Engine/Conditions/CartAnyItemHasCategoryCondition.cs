using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace Feature.Carts.Engine.Conditions
{
    [EntityIdentifier(nameof(CartAnyItemHasCategoryCondition))]
    public class CartAnyItemHasCategoryCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> CategoryItemId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            return CategoryItemId.YieldCartLinesWithCategory(context).Any();
        }
    }

}
