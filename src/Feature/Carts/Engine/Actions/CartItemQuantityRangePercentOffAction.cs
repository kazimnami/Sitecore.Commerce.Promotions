// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartItemQuantityRangePercentOffAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SamplePromotions.Feature.Carts.Engine
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Carts;
    using Sitecore.Commerce.Plugin.Pricing;
    using Sitecore.Framework.Rules;
    using System;
    using System.Linq;

    /// <inheritdoc />
    /// <summary>
    /// Applies a percentage discount to target cart line item with quantity range
    /// </summary>
    [EntityIdentifier(nameof(CartItemQuantityRangePercentOffAction))]
    public class CartItemQuantityRangePercentOffAction : CartTargetItemId, ICartLineAction
    {
        public IRuleValue<decimal> MinQuantity { get; set; }

        public IRuleValue<decimal> MaxQuantity { get; set; }

        public IRuleValue<decimal> PercentOff { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="context">
        /// The rule execution context.
        /// </param>
        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();
            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any())
            {
                return;
            }

            var minQuantity = MinQuantity.Yield(context);
            var maxQuantity = MaxQuantity.Yield(context);
            var percentOff = PercentOff.Yield(context);
            if (minQuantity <= 0 || maxQuantity <= 0 || minQuantity > maxQuantity || percentOff <= 0)
            {
                return;
            }

            var lines = this.MatchingLines(context)
                .Where(line => line.Quantity >= minQuantity && line.Quantity <= maxQuantity).ToList();

            if (!lines.Any())
            {
                return;
            }

            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            var discount = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;

            lines.ForEach(line =>
            {
                if (!totals.Lines.ContainsKey(line.Id) || !line.HasPolicy<PurchaseOptionMoneyPolicy>())
                {
                    return;
                }

                var discountValue = percentOff * 0.01M * totals.Lines[line.Id].SubTotal.Amount;

                if (commerceContext.GetPolicy<GlobalPricingPolicy>().ShouldRoundPriceCalc)
                {
                    discountValue = decimal.Round(
                            discountValue,
                            commerceContext.GetPolicy<GlobalPricingPolicy>().RoundDigits,
                            commerceContext.GetPolicy<GlobalPricingPolicy>().MidPointRoundUp ?
                                MidpointRounding.AwayFromZero :
                                MidpointRounding.ToEven);
                }

                discountValue *= decimal.MinusOne;
                line.Adjustments.Add(new CartLineLevelAwardedAdjustment()
                {
                    Name = (propertiesModel?.GetPropertyValue("PromotionText") as string ?? discount),
                    DisplayName = (propertiesModel?.GetPropertyValue("PromotionCartText") as string ?? discount),
                    Adjustment = new Money(commerceContext.CurrentCurrency(), discountValue),
                    AdjustmentType = discount,
                    IsTaxable = false,
                    AwardingBlock = nameof(CartItemQuantityRangePercentOffAction)
                });
                totals.Lines[line.Id].SubTotal.Amount += discountValue;
                line.GetComponent<MessagesComponent>().AddMessage(
                    commerceContext.GetPolicy<KnownMessageCodePolicy>().Promotions,
                    $"PromotionApplied: {propertiesModel?.GetPropertyValue("PromotionId") ?? nameof(CartItemQuantityRangePercentOffAction)}");
            });
        }
    }
}