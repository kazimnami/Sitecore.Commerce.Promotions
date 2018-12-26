using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using Xunit;

namespace Feature.Carts.Engine.Tests
{
    public class ExtensionMethods_YieldCartLinesWithBrand
    {

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_01_NoCommerceContext(
        IRuleValue<string> targetBrand,
        IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);
            targetBrand.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_02_NoCart(
            IRuleValue<string> targetBrand,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            targetBrand.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_03_NoCartLines(
            IRuleValue<string> targetBrand,
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            cart.Lines.Clear();

            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            targetBrand.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_04_NoLineItemComponent(
            IRuleValue<string> targetBrand,
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            targetBrand.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_05_NoTargetBrand(
            CartItemTargetBrandSubtotalAmountOffAction action,
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            IRuleValue<string> targetBrand = null;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_06_EmptyTargetBrand(
            IRuleValue<string> targetBrand,
            Cart cart,
            LineItemProductExtendedComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.Brand = "Smartphone";
            cart.Lines[1].SetComponent(component);
            commerceContext.AddObject(cart);
            targetBrand.Yield(context).ReturnsForAnyArgs("");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_07_Single(
            IRuleValue<string> targetBrand,
            Cart cart,
            LineItemProductExtendedComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.Brand = "Smartphone";
            cart.Lines[1].SetComponent(component);
            commerceContext.AddObject(cart);
            targetBrand.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            var matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            matchingLines.Should().HaveCount(1);
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_08_Multiple(
            IRuleValue<string> targetBrand,
            Cart cart,
            LineItemProductExtendedComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.Brand = "Smartphone";
            cart.Lines.ForEach(l => l.SetComponent(component));
            commerceContext.AddObject(cart);
            targetBrand.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            var matchingLines = targetBrand.YieldCartLinesWithBrand(context);

            /**********************************************
             * Assert
             **********************************************/
            matchingLines.Should().HaveCount(3);
        }
    }
}
