using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.Carts.Engine
{
    public abstract class BaseCartItemSubtotalPercentOffAction : ICartLineAction, ICartsAction, IAction, IMappableRuleEntity
    {
        public IRuleValue<decimal> Subtotal { get; set; }

        public IBinaryOperator<decimal, decimal> Operator { get; set; }

        public IRuleValue<decimal> PercentOff { get; set; }

        protected abstract IEnumerable<CartLineComponent> MatchingLines(IRuleExecutionContext context);

        protected abstract string NameOfBlock();

        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>(null);
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || Operator == null || totals == null || !totals.Lines.Any())
                return;

            var list = this.MatchingLines(context).Where(l =>
                Operator.Evaluate(l.Totals.SubTotal.Amount, Subtotal.Yield(context))
                && l.Quantity != decimal.Zero).ToList();
            if (!list.Any())
                return;

            var className = NameOfBlock();
            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            var discountAdjustmentType = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;

            foreach (var line in list)
            {
                var discountAmount = PercentOff.Yield(context) * 0.01M * totals.Lines[line.Id].SubTotal.Amount;
                if (commerceContext.GetPolicy<GlobalPricingPolicy>().ShouldRoundPriceCalc)
                    discountAmount = decimal.Round(discountAmount, commerceContext.GetPolicy<GlobalPricingPolicy>().RoundDigits, commerceContext.GetPolicy<GlobalPricingPolicy>().MidPointRoundUp ? MidpointRounding.AwayFromZero : MidpointRounding.ToEven);
                discountAmount *= decimal.MinusOne;

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
