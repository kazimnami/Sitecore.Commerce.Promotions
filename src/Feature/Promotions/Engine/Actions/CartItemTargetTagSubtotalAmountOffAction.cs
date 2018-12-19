using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Promotions.Engine
{
    [EntityIdentifier("CartItemTargetTagSubtotalAmountOffAction")]
    public class CartItemTargetTagSubtotalAmountOffAction : CartTargetTag, ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IRuleValue<decimal> AmountOff { get; set; }

        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any())
                return;

            var list = this.MatchingLines(context).ToList();
            if (!list.Any())
                return;

            var className = nameof(CartItemTargetTagSubtotalAmountOffAction);
            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            var discountAdjustmentType = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;

            var discountAmount = this.AmountOff.Yield(context);
            if (commerceContext.GetPolicy<GlobalPricingPolicy>().ShouldRoundPriceCalc)
                discountAmount = decimal.Round(discountAmount, commerceContext.GetPolicy<GlobalPricingPolicy>().RoundDigits, commerceContext.GetPolicy<GlobalPricingPolicy>().MidPointRoundUp ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven);
            discountAmount *= decimal.MinusOne;

            foreach (var line in list)
            {
                if (!totals.Lines.ContainsKey(line.Id))
                    return;

                line.Adjustments.Add(new CartLineLevelAwardedAdjustment()
                {
                    Name = (propertiesModel?.GetPropertyValue("PromotionText") as string ?? discountAdjustmentType),
                    DisplayName = (propertiesModel?.GetPropertyValue("PromotionCartText") as string ?? discountAdjustmentType),
                    Adjustment = new Money(commerceContext.CurrentCurrency(), discountAmount),
                    AdjustmentType = discountAdjustmentType,
                    IsTaxable = false,
                    AwardingBlock = className
                });

                totals.Lines[line.Id].SubTotal.Amount = totals.Lines[line.Id].SubTotal.Amount + discountAmount;
                line.GetComponent<MessagesComponent>().AddMessage(commerceContext.GetPolicy<KnownMessageCodePolicy>().Promotions, string.Format("PromotionApplied: {0}", propertiesModel?.GetPropertyValue("PromotionId") ?? className));
            };
        }
    }
}