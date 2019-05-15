// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CartShippingOptionAmountOffAction.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Fulfillment.Engine.Rules.Actions
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Carts;
    using Sitecore.Commerce.Plugin.Fulfillment;
    using Sitecore.Commerce.Plugin.Pricing;
    using Sitecore.Framework.Rules;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Applies a discount to cart fulfillments of the specified type
    /// </summary>
    [EntityIdentifier(nameof(CartShippingOptionAmountOffAction))]
    public class CartShippingOptionAmountOffAction : ICartAction
    {
        protected CommerceCommander Commander { get; set; }

        public CartShippingOptionAmountOffAction(CommerceCommander commander)
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
            if (cart == null || !cart.Lines.Any())
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

            var fulfillment = cart.HasComponent<FulfillmentComponent>() ? cart.GetComponent<FulfillmentComponent>() : null;
            var fulfillmentMethod = fulfillment?.FulfillmentMethod;
            if (fulfillmentMethod == null || string.IsNullOrEmpty(fulfillmentMethod.EntityTarget) || string.IsNullOrEmpty(fulfillmentMethod.Name))
            {
                return;
            }

            var fulfillmentFee = cart.Adjustments.FirstOrDefault(a => a.Name.Equals("FulfillmentFee", StringComparison.OrdinalIgnoreCase));
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
                            MidpointRounding.ToEven
                    );
            }

            var discount = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;
            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            var amount = discountValue * decimal.MinusOne;
            cart.Adjustments.Add(new CartLevelAwardedAdjustment()
            {
                Name = propertiesModel?.GetPropertyValue("PromotionText") as string ?? discount,
                DisplayName = propertiesModel?.GetPropertyValue("PromotionCartText") as string ?? discount,
                Adjustment = new Money(commerceContext.CurrentCurrency(), amount),
                AdjustmentType = discount,
                IsTaxable = false,
                AwardingBlock = nameof(CartShippingOptionAmountOffAction)
            });

            cart.GetComponent<MessagesComponent>().AddMessage(
                commerceContext.GetPolicy<KnownMessageCodePolicy>().Promotions,
                $"PromotionApplied: {propertiesModel?.GetPropertyValue("PromotionId") ?? nameof(CartShippingOptionAmountOffAction)}"
            );
        }
    }
}