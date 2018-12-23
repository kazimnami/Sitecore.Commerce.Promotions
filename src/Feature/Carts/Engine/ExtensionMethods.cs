using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    public static class ExtensionMethods
    {
        public static IEnumerable<CartLineComponent> YieldCartLinesWithTag(this IRuleValue<string> ruleValue, IRuleExecutionContext context)
        {
            string targetTag = ruleValue?.Yield(context);
            Cart cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetTag))
                return Enumerable.Empty<CartLineComponent>();

            return cart.Lines.Where(l =>
                l.GetComponent<CartProductComponent>().Tags.Any(t =>
                    t.Name.Equals(targetTag, StringComparison.OrdinalIgnoreCase)));
        }

        public static IEnumerable<CartLineComponent> YieldCartLinesWithBrand(this IRuleValue<string> ruleValue, IRuleExecutionContext context)
        {
            string targetBrand = ruleValue?.Yield(context);
            Cart cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetBrand))
                return Enumerable.Empty<CartLineComponent>();

            return cart.Lines.Where(l =>
                l.GetComponent<LineItemProductExtendedComponent>().Brand.Equals(targetBrand, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<CartLineComponent> YieldCartLinesWithCategory(this IRuleValue<string> ruleValue, IRuleExecutionContext context)
        {
            string targetCategory = ruleValue?.Yield(context);
            Cart cart = context.Fact<CommerceContext>()?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any() || string.IsNullOrEmpty(targetCategory))
                return Enumerable.Empty<CartLineComponent>();

            return cart.Lines.Where(l =>
                (l.GetComponent<LineItemProductExtendedComponent>().ParentCategoryList.IndexOf(targetCategory, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}
