using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace Feature.Carts.Engine.Conditions
{
    [EntityIdentifier(CartsConstants.Conditions.CartAnyItemHasCategoryCondition)]
    public class CartAnyItemHasCategoryCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> CategorySitecoreId { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            return CategorySitecoreId.YieldCartLinesWithCategory(context).Any();
        }
    }

}
