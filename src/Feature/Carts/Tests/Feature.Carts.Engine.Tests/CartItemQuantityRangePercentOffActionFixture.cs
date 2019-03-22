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

namespace Feature.Carts.Engine.Tests
{
    public class BaseCartItemSubtotalAmountOffActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_06_NoTargetItemId(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = null;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_07_NoMinQuantity(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = null;
                action.MaxQuantity = maxQuantity;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().Throw<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_08_NoMaxQuantity(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = null;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().Throw<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_09_NoPercentOff(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
                action.PercentOff = null;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().Throw<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_10_ZeroMinQuantity(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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

                minQuantity.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_11_ZeroMaxQuantity(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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

                maxQuantity.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_12_MinQuantityGreaterThanMaxQuantity(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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

                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(1);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_13_NoMatchingLines(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_14_NoPurchaseOptionMoneyPolicy(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                targetItemId.Yield(context).Returns("Habitat_Master|12345|");
                var index = 12345;
                cart.Lines.ForEach(line =>
                {
                    line.ItemId = $"Habitat_Master|{index++}|";
                    line.Quantity = 2;
                    line.Totals.SubTotal.Amount = 150;
                });

                var cartTotals = new CartTotals(cart);
                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(3);
                percentOff.Yield(context).ReturnsForAnyArgs(10);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
                action.PercentOff = percentOff;

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(0);
                cart.Adjustments.Should().BeEmpty();
            }
        }
        public class Functional
        {
            [Theory, AutoNSubstituteData]
            public void Execute_QuantityInRange_LowerLimit(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                targetItemId.Yield(context).Returns("Habitat_Master|12345|");
                var index = 12345;
                cart.Lines.ForEach(line =>
                {
                    line.ItemId = $"Habitat_Master|{index++}|";
                    line.Quantity = 2;
                    line.Totals.SubTotal.Amount = 150;
                    line.SetPolicy(new PurchaseOptionMoneyPolicy());
                });

                var cartTotals = new CartTotals(cart);
                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(3);
                percentOff.Yield(context).ReturnsForAnyArgs(10);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            }

            [Theory, AutoNSubstituteData]
            public void Execute_QuantityInRange_UpperLimit(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                targetItemId.Yield(context).Returns("Habitat_Master|12345|");
                var index = 12345;
                cart.Lines.ForEach(line =>
                {
                    line.ItemId = $"Habitat_Master|{index++}|";
                    line.Quantity = 3;
                    line.Totals.SubTotal.Amount = 150;
                    line.SetPolicy(new PurchaseOptionMoneyPolicy());
                });

                var cartTotals = new CartTotals(cart);
                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(3);
                percentOff.Yield(context).ReturnsForAnyArgs(10);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            }

            [Theory, AutoNSubstituteData]
            public void Execute_QuantityOutOfRange_LowerLimit(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                var index = 12345;
                cart.Lines.ForEach(line =>
                {
                    line.ItemId = $"Habitat_Master|{index++}|";
                    line.Quantity = 1;
                    line.Totals.SubTotal.Amount = 150;
                    line.SetPolicy(new PurchaseOptionMoneyPolicy());
                });

                var cartTotals = new CartTotals(cart);
                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_QuantityOutOfRange_UpperLimit(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                var index = 12345;
                cart.Lines.ForEach(line =>
                {
                    line.ItemId = $"Habitat_Master|{index++}|";
                    line.Quantity = 4;
                    line.Totals.SubTotal.Amount = 150;
                    line.SetPolicy(new PurchaseOptionMoneyPolicy());
                });

                var cartTotals = new CartTotals(cart);
                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
            public void Execute_PercentOff(
                CartItemQuantityRangePercentOffAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<decimal> minQuantity,
                IRuleValue<decimal> maxQuantity,
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
                cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
                targetItemId.Yield(context).Returns("Habitat_Master|12345|");
                var index = 12345;
                cart.Lines.ForEach(line =>
                {
                    line.ItemId = $"Habitat_Master|{index++}|";
                    line.Quantity = 2;
                    line.Totals.SubTotal.Amount = 150;
                    line.SetPolicy(new PurchaseOptionMoneyPolicy());
                });
                var cartTotals = new CartTotals(cart);
                minQuantity.Yield(context).ReturnsForAnyArgs(2);
                maxQuantity.Yield(context).ReturnsForAnyArgs(3);
                percentOff.Yield(context).ReturnsForAnyArgs(10);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.MinQuantity = minQuantity;
                action.MaxQuantity = maxQuantity;
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
                cartTotals.Lines[cart.Lines[0].Id].SubTotal.Amount.Should().Be(135);
            }

            //[Theory, AutoNSubstituteData]
            //public void Execute_DecimalGreaterThanEqualToOperator_False(
            //    CartItemQuantityRangePercentOffAction action,
            //    IRuleValue<string> targetItemId,
            //    IRuleValue<decimal> minQuantity,
            //    IRuleValue<decimal> maxQuantity,
            //    IRuleValue<decimal> percentOff,
            //    Cart cart,
            //    CommerceContext commerceContext,
            //    IRuleExecutionContext context)
            //{
            //    /**********************************************
            //     * Arrange
            //     **********************************************/
            //    cart.Adjustments.Clear();
            //    cart.Lines.ForEach(l => l.Adjustments.Clear());

            //    var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
            //    globalPricingPolicy.ShouldRoundPriceCalc = false;
            //    cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
            //    var cartline = cart.Lines[1];
            //    var cartTotals = new CartTotals(cart);
            //    subtotal.Yield(context).ReturnsForAnyArgs(150);
            //    amountOff.Yield(context).ReturnsForAnyArgs(25);

            //    context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            //    commerceContext.AddObject(cartTotals);
            //    commerceContext.AddObject(cart);
            //    action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
            //    action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
            //    action.Subtotal = subtotal;
            //    action.AmountOff = amountOff;

            //    /**********************************************
            //     * Act
            //     **********************************************/
            //    Action executeAction = () => action.Execute(context);

            //    /**********************************************
            //     * Assert
            //     **********************************************/
            //    executeAction.Should().NotThrow<Exception>();
            //    cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
            //    cart.Adjustments.Should().BeEmpty();
            //    cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(100);
            //}

            //[Theory, AutoNSubstituteData]
            //public void Execute_ShouldRoundPriceCalc_True(
            //    CartItemQuantityRangePercentOffAction action,
            //    IRuleValue<string> targetItemId,
            //    IRuleValue<decimal> minQuantity,
            //    IRuleValue<decimal> maxQuantity,
            //    IRuleValue<decimal> percentOff,
            //    Cart cart,
            //    CommerceContext commerceContext,
            //    IRuleExecutionContext context)
            //{
            //    /**********************************************
            //     * Arrange
            //     **********************************************/
            //    cart.Adjustments.Clear();
            //    cart.Lines.ForEach(l => l.Adjustments.Clear());

            //    var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
            //    globalPricingPolicy.ShouldRoundPriceCalc = true;
            //    globalPricingPolicy.RoundDigits = 3;
            //    globalPricingPolicy.MidPointRoundUp = true;
            //    cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
            //    var cartline = cart.Lines[1];
            //    cartline.Totals.SubTotal.Amount = 175;
            //    var cartTotals = new CartTotals(cart);
            //    subtotal.Yield(context).ReturnsForAnyArgs(150);
            //    amountOff.Yield(context).ReturnsForAnyArgs(25.555555M);

            //    context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            //    commerceContext.AddObject(cartTotals);
            //    commerceContext.AddObject(cart);
            //    action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
            //    action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
            //    action.Subtotal = subtotal;
            //    action.AmountOff = amountOff;

            //    /**********************************************
            //     * Act
            //     **********************************************/
            //    Action executeAction = () => action.Execute(context);

            //    /**********************************************
            //     * Assert
            //     **********************************************/
            //    executeAction.Should().NotThrow<Exception>();
            //    cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
            //    cart.Adjustments.Should().BeEmpty();
            //    cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(149.444M);
            //    cartline.Adjustments.Should().NotBeEmpty();
            //    cartline.Adjustments.FirstOrDefault().Should().NotBeNull();
            //    cartline.Adjustments.FirstOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
            //    cartline.Adjustments.FirstOrDefault()?.Name.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
            //    cartline.Adjustments.FirstOrDefault()?.DisplayName.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
            //    cartline.Adjustments.FirstOrDefault()?.AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
            //    cartline.Adjustments.FirstOrDefault()?.AwardingBlock.Should().Contain(nameof(BaseCartItemSubtotalAmountOffAction));
            //    cartline.Adjustments.FirstOrDefault()?.IsTaxable.Should().BeFalse();
            //    cartline.Adjustments.FirstOrDefault()?.Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
            //    cartline.Adjustments.FirstOrDefault()?.Adjustment.Amount.Should().Be(-25.556M);
            //    cartline.HasComponent<MessagesComponent>().Should().BeTrue();
            //}

            //[Theory, AutoNSubstituteData]
            //public void Execute_WithProperties(
            //    CartItemQuantityRangePercentOffAction action,
            //    IRuleValue<string> targetItemId,
            //    IRuleValue<decimal> minQuantity,
            //    IRuleValue<decimal> maxQuantity,
            //    IRuleValue<decimal> percentOff,
            //    Cart cart,
            //    CommerceContext commerceContext,
            //    IRuleExecutionContext context)
            //{
            //    /**********************************************
            //     * Arrange
            //     **********************************************/
            //    cart.Adjustments.Clear();
            //    cart.Lines.ForEach(l => l.Adjustments.Clear());

            //    var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
            //    globalPricingPolicy.ShouldRoundPriceCalc = false;
            //    cart.Lines.ForEach(l => l.Totals.SubTotal.Amount = 100);
            //    var cartline = cart.Lines[1];
            //    cartline.Totals.SubTotal.Amount = 150;
            //    var cartTotals = new CartTotals(cart);
            //    subtotal.Yield(context).ReturnsForAnyArgs(150);
            //    amountOff.Yield(context).ReturnsForAnyArgs(25);
            //    var propertiesModel = new PropertiesModel();
            //    propertiesModel.Properties.Add("PromotionId", "id");
            //    propertiesModel.Properties.Add("PromotionCartText", "carttext");
            //    propertiesModel.Properties.Add("PromotionText", "text");
            //    commerceContext.AddObject(propertiesModel);

            //    context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            //    commerceContext.AddObject(cartTotals);
            //    commerceContext.AddObject(cart);
            //    action.MatchingLines(context).ReturnsForAnyArgs(cart.Lines);
            //    action.SubtotalOperator = new DecimalGreaterThanEqualToOperator();
            //    action.Subtotal = subtotal;
            //    action.AmountOff = amountOff;

            //    /**********************************************
            //     * Act
            //     **********************************************/
            //    Action executeAction = () => action.Execute(context);

            //    /**********************************************
            //     * Assert
            //     **********************************************/
            //    executeAction.Should().NotThrow<Exception>();
            //    cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
            //    cart.Adjustments.Should().BeEmpty();
            //    cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(125);
            //    cartline.Adjustments.Should().NotBeEmpty();
            //    cartline.Adjustments.FirstOrDefault().Should().NotBeNull();
            //    cartline.Adjustments.FirstOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
            //    cartline.Adjustments.FirstOrDefault()?.Name.Should().Be("text");
            //    cartline.Adjustments.FirstOrDefault()?.DisplayName.Should().Be("carttext");
            //    cartline.Adjustments.FirstOrDefault()?.AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
            //    cartline.Adjustments.FirstOrDefault()?.AwardingBlock.Should().Contain(nameof(BaseCartItemSubtotalAmountOffAction));
            //    cartline.Adjustments.FirstOrDefault()?.IsTaxable.Should().BeFalse();
            //    cartline.Adjustments.FirstOrDefault()?.Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
            //    cartline.Adjustments.FirstOrDefault()?.Adjustment.Amount.Should().Be(-25);
            //    cartline.HasComponent<MessagesComponent>().Should().BeTrue();
            //}
        }
    }
}
