// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplyFreeGiftDiscountCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;

namespace Feature.Carts.Engine.Commands
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using System;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Defines the ApplyFreeGiftDiscountCommand command.
    /// </summary>
    public class ApplyFreeGiftDiscountCommand : CommerceCommand, IApplyFreeGiftDiscountCommand
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Feature.Carts.Engine.Commands.ApplyFreeGiftDiscountCommand" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public ApplyFreeGiftDiscountCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="cartLineComponent"></param>
        /// <param name="awardingAction"></param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public void Process(CommerceContext commerceContext, [NotNull] CartLineComponent cartLineComponent, [NotNull] string awardingAction)
        {
            using (CommandActivity.Start(commerceContext, this))
            {
                var discountAdjustmentType = commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount;
                var propertiesModel = commerceContext.GetObject<PropertiesModel>();
                var totals = commerceContext?.GetObject<CartTotals>();
                var discountAmount = cartLineComponent.UnitListPrice.Amount * decimal.MinusOne;

                cartLineComponent.Adjustments.Add(new CartLineLevelAwardedAdjustment
                {
                    Name = (propertiesModel?.GetPropertyValue("PromotionText") as string ?? discountAdjustmentType),
                    DisplayName = (propertiesModel?.GetPropertyValue("PromotionCartText") as string ?? discountAdjustmentType),
                    Adjustment = new Money(commerceContext.CurrentCurrency(), discountAmount),
                    AdjustmentType = discountAdjustmentType,
                    IsTaxable = false,
                    AwardingBlock = awardingAction
                });

                totals.Lines[cartLineComponent.Id].SubTotal.Amount = totals.Lines[cartLineComponent.Id].SubTotal.Amount + discountAmount;
                cartLineComponent.GetComponent<MessagesComponent>().AddMessage(commerceContext.GetPolicy<KnownMessageCodePolicy>().Promotions, string.Format("PromotionApplied: {0}", propertiesModel?.GetPropertyValue("PromotionId") ?? awardingAction));
            }
        }
    }
}