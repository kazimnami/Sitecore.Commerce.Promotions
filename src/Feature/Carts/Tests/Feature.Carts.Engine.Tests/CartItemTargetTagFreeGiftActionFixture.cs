using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Carts.Engine.Actions;
using Feature.Carts.Engine.Commands;
using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using Xunit;

namespace Feature.Carts.Engine.Tests
{
    public class CartItemTargetTagFreeGiftActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
                IRuleValue<string> targetTag,
                Cart cart,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                action.TargetTag = targetTag;
                
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
                IRuleValue<string> targetTag,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                commerceContext.AddObject(cartTotals);
                action.TargetTag = targetTag;

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
                IRuleValue<string> targetTag,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.Clear();

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                action.TargetTag = targetTag;
                
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
                IRuleValue<string> targetTag,
                Cart cart,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                commerceContext.AddObject(cart);

                action.TargetTag = targetTag;

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
                IRuleValue<string> targetTag,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
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

                action.TargetTag = targetTag;

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

        public class Functional : CartItemTargetTagFreeGiftAction
        {
            public Functional() : base(Substitute.For<IApplyFreeGiftDiscountCommand>(),
                Substitute.For<IApplyFreeGiftEligibilityCommand>(),
                Substitute.For<IApplyFreeGiftAutoRemoveCommand>())
            {
            }

            [Theory, InlineAutoNSubstituteData("freegift")]
            public void Execute_HasMatchingLines_ShouldApplyCartLineAdjustment(
                string targetTag,
                bool autoRemove,
                Cart cart,
                CartProductComponent cartProductComponent,
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

                cartProductComponent.Tags.Add(new Tag(targetTag));

                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                cart.Lines[0].SetComponent(cartProductComponent);

                action.TargetTag = Substitute.For<IRuleValue<string>>();
                action.TargetTag.Yield(context).ReturnsForAnyArgs(targetTag);

                action.AutoRemove = Substitute.For<IRuleValue<bool>>();
                action.AutoRemove.Yield(context).ReturnsForAnyArgs(autoRemove);

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

            [Theory, InlineAutoNSubstituteData("freegift")]
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

                action.TargetTag = Substitute.For<IRuleValue<string>>();
                action.TargetTag.Yield(context).ReturnsForAnyArgs(targetItemId);

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
                ApplyFreeGiftDiscountCommand.DidNotReceive().Process(Arg.Any<CommerceContext>(), Arg.Any<CartLineComponent>(), Arg.Any<string>());
            }
        }
    }
}
