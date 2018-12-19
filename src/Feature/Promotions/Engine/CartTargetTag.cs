using Sitecore.Framework.Rules;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Promotions.Engine
{
    public abstract class CartTargetTag
    {
        public IRuleValue<string> TargetTag { get; set; }

        protected virtual IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context)
        {
            string targetTag = TargetTag.Yield(context);
            Cart cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetTag))
                return Enumerable.Empty<CartLineComponent>();
            
            return cart.Lines.Where(l => 
                l.GetComponent<CartProductComponent>().Tags.Any(t => 
                    t.Name.Equals(targetTag, StringComparison.OrdinalIgnoreCase)));
        }
    }
}