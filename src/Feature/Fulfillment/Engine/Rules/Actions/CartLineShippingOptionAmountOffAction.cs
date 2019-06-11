// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartLineShippingOptionAmountOffAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SamplePromotions.Feature.Fulfillment.Engine.Rules.Actions
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Carts;
    using Sitecore.Commerce.Plugin.Fulfillment;
    using Sitecore.Commerce.Plugin.Pricing;
    using Sitecore.Framework.Rules;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Applies a discount to cart line fulfillments of the specified type
    /// </summary>
    [EntityIdentifier(nameof(CartLineShippingOptionAmountOffAction))]
    public class CartLineShippingOptionAmountOffAction : ICartLineAction
    {
        protected CommerceCommander Commander { get; set; }

        public CartLineShippingOptionAmountOffAction(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        public IRuleValue<string> FulfillmentOptionName { get; set; }

        public IRuleValue<decimal> AmountOff { get; set; }

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
            if (cart == null || !cart.Lines.Any() || !cart.HasComponent<SplitFulfillmentComponent>() || totals == null || !totals.Lines.Any())
            {
                return;
            }

            var optionName = FulfillmentOptionName.Yield(context);
            var amountOff = AmountOff.Yield(context);
            if (string.IsNullOrWhiteSpace(optionName) || amountOff <= 0)
            {
                return;
            }

            var methods = Task.Run(() => Commander.Command<GetFulfillmentMethodsCommand>().Process(commerceContext)).Result
                .Where(o => o.FulfillmentType.Equals(optionName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!methods.Any())
            {
                return;
            }

            var lines = cart.Lines.Where(line => {
                if (!line.HasComponent<FulfillmentComponent>())
                {
                    return false;
                }

                var fulfillment = line.GetComponent<FulfillmentComponent>();
                if (!string.IsNullOrEmpty(fulfillment.FulfillmentMethod?.EntityTarget)
                    && !string.IsNullOrEmpty(fulfillment.FulfillmentMethod?.Name))
                {
                    return methods.Any(m =>
                    {
                        if (m.Id.Equals(fulfillment.FulfillmentMethod.EntityTarget, StringComparison.OrdinalIgnoreCase))
                        {
                            return m.Name.Equals(fulfillment.FulfillmentMethod.Name, StringComparison.OrdinalIgnoreCase);
                        }

                        return false;
                    });
                }

                return false;
            });
            
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

                var fulfillmentFee = line.Adjustments.FirstOrDefault(a => a.Name.Equals("FulfillmentFee", StringComparison.OrdinalIgnoreCase));
                if (fulfillmentFee == null)
                {
                    return;
                }

                var discountValue = amountOff > fulfillmentFee.Adjustment.Amount ? fulfillmentFee.Adjustment.Amount : amountOff;
                if (discountValue == Decimal.Zero)
                {
                    return;
                }

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
                    AwardingBlock = nameof(CartLineShippingOptionAmountOffAction)
                });
                totals.Lines[line.Id].SubTotal.Amount += discountValue;
                line.GetComponent<MessagesComponent>().AddMessage(
                    commerceContext.GetPolicy<KnownMessageCodePolicy>().Promotions,
                    $"PromotionApplied: {propertiesModel?.GetPropertyValue("PromotionId") ?? nameof(CartLineShippingOptionAmountOffAction)}");
            });
        }
    }
}