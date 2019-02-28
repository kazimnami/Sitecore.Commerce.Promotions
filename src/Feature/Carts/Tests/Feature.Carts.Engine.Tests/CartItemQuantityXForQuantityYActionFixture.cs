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
    public class CartItemQuantityXForQuantityYActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
            CartItemQuantityXForQuantityYAction action,
            IRuleValue<string> targetItemId,
            IRuleValue<int> quantityX,
            IRuleValue<int> quantityY,
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
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_03_NoCartLines(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                CartItemQuantityXForQuantityYAction action,
                //IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_07_NoQuantityX(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                //IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                action.QuantityX = null;
                action.QuantityY = quantityY;

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
            public void Execute_08_ZeroQuantityX(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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

                quantityX.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_09_NoQuantityY(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                //IRuleValue<int> quantityY,
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
                action.QuantityX = quantityX;
                action.QuantityY = null;

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
            public void Execute_10_ZeroQuantityY(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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

                quantityY.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_11_EqualQuantityXAndQuantityY(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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

                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(4);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_12_NoPurchaseOptionMoneyPolicy(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                while (cart.Lines.Count > 1)
                {
                    cart.Lines.RemoveAt(0);
                }
                var cartline = cart.Lines[0];
                cartline.Quantity = 4;
                cartline.Totals.SubTotal.Amount = 100;
                cartline.ItemId = "Habitat_Master|12345|";
                var cartTotals = new CartTotals(cart);
                targetItemId.Yield(context).ReturnsForAnyArgs("Habitat_Master|12345|");
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_TargetItemId_NoMatch(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                while (cart.Lines.Count > 1)
                {
                    cart.Lines.RemoveAt(0);
                }
                var cartline = cart.Lines[0];
                cartline.Quantity = 7;
                cartline.Totals.SubTotal.Amount = 175;
                cartline.ItemId = "Habitat_Master|12345|";
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                targetItemId.Yield(context).ReturnsForAnyArgs("Habitat_Master|54321|");
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(175);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_Zero_UpperLimit(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                while (cart.Lines.Count > 1)
                {
                    cart.Lines.RemoveAt(0);
                }
                var cartline = cart.Lines[0];
                cartline.Quantity = 3;
                cartline.Totals.SubTotal.Amount = 75;
                cartline.ItemId = "Habitat_Master|12345|";
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                targetItemId.Yield(context).ReturnsForAnyArgs("Habitat_Master|12345|");
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(75);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_One_LowerLimit(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                while (cart.Lines.Count > 1)
                {
                    cart.Lines.RemoveAt(0);
                }
                var cartline = cart.Lines[0];
                cartline.Quantity = 4;
                cartline.Totals.SubTotal.Amount = 100;
                cartline.ItemId = "Habitat_Master|12345|";
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                targetItemId.Yield(context).ReturnsForAnyArgs("Habitat_Master|12345|");
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(75);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_One_UpperLimit(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                while (cart.Lines.Count > 1)
                {
                    cart.Lines.RemoveAt(0);
                }
                var cartline = cart.Lines[0];
                cartline.Quantity = 7;
                cartline.Totals.SubTotal.Amount = 175;
                cartline.ItemId = "Habitat_Master|12345|";
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                targetItemId.Yield(context).ReturnsForAnyArgs("Habitat_Master|12345|");
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
            public void Execute_TimesQualified_Two_LowerLimit(
                CartItemQuantityXForQuantityYAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<int> quantityY,
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
                while (cart.Lines.Count > 1)
                {
                    cart.Lines.RemoveAt(0);
                }
                var cartline = cart.Lines[0];
                cartline.Quantity = 8;
                cartline.Totals.SubTotal.Amount = 200;
                cartline.ItemId = "Habitat_Master|12345|";
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                targetItemId.Yield(context).ReturnsForAnyArgs("Habitat_Master|12345|");
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.TargetItemId = targetItemId;
                action.QuantityX = quantityX;
                action.QuantityY = quantityY;

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
        }
    }
}
