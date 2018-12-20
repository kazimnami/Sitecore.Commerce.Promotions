using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;

namespace Feature.Carts.Engine.Conditions
{
    [EntityIdentifier("CartAnyItemHasTagCondition")]
    public class CartAnyItemHasTagCondition : ICartsCondition, ICondition, IMappableRuleEntity
    {
        public IRuleValue<string> Brand { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var brand = Brand.Yield(context);

            var cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(brand))
                return false;
            return cart.Lines.Any<CartLineComponent>(l => 
                l.GetComponent<CartProductComponent>().Tags.Any<Sitecore.Commerce.Core.Tag>(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase)));
        }
    }

}
