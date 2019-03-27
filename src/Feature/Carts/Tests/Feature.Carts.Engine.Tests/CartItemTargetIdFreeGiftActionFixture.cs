using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Carts.Engine.Actions;
using Feature.Carts.Engine.Commands;
using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using Xunit;

namespace Feature.Carts.Engine.Tests
{
    public class CartItemTargetIdFreeGiftActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
                IRuleValue<string> targetItemId,
                Cart cart,
                CartItemTargetIdFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                action.TargetItemId = targetItemId;
                
                context.Fact(Arg.Any<IFactIdentifier>()).Returns((CommerceContext) null);

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
                IRuleValue<string> targetItemId,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetIdFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                commerceContext.AddObject(cartTotals);
                action.TargetItemId = targetItemId;

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

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
                IRuleValue<string> targetItemId,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetIdFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.Clear();

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                action.TargetItemId = targetItemId;

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

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
                IRuleValue<string> targetItemId,
                Cart cart,
                CommerceContext commerceContext,
                CartItemTargetIdFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                commerceContext.AddObject(cart);

                action.TargetItemId = targetItemId;
                
                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

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
                IRuleValue<string> targetItemId,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetIdFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                cartTotals.Lines.Clear();

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                action.TargetItemId = targetItemId;
                
                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

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

        public class Functional : CartItemTargetIdFreeGiftAction
        {
            public Functional() : base(Substitute.For<IApplyFreeGiftDiscountCommand>(),
                Substitute.For<IApplyFreeGiftEligibilityCommand>(),
                Substitute.For<IApplyFreeGiftAutoRemoveCommand>(),
                Substitute.For<AddCartLineCommand>(Substitute.For<IFindEntityPipeline>(), Substitute.For<IAddCartLinePipeline>(), Substitute.For<IServiceProvider>()))
            {
            }

            [Theory, InlineAutoNSubstituteData("Habitat_Master|6042175|56042175")]
            public void Execute_HasMatchingLines_ShouldApplyCartLineAdjustment(
                string targetItemId,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var action = this;

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                cart.Lines[0].ItemId = targetItemId;

                action.TargetItemId = Substitute.For<IRuleValue<string>>();
                action.TargetItemId.Yield(context).ReturnsForAnyArgs(targetItemId);

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                ApplyFreeGiftEligibilityCommand.Received().Process(Arg.Any<CommerceContext>(), cart, Arg.Any<string>());
                ApplyFreeGiftDiscountCommand.Received().Process(Arg.Any<CommerceContext>(), Arg.Any<CartLineComponent>(), Arg.Any<string>());
            }

            [Theory, InlineAutoNSubstituteData("Habitat_Master|6042175|56042175")]
            public void Execute_NoMatchingLines_ShouldNotApplyCartLineAdjustment(
                string targetItemId,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var action = this;

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                action.TargetItemId = Substitute.For<IRuleValue<string>>();
                action.TargetItemId.Yield(context).ReturnsForAnyArgs(targetItemId);

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                ApplyFreeGiftDiscountCommand.DidNotReceive().Process(Arg.Any<CommerceContext>(), Arg.Any<CartLineComponent>(), Arg.Any<string>());
            }
        }
    }
}
