using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Carts.Engine.Conditions
{
    [EntityIdentifier(CartsConstants.Conditions.CartAnyItemHasBrandCondition)]
    public class CartAnyItemHasBrandCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> Brand { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            return Brand.YieldCartLinesWithBrand(context).Any();
        }
    }

}
