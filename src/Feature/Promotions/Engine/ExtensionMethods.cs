using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamplePromotions.Feature.Promotions.Engine
{
    public static class ExtensionMethods
    {
        public static IEnumerable<CartLineComponent> YieldCartLinesWithItemsCollection(this IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>(null);
            var cart = commerceContext?.GetObject<Cart>();
            if (cart == null || !cart.Lines.Any())
                return Enumerable.Empty<CartLineComponent>();

            var items = new List<PromotionItemModel>();
            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            if ((propertiesModel?.GetPropertyValue("PromotionItems") is PromotionItemsComponent))
            {
                items = ((PromotionItemsComponent)propertiesModel.GetPropertyValue("PromotionItems")).Items;
            }

            var promotionIncludedItems = items.Where(i => !i.Excluded).Select(i => i.ItemId).ToList();
            var promotionExcludedItems = items.Where(i => i.Excluded).Select(i => i.ItemId).ToList();
            var list = cart.Lines.Select(l => l.ItemId).ToList();

            if (promotionIncludedItems.Any())
            {
                list = list.Intersect(promotionIncludedItems, StringComparer.OrdinalIgnoreCase).ToList();
            }

            list = list.Except(promotionExcludedItems, StringComparer.OrdinalIgnoreCase).ToList();

            return cart.Lines.Where(l => list.Contains(l.ItemId));
        }
    }
}
