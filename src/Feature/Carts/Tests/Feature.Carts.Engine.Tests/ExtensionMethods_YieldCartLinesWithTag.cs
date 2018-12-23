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
    public class ExtensionMethods_YieldCartLinesWithTag
    {

        [Theory, AutoNSubstituteData]
        public void MatchingLines_01_NoCommerceContext(
        IRuleValue<string> targetTag,
        IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);
            targetTag.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_02_NoCart(
            IRuleValue<string> targetTag,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            targetTag.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_03_NoCartLines(
            IRuleValue<string> targetTag,
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
            targetTag.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_04_NoLineItemComponent(
            IRuleValue<string> targetTag,
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            targetTag.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_05_NoTag(
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            IRuleValue<string> targetTag = null;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_06_EmptyTag(
            IRuleValue<string> targetTag,
            Cart cart,
            CartProductComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.Tags.Add(new Tag("Smartphone"));
            cart.Lines[1].SetComponent(component);
            commerceContext.AddObject(cart);
            targetTag.Yield(context).ReturnsForAnyArgs("");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_07_Single(
            IRuleValue<string> targetTag,
            Cart cart,
            CartProductComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.Tags.Add(new Tag("Smartphone"));
            cart.Lines[1].SetComponent(component);
            commerceContext.AddObject(cart);
            targetTag.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            var matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            matchingLines.Should().HaveCount(1);
        }

        [Theory, AutoNSubstituteData]
        public void MatchingLines_08_Multiple(
            IRuleValue<string> targetTag,
            Cart cart,
            CartProductComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.Tags.Add(new Tag("Smartphone")); ;
            cart.Lines.ForEach(l => l.SetComponent(component));
            commerceContext.AddObject(cart);
            targetTag.Yield(context).ReturnsForAnyArgs("Smartphone");

            /**********************************************
             * Act
             **********************************************/
            var matchingLines = targetTag.YieldCartLinesWithTag(context);

            /**********************************************
             * Assert
             **********************************************/
            matchingLines.Should().HaveCount(3);
        }
    }
}
