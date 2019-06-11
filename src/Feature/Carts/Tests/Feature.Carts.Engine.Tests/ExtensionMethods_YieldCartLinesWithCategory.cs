using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using Xunit;

namespace SamplePromotions.Feature.Carts.Engine.Tests
{
    public class ExtensionMethods_YieldCartLinesWithCategory
    {

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_01_NoCommerceContext(
        IRuleValue<string> targetCategorySitecoreId,
        IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("c2bfaf91-7825-4846-0ad3-0479cdf7b607");;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_02_NoCart(
            IRuleValue<string> targetCategorySitecoreId,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("c2bfaf91-7825-4846-0ad3-0479cdf7b607");;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_03_NoCartLines(
            IRuleValue<string> targetCategorySitecoreId,
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
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("c2bfaf91-7825-4846-0ad3-0479cdf7b607");;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_04_NoLineItemComponent(
            IRuleValue<string> targetCategorySitecoreId,
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("c2bfaf91-7825-4846-0ad3-0479cdf7b607");;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_05_NoCategory(
            Cart cart,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            commerceContext.AddObject(cart);
            IRuleValue<string> targetCategorySitecoreId = null;

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_06_EmptyCategory(
            IRuleValue<string> targetCategorySitecoreId,
            Cart cart,
            LineItemProductExtendedComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.ParentCategoryList = "8e456d84-4251-dba1-4b86-ce103dedcd02|c2bfaf91-7825-4846-0ad3-0479cdf7b607";
            cart.Lines[1].SetComponent(component);
            commerceContext.AddObject(cart);
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("");

            /**********************************************
             * Act
             **********************************************/
            IEnumerable<CartLineComponent> matchingLines = null;
            Action executeAction = () => matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            executeAction.Should().NotThrow<Exception>();
            matchingLines.Should().BeEmpty();
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_07_Single(
            IRuleValue<string> targetCategorySitecoreId,
            Cart cart,
            LineItemProductExtendedComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.ParentCategoryList = "8e456d84-4251-dba1-4b86-ce103dedcd02|c2bfaf91-7825-4846-0ad3-0479cdf7b607";
            cart.Lines[1].SetComponent(component);
            commerceContext.AddObject(cart);
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("c2bfaf91-7825-4846-0ad3-0479cdf7b607");;

            /**********************************************
             * Act
             **********************************************/
            var matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            matchingLines.Should().HaveCount(1);
        }

        [Theory, AutoNSubstituteData]
        public void YieldCartLines_08_Multiple(
            IRuleValue<string> targetCategorySitecoreId,
            Cart cart,
            LineItemProductExtendedComponent component,
            CommerceContext commerceContext,
            IRuleExecutionContext context)
        {
            /**********************************************
             * Arrange
             **********************************************/
            context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
            component.ParentCategoryList = "8e456d84-4251-dba1-4b86-ce103dedcd02|c2bfaf91-7825-4846-0ad3-0479cdf7b607";
            cart.Lines.ForEach(l => l.SetComponent(component));
            commerceContext.AddObject(cart);
            targetCategorySitecoreId.Yield(context).ReturnsForAnyArgs("c2bfaf91-7825-4846-0ad3-0479cdf7b607");

            /**********************************************
             * Act
             **********************************************/
            var matchingLines = targetCategorySitecoreId.YieldCartLinesWithCategory(context);

            /**********************************************
             * Assert
             **********************************************/
            matchingLines.Should().HaveCount(3);
        }
    }
}
