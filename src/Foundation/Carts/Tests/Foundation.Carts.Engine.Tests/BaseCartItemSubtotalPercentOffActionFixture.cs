using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

//using Sitecore.Commerce.UnitTesting;

namespace SamplePromotions.Foundation.Carts.Engine.Tests
{
    public class BaseCartItemSubtotalPercentOffActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
            BaseCartItemSubtotalPercentOffAction action,
            IBinaryOperator<decimal, decimal> subtotalOperator,
            IRuleValue<decimal> subtotal,
            IRuleValue<decimal> percentOff,
            Cart cart,
            IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_02_NoCart(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_03_NoCartLines(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.Clear();

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_04_NoCartTotals(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_05_NoCartTotalLines(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                cartTotals.Lines.Clear();

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_06_NoSubtotalOperator(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = null;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_07_NoSubtotal(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = null;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_08_NoPercent(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = null;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_09_ZeroPercent(
                 BaseCartItemSubtotalPercentOffAction action,
                 IBinaryOperator<decimal, decimal> subtotalOperator,
                 IRuleValue<decimal> subtotal,
                 IRuleValue<decimal> percentOff,
                 Cart cart,
                 CartTotals cartTotals,
                 CommerceContext commerceContext,
                 IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                percentOff.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_10_NoMatchingLines(
                BaseCartItemSubtotalPercentOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(Enumerable.Empty<CartLineComponent>());
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }
        }
        public class Functional
        {
            [Theory, AutoNSubstituteData]
            public void Execute_DecimalEqualityOperator_True(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
                globalPricingPolicy.ShouldRoundPriceCalc = false;
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                var cartline = cart.Lines[1];
                cartline.Totals.SubTotal.Amount = 150;
                var cartTotals = new CartTotals(cart);
                subtotal.Yield(context).ReturnsForAnyArgs(150);
                percentOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalEqualityOperator();
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
                cart.Adjustments.Should().BeEmpty();
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(112.5M);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_DecimalEqualityOperator_False(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
                globalPricingPolicy.ShouldRoundPriceCalc = false;
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                var cartline = cart.Lines[1];
                var cartTotals = new CartTotals(cart);
                subtotal.Yield(context).ReturnsForAnyArgs(150);
                percentOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalEqualityOperator();
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(100);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_DecimalGreaterThanEqualToOperator_True(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
                globalPricingPolicy.ShouldRoundPriceCalc = false;
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                var cartline = cart.Lines[1];
                cartline.Totals.SubTotal.Amount = 175;
                var cartTotals = new CartTotals(cart);
                subtotal.Yield(context).ReturnsForAnyArgs(150);
                percentOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
                cart.Adjustments.Should().BeEmpty();
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(131.25M);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_DecimalGreaterThanEqualToOperator_False(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
                globalPricingPolicy.ShouldRoundPriceCalc = false;
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                var cartline = cart.Lines[1];
                var cartTotals = new CartTotals(cart);
                subtotal.Yield(context).ReturnsForAnyArgs(150);
                percentOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(100);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_ShouldRoundPriceCalc_True(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
                globalPricingPolicy.ShouldRoundPriceCalc = true;
                globalPricingPolicy.RoundDigits = 3;
                globalPricingPolicy.MidPointRoundUp = true;
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                var cartline = cart.Lines[1];
                cartline.Totals.SubTotal.Amount = 175;
                var cartTotals = new CartTotals(cart);
                subtotal.Yield(context).ReturnsForAnyArgs(150);
                percentOff.Yield(context).ReturnsForAnyArgs(33.3333333M);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
                cart.Adjustments.Should().BeEmpty();
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(116.667M);
                cartline.Adjustments.Should().NotBeEmpty();
                cartline.Adjustments.FirstOrDefault().Should().NotBeNull();
                cartline.Adjustments.FirstOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
                cartline.Adjustments.FirstOrDefault()?.Name.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.DisplayName.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.AwardingBlock.Should().Contain(nameof(BaseCartItemSubtotalPercentOffAction));
                cartline.Adjustments.FirstOrDefault()?.IsTaxable.Should().BeFalse();
                cartline.Adjustments.FirstOrDefault()?.Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
                cartline.Adjustments.FirstOrDefault()?.Adjustment.Amount.Should().Be(-58.333M);
                cartline.HasComponent<MessagesComponent>().Should().BeTrue();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_WithProperties(
                BaseCartItemSubtotalPercentOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> percentOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
                globalPricingPolicy.ShouldRoundPriceCalc = false;
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                var cartline = cart.Lines[1];
                cartline.Totals.SubTotal.Amount = 150;
                var cartTotals = new CartTotals(cart);
                subtotal.Yield(context).ReturnsForAnyArgs(150);
                percentOff.Yield(context).ReturnsForAnyArgs(25.55M);
                var propertiesModel = new PropertiesModel();
                propertiesModel.Properties.Add("PromotionId", "id");
                propertiesModel.Properties.Add("PromotionCartText", "carttext");
                propertiesModel.Properties.Add("PromotionText", "text");
                commerceContext.AddObject(propertiesModel);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
                cart.Adjustments.Should().BeEmpty();
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(111.6750M);
                cartline.Adjustments.Should().NotBeEmpty();
                cartline.Adjustments.FirstOrDefault().Should().NotBeNull();
                cartline.Adjustments.FirstOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
                cartline.Adjustments.FirstOrDefault()?.Name.Should().Be("text");
                cartline.Adjustments.FirstOrDefault()?.DisplayName.Should().Be("carttext");
                cartline.Adjustments.FirstOrDefault()?.AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.AwardingBlock.Should().Contain(nameof(BaseCartItemSubtotalPercentOffAction));
                cartline.Adjustments.FirstOrDefault()?.IsTaxable.Should().BeFalse();
                cartline.Adjustments.FirstOrDefault()?.Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
                cartline.Adjustments.FirstOrDefault()?.Adjustment.Amount.Should().Be(-38.325M);
                cartline.HasComponent<MessagesComponent>().Should().BeTrue();
            }
        }
    }
}
