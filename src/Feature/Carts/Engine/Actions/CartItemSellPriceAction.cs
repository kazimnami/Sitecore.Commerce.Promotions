using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamplePromotions.Feature.Carts.Engine.Actions
{
    public class CartItemSellPriceAction : CartTargetItemId, ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IRuleValue<Decimal> SellPrice { get; set; }

        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>(null);
            var cart = commerceContext?.GetObjects<Cart>().FirstOrDefault();
            var totals = commerceContext?.GetObjects<CartTotals>().FirstOrDefault();
            if (cart == null || !cart.Lines.Any() || (totals == null || !totals.Lines.Any()))
            {
                return;
            }

            var list = MatchingLines(context).ToList();
            if (!list.Any())
            {
                return;
            }

            list.ForEach(line =>
            {
                if (!totals.Lines.ContainsKey(line.Id) || !line.HasPolicy<PurchaseOptionMoneyPolicy>())
                {
                    return;
                }

                var propertiesModel = commerceContext.GetObject<PropertiesModel>();
                var discount = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;
                Decimal d = SellPrice.Yield(context);
                if (commerceContext.GetPolicy<GlobalPricingPolicy>().ShouldRoundPriceCalc)
                {
                    d = Decimal.Round(d,
                            commerceContext.GetPolicy<GlobalPricingPolicy>().RoundDigits,
                            commerceContext.GetPolicy<GlobalPricingPolicy>().MidPointRoundUp
                                ? MidpointRounding.AwayFromZero
                                : MidpointRounding.ToEven);
                }

                var amount = (line.GetPolicy<PurchaseOptionMoneyPolicy>().SellPrice.Amount - d) * Decimal.MinusOne;
                line.Adjustments.Add(new CartLineLevelAwardedAdjustment()
                {
                    Name = (propertiesModel?.GetPropertyValue("PromotionText") as string ?? discount),
                    DisplayName = (propertiesModel?.GetPropertyValue("PromotionCartText") as string ?? discount),
                    Adjustment = new Money(commerceContext.CurrentCurrency(), amount),
                    AdjustmentType = discount,
                    IsTaxable = false,
                    AwardingBlock = nameof(CartItemSellPriceAction)
                });
                line.GetPolicy<PurchaseOptionMoneyPolicy>().SellPrice.Amount = d;
                totals.Lines[line.Id].SubTotal.Amount = d;

                line.GetComponent<MessagesComponent>().AddMessage(commerceContext.GetPolicy<KnownMessageCodePolicy>().Promotions, string.Format("PromotionApplied: {0}", propertiesModel?.GetPropertyValue("PromotionId") ?? nameof(CartItemSellPriceAction)));
            });
        }
    }
}
