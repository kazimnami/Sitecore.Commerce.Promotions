using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace Feature.Carts.Engine.Conditions
{
    [EntityIdentifier(nameof(CartAnyItemHasBrandCondition))]
    public class CartAnyItemHasBrandCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> Brand { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var brand = Brand.Yield(context);

            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(brand))
                return false;
            return cart.Lines.Any(l => 
                l.GetComponent<LineItemProductExtendedComponent>().Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
        }
    }

}
