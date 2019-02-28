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
    public class CartAnyItemQuantityXForQuantityYActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
            CartAnyItemQuantityXForQuantityYAction action,
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
                CartAnyItemQuantityXForQuantityYAction action,
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
                CartAnyItemQuantityXForQuantityYAction action,
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
                CartAnyItemQuantityXForQuantityYAction action,
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
                CartAnyItemQuantityXForQuantityYAction action,
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
            public void Execute_06_NoQuantityX(
                CartAnyItemQuantityXForQuantityYAction action,
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
            public void Execute_07_ZeroQuantityX(
                CartAnyItemQuantityXForQuantityYAction action,
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
            public void Execute_08_NoQuantityY(
                CartAnyItemQuantityXForQuantityYAction action,
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
            public void Execute_09_ZeroQuantityY(
                CartAnyItemQuantityXForQuantityYAction action,
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
            public void Execute_10_EqualQuantityXAndQuantityY(
                CartAnyItemQuantityXForQuantityYAction action,
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
            public void Execute_11_NoPurchaseOptionMoneyPolicy(
                CartAnyItemQuantityXForQuantityYAction action,
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
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
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
            public void Execute_TimesQualified_Zero_UpperLimit(
                CartAnyItemQuantityXForQuantityYAction action,
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
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
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
                CartAnyItemQuantityXForQuantityYAction action,
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
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
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
                CartAnyItemQuantityXForQuantityYAction action,
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
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
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
                CartAnyItemQuantityXForQuantityYAction action,
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
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(25)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(4);
                quantityY.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
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
