using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Carts.Engine
{
    public abstract class BaseCartItemSubtotalAmountOffAction : ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IBinaryOperator<decimal, decimal> SubtotalOperator { get; set; }

        public IRuleValue<decimal> Subtotal { get; set; }

        public IRuleValue<decimal> AmountOff { get; set; }

        public abstract IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context);

        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any() || SubtotalOperator == null || Subtotal == null || AmountOff == null)
                return;

            var amountOff = AmountOff.Yield(context);
            if (amountOff == 0)
                return;

            var matches = this.MatchingLines(context);
            if (matches == null || !matches.Any())
                return;

            var list = matches.Where(l =>
                SubtotalOperator.Evaluate(l.Totals.SubTotal.Amount, Subtotal.Yield(context))
                && l.Quantity != decimal.Zero).ToList();
            if (!list.Any())
                return;

            var className = this.GetType().Name;
            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            var discountAdjustmentType = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;

            var discountAmount = amountOff;
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