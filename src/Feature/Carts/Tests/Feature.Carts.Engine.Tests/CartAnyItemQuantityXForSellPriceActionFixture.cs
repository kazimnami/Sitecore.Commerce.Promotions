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
    public class CartAnyItemQuantityXForSellPriceActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<string> targetItemId,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                //IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
            public void Execute_08_NoSellPrice(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                //IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                action.SellPrice = null;
                action.MaximumApplications = maximumApplications;

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
            public void Execute_09_ZeroSellPrice(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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

                sellPrice.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
            public void Execute_10_NoPurchaseOptionMoneyPolicy(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                sellPrice.Yield(context).ReturnsForAnyArgs(3);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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

            [Theory, AutoNSubstituteData]
            public void Execute_11_NoMaximumApplications(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                //IRuleValue<int> maximumApplications,
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
                action.SellPrice = sellPrice;
                action.MaximumApplications = null;

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

        }
        public class Functional
        {
            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_Zero_UpperLimit(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                cartline.Quantity = 1;
                cartline.Totals.SubTotal.Amount = 75;
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(75)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(2);
                sellPrice.Yield(context).ReturnsForAnyArgs(20);
                maximumApplications.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                cartline.Quantity = 2;
                cartline.Totals.SubTotal.Amount = 150;
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(75)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(2);
                sellPrice.Yield(context).ReturnsForAnyArgs(40);
                maximumApplications.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(40);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_One_UpperLimit(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                cartline.Totals.SubTotal.Amount = 225;
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(75)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(2);
                sellPrice.Yield(context).ReturnsForAnyArgs(40);
                maximumApplications.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(115);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_Two_LowerLimit(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                cartline.Totals.SubTotal.Amount = 300;
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(75)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(2);
                sellPrice.Yield(context).ReturnsForAnyArgs(40);
                maximumApplications.Yield(context).ReturnsForAnyArgs(0);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(80);
            }

            [Theory, AutoNSubstituteData]
            public void Execute_TimesQualified_Two_MaximumApplications_One(
                CartAnyItemQuantityXSellPriceAction action,
                IRuleValue<int> quantityX,
                IRuleValue<decimal> sellPrice,
                IRuleValue<int> maximumApplications,
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
                cartline.Totals.SubTotal.Amount = 300;
                cartline.SetPolicy(new PurchaseOptionMoneyPolicy
                {
                    SellPrice = new Money(75)
                });
                var cartTotals = new CartTotals(cart);
                quantityX.Yield(context).ReturnsForAnyArgs(2);
                sellPrice.Yield(context).ReturnsForAnyArgs(40);
                maximumApplications.Yield(context).ReturnsForAnyArgs(1);

                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);
                action.QuantityX = quantityX;
                action.SellPrice = sellPrice;
                action.MaximumApplications = maximumApplications;

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
                cartTotals.Lines[cartline.Id].SubTotal.Amount.Should().Be(190);
            }
        }
    }
}
