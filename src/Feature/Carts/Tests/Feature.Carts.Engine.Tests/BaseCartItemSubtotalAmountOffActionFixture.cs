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

namespace Feature.Carts.Engine.Tests
{
    public class BaseCartItemSubtotalAmountOffActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_1_NoCommerceContext(
            BaseCartItemSubtotalAmountOffAction action,
            IBinaryOperator<decimal, decimal> subtotalOperator,
            IRuleValue<decimal> subtotal,
            IRuleValue<decimal> amountOff,
            Cart cart,
            //CartTotals cartTotals,
            //CommerceContext commerceContext,
            IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);
                //commerceContext.AddObject(cartTotals);
                //commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
            public void Execute_2_NoCart(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
                //Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                //cart.Adjustments.Clear();
                //cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                //commerceContext.AddObject(cart);
                //action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                //cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                //cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_3_NoCartLines(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                //cart.Lines.ForEach(l => l.Adjustments.Clear());
                cart.Lines.Clear();

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
            public void Execute_4_NoCartTotals(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
                Cart cart,
                //CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                //commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = subtotalOperator;
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
            public void Execute_5_NoCartTotalLines(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                action.AmountOff = amountOff;

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
            public void Execute_6_NoSubtotalOperator(
                BaseCartItemSubtotalAmountOffAction action,
                //IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                action.AmountOff = amountOff;

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
            public void Execute_7_NoSubtotal(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                //IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                action.AmountOff = amountOff;

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
            public void Execute_8_NoAmount(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                //IRuleValue<decimal> amountOff,
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
                action.AmountOff = null;

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
            public void Execute_9_NoMatchingLines_FromAbstraction(
                BaseCartItemSubtotalAmountOffAction action,
                IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                action.AmountOff = amountOff;

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
                BaseCartItemSubtotalAmountOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                amountOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalEqualityOperator();
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(125);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_DecimalEqualityOperator_False(
                BaseCartItemSubtotalAmountOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                amountOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalEqualityOperator();
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
                BaseCartItemSubtotalAmountOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                amountOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(150);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_DecimalGreaterThanEqualToOperator_False(
                BaseCartItemSubtotalAmountOffAction action,
                //IBinaryOperator<decimal, decimal> subtotalOperator,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                amountOff.Yield(context).ReturnsForAnyArgs(25);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
                BaseCartItemSubtotalAmountOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                amountOff.Yield(context).ReturnsForAnyArgs(25.555555M);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
                action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
                action.Subtotal = subtotal;
                action.AmountOff = amountOff;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(149.444M);
                cartline.Adjustments.Should().NotBeEmpty();
                cartline.Adjustments.FirstOrDefault().Should().NotBeNull();
                cartline.Adjustments.FirstOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
                cartline.Adjustments.FirstOrDefault()?.Name.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.DisplayName.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.AwardingBlock.Should().Contain(nameof(BaseCartItemSubtotalAmountOffAction));
                cartline.Adjustments.FirstOrDefault()?.IsTaxable.Should().BeFalse();
                cartline.Adjustments.FirstOrDefault()?.Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
                cartline.Adjustments.FirstOrDefault()?.Adjustment.Amount.Should().Be(-25.556M);
                cartline.HasComponent<MessagesComponent>().Should().BeTrue();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_WithProperties(
                BaseCartItemSubtotalAmountOffAction action,
                IRuleValue<decimal> subtotal,
                IRuleValue<decimal> amountOff,
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
                amountOff.Yield(context).ReturnsForAnyArgs(25);
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
                action.AmountOff = amountOff;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(125);
                cartline.Adjustments.Should().NotBeEmpty();
                cartline.Adjustments.FirstOrDefault().Should().NotBeNull();
                cartline.Adjustments.FirstOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
                cartline.Adjustments.FirstOrDefault()?.Name.Should().Be("text");
                cartline.Adjustments.FirstOrDefault()?.DisplayName.Should().Be("carttext");
                cartline.Adjustments.FirstOrDefault()?.AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
                cartline.Adjustments.FirstOrDefault()?.AwardingBlock.Should().Contain(nameof(BaseCartItemSubtotalAmountOffAction));
                cartline.Adjustments.FirstOrDefault()?.IsTaxable.Should().BeFalse();
                cartline.Adjustments.FirstOrDefault()?.Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
                cartline.Adjustments.FirstOrDefault()?.Adjustment.Amount.Should().Be(-25);
                cartline.HasComponent<MessagesComponent>().Should().BeTrue();
            }
        }
    }
}
